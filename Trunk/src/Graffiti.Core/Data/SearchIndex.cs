using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Search.Highlight;
using Lucene.Net.Search.Similar;
using Lucene.Net.Store;
using Lucene.Net.Util;
using LQ = Lucene.Net.QueryParsers;
using Version = Lucene.Net.Util.Version;

namespace Graffiti.Core
{
	public static class SearchFields
	{
		public const string Title = "title";
		public const string Body = "body";
		public const string Tag = "tag";
	}

	/// <summary>
	///     Provides an in memory search index for all the posts in a Graffiti Site.
	/// </summary>
	public class SearchIndex
	{
		private static readonly Regex _andRegex = new Regex(@"\sAND\s", RegexOptions.IgnoreCase);
		private static readonly Regex _orRegex = new Regex(@"\sOR\s", RegexOptions.IgnoreCase);
		private static readonly Regex _notRegex = new Regex(@"\sNOT\s", RegexOptions.IgnoreCase);

		private static readonly string[] specialLuceneCharacters =
			{
				@"\", "+", "-", "&&", "||", "!", "(", ")", "{", "}", "[",
				"]", "^", "\"", "~", "*", "?", ":"
			};


		private readonly Analyzer _analyzer;

		private ReaderWriterLock lck = new ReaderWriterLock();
		private RAMDirectory rd;

		public SearchIndex()
		{
			_analyzer = new StandardAnalyzer(Version.LUCENE_30);
			// ToDo: SnowballAnalyzer
		}

		protected virtual int ReaderTimeOut
		{
			get { return 15000; }
		}

		protected virtual int WriterTimeOut
		{
			get { return 30000; }
		}


		private static List<Post> ItemsToIndex()
		{
			DataBuddy.Query q = PostCollection.DefaultQuery();
			PostCollection pc = new PostCollection();
			pc.LoadAndCloseReader(q.ExecuteReader());

			return pc;
		}

		private static Document CreateDocument(Post t)
		{
			Document doc = new Document();
			doc.Add(new Field("postid", t.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add(new Field("title", t.Title, Field.Store.YES, Field.Index.ANALYZED));
			doc.Add(new Field("body", RemoveHtml(t.Body, 0), Field.Store.YES, Field.Index.ANALYZED));
			doc.Add(new Field("exact", RemoveHtml(t.Body, 0), Field.Store.NO, Field.Index.ANALYZED, Field.TermVector.YES));
			doc.Add(new Field("rawbody", t.Body, Field.Store.YES, Field.Index.NO));
			doc.Add(new Field("category", t.Category.Name, Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add(new Field("categoryid", t.Category.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add(new Field("username", t.UserName, Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add(new Field("modifiedby", t.ModifiedBy, Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add(new Field("createdby", t.CreatedBy, Field.Store.YES, Field.Index.NOT_ANALYZED));
			foreach (string tag in Util.ConvertStringToList(t.TagList))
			{
				doc.Add(new Field("tag", tag, Field.Store.YES, Field.Index.ANALYZED));
			}
			doc.Add(new Field("author", t.CreatedBy, Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add(new Field("date", DateTools.DateToString(t.Published, DateTools.Resolution.HOUR), Field.Store.YES,
			                  Field.Index.NOT_ANALYZED));
			doc.Add(new Field("createddate", DateTools.DateToString(t.CreatedOn, DateTools.Resolution.HOUR), Field.Store.YES,
			                  Field.Index.NOT_ANALYZED));
			doc.Add(new Field("modifieddate", DateTools.DateToString(t.ModifiedOn, DateTools.Resolution.HOUR), Field.Store.YES,
			                  Field.Index.NOT_ANALYZED));
			doc.Add(new Field("name", t.Name, Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add(new Field("image", t.ImageUrl ?? string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add(new Field("commentcount", t.CommentCount.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add(new Field("pendingcommentcount", t.PendingCommentCount.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add(new Field("propertykeys", t.PropertyKeys, Field.Store.YES, Field.Index.NO));
			doc.Add(new Field("propertyvalues", t.PropertyValues, Field.Store.YES, Field.Index.NO));
			return doc;
		}

		private Post CreatePostFromDocument(Document doc, Highlighter hl)
		{
			var post = new Post
				           {
					           Id = Int32.Parse(doc.GetField("postid").StringValue),
					           UserName = doc.GetField("username").StringValue,
					           CreatedBy = doc.GetField("author").StringValue,
					           //CreatedBy = doc.GetField("createdby").StringValue,
					           ModifiedBy = doc.GetField("modifiedby").StringValue,
					           Title = doc.GetField("title").StringValue,
					           CategoryId = Int32.Parse(doc.GetField("categoryid").StringValue),
					           Published = DateTools.StringToDate(doc.GetField("date").StringValue),
					           CreatedOn = DateTools.StringToDate(doc.GetField("createddate").StringValue),
					           ModifiedOn = DateTools.StringToDate(doc.GetField("modifieddate").StringValue),
					           Name = doc.GetField("name").StringValue,
					           PostBody =
						           (_analyzer == null || hl == null)
							           ? doc.GetField("rawbody").StringValue
							           : BestMatch(doc.GetField("body").StringValue, "body", _analyzer, hl),
					           ExtendedBody = "<!--hidden-->",
					           CommentCount = Int32.Parse(doc.GetField("commentcount").StringValue),
					           PendingCommentCount = Int32.Parse(doc.GetField("pendingcommentcount").StringValue),
					           ImageUrl = doc.GetField("image").StringValue,
					           PropertyKeys = doc.GetField("propertykeys").StringValue,
					           PropertyValues = doc.GetField("propertyvalues").StringValue,
				           };


			var sa = doc.GetValues("tag");
			if (sa != null)
				post.TagList = string.Join(",", sa);

			post.DeserializeCustomFields();

			return post;
		}

		private LQ.QueryParser GetQueryParser()
		{
			var parser = new LQ.MultiFieldQueryParser(Version.LUCENE_30,
			                                          new[] {SearchFields.Body, SearchFields.Title, SearchFields.Tag}, _analyzer);
			parser.DefaultOperator = LQ.QueryParser.Operator.AND;
			return parser;
		}


		protected static string BestMatch(string text, string fieldName, Analyzer analyzer, Highlighter hl)
		{
			try
			{
				//decode text to prevent fragmenting escape chars or char refs
				text = HttpUtility.HtmlDecode(text);
				TokenStream tokenStream = analyzer.TokenStream(fieldName, new StringReader(text));
				//encode for html validity, replace temporary highlight tags
				return
					HttpUtility.HtmlEncode(hl.GetBestFragments(tokenStream, text, 2, "..."))
					           .Replace(":openhighlight", "<span style=\"BACKGROUND-COLOR: Yellow\">")
					           .Replace(":closehighlight", "</span>");
			}
			catch
			{
				return RemoveHtml(text.Replace(":openhighlight", "").Replace(":closehighlight", ""), 250);
			}
		}

		private void EnsureIndexExists()
		{
			if (rd == null)
				BuildIndex();
		}

		private void BuildIndex()
		{
			IndexWriter writer = null;
			lck.AcquireWriterLock(WriterTimeOut);
			try
			{
				if (rd == null)
				{
					rd = new RAMDirectory();
					writer = new IndexWriter(rd, _analyzer, true, IndexWriter.MaxFieldLength.LIMITED);

					foreach (var post in ItemsToIndex())
					{
						writer.AddDocument(CreateDocument(post));
					}
				}
			}
			finally
			{
				if (writer != null)
					writer.Dispose();

				lck.ReleaseWriterLock();
			}
		}

		#region Public Methods

		public int TotalDocuments()
		{
			EnsureIndexExists();
			IndexSearcher searcher = null;

			lck.AcquireReaderLock(ReaderTimeOut);
			try
			{
				var query = new MatchAllDocsQuery();
				searcher = new IndexSearcher(rd);

				TopDocs hits = searcher.Search(query, 1);
				return hits.TotalHits;
			}
			finally
			{
				if (searcher != null)
					searcher.Dispose();

				lck.ReleaseReaderLock();
			}
		}

		/// <summary>
		///     For objects matching your query
		/// </summary>
		public SearchResultSet<Post> Search(SearchQuery searchQuery)
		{
			var searchResults = new SearchResultSet<Post>();

			if (String.IsNullOrWhiteSpace(searchQuery.QueryText))
				return searchResults;

			DateTime dt = DateTime.Now;
			string queryText = searchQuery.QueryText;
			IndexSearcher searcher = null;

			EnsureIndexExists();

			var parser = GetQueryParser();
			foreach (var specialCharacter in specialLuceneCharacters)
			{
				if (queryText.Contains(specialCharacter))
					queryText = queryText.Replace(specialCharacter, @"\" + specialCharacter);
			}
			queryText = _andRegex.Replace(queryText, " AND ");
			queryText = _orRegex.Replace(queryText, " OR ");

			var query = parser.Parse(queryText);
			if (String.IsNullOrWhiteSpace(query.ToString()))
				return searchResults;

			lck.AcquireReaderLock(ReaderTimeOut);
			try
			{
				searcher = new IndexSearcher(rd);
				TopDocs hits = searcher.Search(query, searchQuery.MaxResults);

				searchResults.TotalRecords = hits.TotalHits;
				TimeSpan ts = (DateTime.Now - dt);
				searchResults.SearchDuration = ts.Milliseconds;

				if (hits.ScoreDocs.Length <= 0)
					return searchResults;

				var formatter = new SimpleHTMLFormatter(":openhighlight", ":closehighlight");
				//use placeholders instead of html to allow later htmlencode
				var highlighter = new Highlighter(formatter, new QueryScorer(query));

				int start = (searchQuery.PageIndex - 1)*searchQuery.PageSize;
				int end = searchQuery.PageIndex*searchQuery.PageSize;

				if (start > hits.ScoreDocs.Length)
					start = hits.ScoreDocs.Length;

				if (end > hits.ScoreDocs.Length)
					end = hits.ScoreDocs.Length;

				for (int i = start; i < end; i++)
				{
					var doc = searcher.Doc(hits.ScoreDocs[i].Doc);
					searchResults.Add(CreatePostFromDocument(doc, highlighter));
				}

				return searchResults;
			}
			catch (LQ.ParseException)
			{
				// Used to retry here... but now we escape query up-front
				throw;
			}
			finally
			{
				if (searcher != null)
					searcher.Dispose();

				lck.ReleaseReaderLock();
			}
		}

		private static Query GetIdSearchQuery(int id)
		{
			return new TermQuery(new Term("postid", NumericUtils.IntToPrefixCoded(id)));
		}

		public List<Post> Similar(int postid, int itemsToReturn)
		{
			var list = new List<Post>();

			if (postid <= 0)
				return list;

			IndexSearcher searcher = null;
			IndexReader reader = null;

			EnsureIndexExists();

			var query = GetIdSearchQuery(postid);

			lck.AcquireReaderLock(ReaderTimeOut);
			try
			{
				searcher = new IndexSearcher(rd);

				// Get Original document
				TopDocs hits = searcher.Search(query, itemsToReturn);
				if (hits == null || hits.ScoreDocs.Length <= 0)
					return list;

				int docNum = hits.ScoreDocs[0].Doc;
				if (docNum > -1)
				{
					LQ.QueryParser parser = GetQueryParser();
					reader = IndexReader.Open(rd, true);

					var mlt = new MoreLikeThis(reader);
					mlt.Analyzer = _analyzer;
					mlt.SetFieldNames(new[] {SearchFields.Title, SearchFields.Body, SearchFields.Tag});
					mlt.MinDocFreq = 5;
					mlt.MinTermFreq = 2;
					mlt.Boost = true;
					var moreResultsQuery = mlt.Like(docNum);

					TopDocs similarhits = searcher.Search(moreResultsQuery, itemsToReturn);

					for (int i = 0; i < similarhits.ScoreDocs.Length; i++)
					{
						Document doc = searcher.Doc(similarhits.ScoreDocs[i].Doc);
						var post = CreatePostFromDocument(doc, null);
						if (postid != post.Id)
							list.Add(post);

						if (list.Count >= itemsToReturn)
							break;
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				if (searcher != null)
					searcher.Dispose();

				if (reader != null)
					reader.Dispose();

				lck.ReleaseReaderLock();
			}


			return list;
		}

		/// <summary>
		///     Clears all results from the current index. During the next search the index will be rebuilt.
		/// </summary>
		public void Clear()
		{
			lck.AcquireWriterLock(WriterTimeOut);
			try
			{
				if (rd != null)
				{
					rd.Dispose();
					rd = null;
				}
			}
			finally
			{
				lck.ReleaseWriterLock();
			}
		}

		/// <summary>
		///     Clears the current index and then resuilds it.
		/// </summary>
		public void Reset()
		{
			lck.AcquireWriterLock(WriterTimeOut);
			try
			{
				if (rd != null)
				{
					rd.Dispose();
					rd = null;
				}

				BuildIndex();
			}
			finally
			{
				lck.ReleaseWriterLock();
			}
		}

		#endregion

		#region Helpers

		protected static Regex htmlRegex = new Regex("<[^>]+>|\\&nbsp\\;|\\&lt\\;|\\&rt\\;",
		                                             RegexOptions.IgnoreCase | RegexOptions.Compiled);

		protected static Regex spacer = new Regex(@"\s{2,}");
		private static Regex isWhitespace = new Regex("[^\\w&;#]", RegexOptions.Singleline | RegexOptions.Compiled);

		protected static string RemoveHtml(string html, int charLimit)
		{
			if (html == null || html.Trim().Length == 0)
				return string.Empty;

			html = htmlRegex.Replace(html, string.Empty);
			html = spacer.Replace(html.Trim(), " ");

			if (charLimit <= 0 || charLimit >= html.Length)
				return html;
			else
				return MaxLength(html, charLimit);
		}

		protected static string MaxLength(string text, int charLimit)
		{
			if (string.IsNullOrEmpty(text))
				return string.Empty;

			if (charLimit >= text.Length)
				return text;

			Match match = isWhitespace.Match(text, charLimit);
			if (!match.Success)
				return text;
			else
				return text.Substring(0, match.Index);
		}

		#endregion
	}

	public class SearchQuery
	{
		public SearchQuery()
		{
			Sort = SearchSort.Relevance;
			MaxResults = 999;
		}

		/// <summary>
		///     The number of records to return per page.
		/// </summary>
		public int PageSize { get; set; }

		/// <summary>
		///     Current Page
		/// </summary>
		public int PageIndex { get; set; }

		/// <summary>
		///     What are we searching for
		/// </summary>
		public string QueryText { get; set; }

		/// <summary>
		///     What/how are we sorting
		/// </summary>
		public SearchSort Sort { get; set; }

		public int MaxResults { get; set; }
	}

	public enum SearchSort
	{
		Ascending,
		Descending,
		Index,
		Relevance,
	}

	public class SearchResultSet<T> : List<T> where T : class, new()
	{
		/// <summary>
		///     Current page, this value is returned via the SearchQuery
		/// </summary>
		public int PageSize { get; set; }


		/// <summary>
		///     Current page index, this value is returned via the SearchQuery
		/// </summary>
		public int PageIndex { get; set; }


		/// <summary>
		///     Total number of records matching the SearchQuery
		/// </summary>
		public int TotalRecords { get; set; }


		/// <summary>
		///     Number of milliseconds the search took to complete.
		/// </summary>
		public int SearchDuration { get; set; }
	}
}