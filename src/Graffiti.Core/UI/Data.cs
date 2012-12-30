using System;
using System.Collections.Generic;
using System.Linq;
using Graffiti.Core.Services;

namespace Graffiti.Core
{
	/// <summary>
	/// Data is a built-in Chalk extension, similar to Macros, which enables quick and easy access to additional Graffiti data. ($data)
	/// </summary>
	[Chalk("data")]
	public class Data
	{
        private IPostService _postService;
	    private ITagService _tagService;
	    private ICategoryService _categoryService;
	    private ICommentService _commentService;

        public Data(IPostService postService, ICategoryService categoryService, ICommentService commentService, ITagService tagService) 
        {
            _postService = postService;
            _categoryService = categoryService;
            _commentService = commentService;
            _tagService = tagService;
        }

        public Data()
        {
            _postService = ServiceLocator.Get<IPostService>();
            _categoryService = ServiceLocator.Get<ICategoryService>();
            _commentService = ServiceLocator.Get<ICommentService>();
            _tagService = ServiceLocator.Get<ITagService>();           
        }

		/// <summary>
		/// Gets the Post for the specified ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Post GetPost(int id)
		{
			return _postService.FetchCachedPost(id);
		}

		/// <summary>
		/// Gets the Post for the specified name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Post GetPost(string name)
		{
			if (string.IsNullOrEmpty(name))
				return null;
			if (name.StartsWith("/"))
				name = name.Substring(1);
			if (name.EndsWith("/"))
				name = name.Substring(0, name.Length - 1);
			if (string.IsNullOrEmpty(name))
				return null;

			int postId = _postService.GetPostIdByName(name);

			if (postId == -1)
				return null;
			return _postService.FetchCachedPost(postId);
		}

		/// <summary>
		/// Gets the sites featured post
		/// </summary>
		/// <returns></returns>
		public Post Featured()
		{
			SiteSettings settings = SiteSettings.Get();
			if (settings.FeaturedId > 0)
			{
				Post p = _postService.FetchCachedPost(settings.FeaturedId);

				if (p.IsPublished)
					return p;
			}

			return null;
		}

		/// <summary>
		/// Gets the sites featured post for the specified category Id
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		public Post Featured(int categoryId)
		{
			return Featured(_categoryService.FetchCachedCategory(categoryId, true));
		}

		/// <summary>
		/// Gets the sites featured post for the specified category name
		/// </summary>
		/// <param name="categoryName"></param>
		/// <returns></returns>
		public Post Featured(string categoryName)
		{
			return Featured(GetCategory(categoryName));
		}

		/// <summary>
		/// Gets the sites featured post for the specifed Category
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		private Post Featured(Category category)
		{
			if (category != null)
			{
				if (category.FeaturedId > 0)
				{
					Post p = _postService.FetchCachedPost(category.FeaturedId);

					if (p.IsPublished)
						return p;
				}
			}

			return null;
		}

		public PostCollection PostsByUser(string username, int numberOfPosts, SortOrderType sortOrder)
		{
			IGraffitiUser user = GraffitiUsers.GetUser(username);
			if (user == null)
				return null;

			const string CacheKey = "Posts-Users-P:{0}-U:{1}-T:{2}-PS:{3}";

			PostCollection pc = ZCache.Get<PostCollection>(string.Format(CacheKey, 1, user.UniqueId, sortOrder, numberOfPosts));
			if (pc == null)
			{
                pc = new PostCollection(_postService.FetchPosts(numberOfPosts, sortOrder).Where(x => x.UserName == user.Name).ToList());
				ZCache.InsertCache(string.Format(CacheKey, 1, user.UniqueId, sortOrder, numberOfPosts), pc, 60);
			}

			return pc;
		}

		public PostCollection PostsByUserAndCategory(string username, string categoryName, int numberOfPosts)
		{
			Category category = GetCategory(categoryName);
			IGraffitiUser user = GraffitiUsers.GetUser(username);

			if (category == null || user == null)
				return null;

			const string CacheKey = "Posts-Users-Categories-P:{0}-U:{1}-C:{2}-T:{3}-PS:{4}";

			PostCollection pc = ZCache.Get<PostCollection>(string.Format(CacheKey, 1, user.UniqueId, category.Id, category.SortOrder, numberOfPosts));
			if (pc == null)
			{
			    IList<Post> posts =
                    _postService.FetchPosts().Where(x => x.UserName == user.Name).ToList();

				if (Category.IncludeChildPosts)
				{
					if (category.ParentId > 0)
                        posts = posts.Where(x => x.CategoryId == category.Id).ToList();
					else
					{
						List<int> ids = new List<int>(category.Children.Count + 1);
						foreach (Category child in category.Children)
							ids.Add(child.Id);

						ids.Add(category.Id);

                        posts = posts.Where(x => ids.Contains(x.CategoryId)).ToList();
					}
				}
				else
				{
                    posts = posts.Where(x => x.CategoryId == category.Id).ToList();
				}

                posts = posts.OrderByDescending(x => x.Published).Take(numberOfPosts).ToList();
                pc = new PostCollection(posts);

				ZCache.InsertCache(string.Format(CacheKey, 1, user.UniqueId, category.Id, category.SortOrder, numberOfPosts), pc, 60);
			}

			return pc;
		}

		/// <summary>
		/// Gets all Posts by the specified tag
		/// </summary>
		/// <param name="tagName"></param>
		/// <returns></returns>
		public PostCollection PostsByTag(string tagName)
		{
			string TagName = Util.CleanForUrl(tagName);
			PostCollection pc = ZCache.Get<PostCollection>("Tags-" + TagName);
			if (pc == null)
			{
				pc = new PostCollection(_postService.FetchPostsByTag(TagName));
				ZCache.InsertCache("Tags-" + TagName, pc, 60);
			}

			return pc;
		}

		/// <summary>
		/// Gets all posts by the specified tag in the specified category name
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="category"></param>
		/// <returns></returns>
		public PostCollection PostsByTagAndCategory(string tagName, string category)
		{
			Category cat = GetCategory(category);
			return PostsByTagAndCategory(tagName, cat, Int32.MaxValue);
		}

		/// <summary>
		/// Gets all posts by the specified tag with the specified category name with a max number of results
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="category"></param>
		/// <param name="numberOfPosts"></param>
		/// <returns></returns>
		public PostCollection PostsByTagAndCategory(string tagName, string category, int numberOfPosts)
		{
			Category cat = GetCategory(category);
			return PostsByTagAndCategory(tagName, cat, numberOfPosts);
		}

		/// <summary>
		/// Gets all posts by the specified tag in the specified Category
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="category"></param>
		/// <returns></returns>
		public PostCollection PostsByTagAndCategory(string tagName, Category category)
		{
			return PostsByTagAndCategory(tagName, category, Int32.MaxValue);
		}

		/// <summary>
		/// Gets all posts by the specified tag in the specified category with a max number of results
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="category"></param>
		/// <param name="numberOfPosts"></param>
		/// <returns></returns>
		public PostCollection PostsByTagAndCategory(string tagName, Category category, int numberOfPosts)
		{
			if (category == null)
				return null;

			if (String.IsNullOrEmpty(tagName))
				return PostsByCategory(category, numberOfPosts);
			else
			{
				PostCollection pc = new PostCollection();

				int tempCount = 0;
				foreach (Post p in _postService.FetchPostsByTagAndCategory(tagName, category.Id))
				{
					tempCount++;
					if (tempCount > numberOfPosts)
						break;

					pc.Add(p);
				}

				return pc;
			}
		}

		/// <summary>
		/// Gets the last x amount of posts from the specified category name
		/// </summary>
		/// <param name="categoryName"></param>
		/// <param name="numberOfPosts"></param>
		/// <returns></returns>
		public PostCollection PostsByCategory(string categoryName, int numberOfPosts)
		{
			Category category = GetCategory(categoryName);
			return category == null ? null : PostsByCategory(category, numberOfPosts);
		}

		/// <summary>
		/// Gets the last x amount of posts from the specified Category
		/// </summary>
		/// <param name="category"></param>
		/// <param name="numberOfPosts"></param>
		/// <returns></returns>
		public PostCollection PostsByCategory(Category category, int numberOfPosts)
		{
			return PostsByCategory(category, numberOfPosts, false);
		}

		public PostCollection PostsByCategory(Category category, int numberOfPosts, bool filterHome)
		{
			if (category == null)
				return null;

			const string CacheKey = "Posts-Categories-P:{0}-C:{1}-T:{2}-PS:{3}";

			PostCollection pc = ZCache.Get<PostCollection>(string.Format(CacheKey, 1, category.Id, category.SortOrder, numberOfPosts));
			if (pc == null)
			{
                IList<Post> posts = _postService.FetchPosts();

				if (Category.IncludeChildPosts)
				{
					if (category.ParentId > 0)
                        posts = posts.Where(x => x.CategoryId == category.Id).ToList();
					else
					{
						List<int> ids = new List<int>(category.Children.Count + 1);
						foreach (Category child in category.Children)
							ids.Add(child.Id);

						ids.Add(category.Id);

                        posts = posts.Where(x => ids.Contains(category.Id)).ToList();
					}
				}
				else
				{
                    posts = posts.Where(x => x.CategoryId == category.Id).ToList();
				}

				if (filterHome)
				{
					string where = GraffitiContext.Current["where"] as string;
					if (!string.IsNullOrEmpty(where) && where == "home" && Site.UseCustomHomeList)
                        posts = posts.Where(x => x.IsHome).ToList();
				}

                posts = posts.OrderByDescending(x => x.Published).Take(numberOfPosts).ToList();

                pc = new PostCollection(posts);

				ZCache.InsertCache(string.Format(CacheKey, 1, category.Id, category.SortOrder, numberOfPosts), pc, 60);
			}

			return pc;
		}

        //public IEnumerable Query(IDictionary paramaters)
        //{
        //    string type = paramaters["type"] as string;
        //    if (type != null)
        //        paramaters.Remove("type");
        //    else
        //        type = "post";

        //    switch (type)
        //    {
        //        case "post":

        //            Query postQuery = Post.CreateQuery();
        //            SetLimits(postQuery, paramaters);

        //            string categoryName = paramaters["category"] as string;
        //            if (categoryName != null)
        //                paramaters.Remove("category");

        //            if (categoryName == "none")
        //                categoryName = _categoryService.UncategorizedName();

        //            if (categoryName != null)
        //                paramaters["categoryid"] = _categoryService.GetCachedCategory(categoryName, false).Id;

        //            if (paramaters["isDeleted"] == null)
        //                paramaters["isDeleted"] = false;

        //            if (paramaters["isPublished"] == null)
        //            {
        //                paramaters["isPublished"] = true;
        //            }

        //            string orderBy = paramaters["orderby"] as string;
        //            if (orderBy != null)
        //                paramaters.Remove("orderBy");
        //            else
        //                orderBy = "Published DESC";

        //            postQuery.Orders.Add(orderBy);

        //            string cacheKey = "Posts-";
        //            foreach (string key in paramaters.Keys)
        //            {
        //                Column col = GetPostColumn(key);
        //                postQuery.AndWhere(col, paramaters[key]);
        //                cacheKey += "|" + col.Name + "|" + paramaters[key];
        //            }

        //            PostCollection pc = ZCache.Get<PostCollection>(cacheKey);
        //            if (pc == null)
        //            {
        //                pc = new PostCollection();
        //                pc.LoadAndCloseReader(postQuery.ExecuteReader());
        //                ZCache.InsertCache(cacheKey, pc, 90);
        //            }
        //            return pc;

        //        case "comment":

        //            break;

        //        case "category":

        //            break;
        //    }

        //    return null;
        //}

        //private static Column GetPostColumn(string key)
        //{
        //    foreach (Column c in Post.Table.Columns)
        //    {
        //        if (Util.AreEqualIgnoreCase(c.Name, key))
        //            return c;
        //    }

        //    throw new Exception("Post Column " + key + " was not found");
        //}

        //private static void SetLimits(Query q, IDictionary paramaters)
        //{
        //    if (paramaters["top"] != null)
        //    {
        //        q.Top = paramaters["top"] as string;
        //        paramaters.Remove("top");
        //    }
        //    else if (paramaters["pageIndex"] != null)
        //    {
        //        q.PageIndex = Convert.ToInt32(paramaters["pageIndex"]);
        //        paramaters.Remove("pageIndex");

        //        if (paramaters["pageSize"] != null)
        //        {
        //            q.PageSize = Convert.ToInt32(paramaters["pageSize"]);
        //            paramaters.Remove("pageSize");

        //        }
        //        else
        //            q.PageSize = 10;
        //    }
        //}

		/// <summary>
		/// Gets all of the sites Categories
		/// </summary>
		/// <returns></returns>
		public CategoryCollection Categories()
		{
			return new CategoryCollection(_categoryService.FetchTopLevelCachedCategories());
		}

		/// <summary>
		/// Gets a Category by the specified Id
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		public Category GetCategory(int categoryId)
		{
			return _categoryService.FetchCachedCategory(categoryId, true);
		}

		/// <summary>
		/// Gets a Category by the specified name
		/// </summary>
		/// <param name="categoryName"></param>
		/// <returns></returns>
		public Category GetCategory(string categoryName)
		{
			if (string.IsNullOrEmpty(categoryName))
				return null;

			if (categoryName.StartsWith("/"))
				categoryName = categoryName.Substring(1);


			if (categoryName.EndsWith("/"))
				categoryName = categoryName.Substring(0, categoryName.Length - 1);

			if (string.IsNullOrEmpty(categoryName))
				return null;

			string[] parts = categoryName.Split(new char[] { '/' });
			Category category = null;
			Category parent_Category = _categoryService.FetchCachedCategory(parts[0], true);

			if (parent_Category == null)
				return null;


			if (parts.Length > 1)
			{
				if (parent_Category.HasChildren)
				{
					foreach (Category child_Category in parent_Category.Children)
					{
						if (Util.AreEqualIgnoreCase(child_Category.Name, parts[1]))
						{
							category = child_Category;
							break;
						}
					}
				}
			}
			else
				category = parent_Category;

			return category;
		}

		/// <summary>
		/// Gets the last x amount specified of recent comments
		/// </summary>
		/// <param name="numberOfComments"></param>
		/// <returns></returns>
		public CommentCollection RecentComments(int numberOfComments)
		{
			return RecentComments(numberOfComments, -1);
		}

		/// <summary>
		/// Gets the last x amount of comments from the specified category name
		/// </summary>
		/// <param name="numberOfComments"></param>
		/// <param name="categoryName"></param>
		/// <returns></returns>
		public CommentCollection RecentComments(int numberOfComments, string categoryName)
		{
			Category category = GetCategory(categoryName);
			return category == null ? null : RecentComments(numberOfComments, category.Id);
		}

		/// <summary>
		/// Gets the last x amount of comments from the specified category Id
		/// </summary>
		/// <param name="numberOfComments"></param>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		public CommentCollection RecentComments(int numberOfComments, int categoryId)
		{
			CommentCollection cc = ZCache.Get<CommentCollection>("Comments-Recent-" + numberOfComments + "c:" + categoryId);
			if (cc == null)
			{
			    IList<Comment> comments = _commentService.FetchComments().Where(x => x.IsPublished && !x.IsDeleted).ToList();
				if (categoryId > 0)
				{
					Category category = _categoryService.FetchCachedCategory(categoryId, true);
					if (category != null)
					{
						if (category.ParentId > 0)
							comments = comments.Where(x => x.Post.CategoryId == categoryId).ToList();
						else
						{
							List<int> ids = new List<int>(category.Children.Count + 1);
							foreach (Category child in category.Children)
								ids.Add(child.Id);

							ids.Add(category.Id);

                            comments = comments.Where(x => ids.Contains(x.Post.CategoryId)).ToList();
						}
					}
					else
					{
						//this should result in no data, but it will signal to 
						//the end user to edit/remove this widget
                        comments = comments.Where(x => x.Post.CategoryId == categoryId).ToList();
					}
				}

			    comments = comments.OrderByDescending(x => x.Published).Take(numberOfComments).ToList();
			    cc = new CommentCollection(comments);

				ZCache.InsertCache("Comments-Recent-" + numberOfComments + "c:" + categoryId, cc, 60);
			}

			return cc;
		}

		/// <summary>
		/// Gets the post feedback from the specified postId
		/// </summary>
		/// <param name="PostId"></param>
		/// <returns></returns>
		public CommentCollection PostFeedback(int PostId)
		{
			CommentCollection cc = ZCache.Get<CommentCollection>("Feedback-" + PostId);
			if (cc == null)
			{
                IList<Comment> comments = _commentService.FetchComments()
                    .Where(x => x.IsPublished && !x.IsDeleted)
                    .Where(x => x.PostId == PostId)
                    .OrderBy(x => x.Published).ToList();
			    cc = new CommentCollection(comments.ToList());
				ZCache.InsertCache("Feedback-" + PostId, cc, 10);
			}

			return cc;
		}

		/// <summary>
		/// Gets the post comments from the specified postId
		/// </summary>
		/// <param name="PostId"></param>
		/// <returns></returns>
		public CommentCollection PostComments(int PostId)
		{
			CommentCollection cc = ZCache.Get<CommentCollection>("Comments-" + PostId);
			if (cc == null)
			{				
                IList<Comment> comments = _commentService.FetchComments()
                    .Where(x => x.IsPublished && !x.IsDeleted)
                    .Where(x => x.PostId == PostId)
                    .Where(x => !x.IsTrackback)
                    .OrderBy(x => x.Published).ToList();
                cc = new CommentCollection(comments.ToList());
				ZCache.InsertCache("Comments-" + PostId, cc, 10);
			}

			return cc;
		}

		/// <summary>
		/// Gets the post trackbacks from the specified postId
		/// </summary>
		/// <param name="PostId"></param>
		/// <returns></returns>
		public CommentCollection PostTrackbacks(int PostId)
		{
			CommentCollection cc = ZCache.Get<CommentCollection>("Trackbacks-" + PostId);
			if (cc == null)
			{
                IList<Comment> comments = _commentService.FetchComments()
                    .Where(x => x.IsPublished && !x.IsDeleted)
                    .Where(x => x.PostId == PostId)
                    .Where(x => x.IsTrackback)
                    .OrderBy(x => x.Published).ToList();
                cc = new CommentCollection(comments.ToList());
				ZCache.InsertCache("Trackbacks-" + PostId, cc, 60);
			}

			return cc;
		}

		/// <summary>
		/// Gets x amount of recent posts
		/// </summary>
		/// <param name="numberOfPosts"></param>
		/// <returns></returns>
		public PostCollection RecentPosts(int numberOfPosts)
        {
            return RecentPosts(numberOfPosts, -1);
        }

        /// <summary>
        /// Gets x amount of recent posts from the specified category Id
        /// </summary>
        /// <param name="numberOfPosts"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public PostCollection RecentPosts(int numberOfPosts, int categoryId)
        {
            PostCollection pc = ZCache.Get<PostCollection>("Posts-Recent-" + numberOfPosts + "c:" + categoryId);
            if (pc == null)
            {
                IList<Post> posts = _postService.FetchPosts();

                if (categoryId > 0)
                {
                    Category category = _categoryService.FetchCachedCategory(categoryId, true);
                    if (category != null)
                    {
                        if (category.ParentId > 0)
                            posts = posts.Where(x => x.CategoryId == category.Id).ToList();
                        else
                        {
                            List<int> ids = new List<int>(category.Children.Count + 1);
                            foreach (Category child in category.Children)
                                ids.Add(child.Id);

                            ids.Add(category.Id);

                            posts = posts.Where(x => ids.Contains(category.Id)).ToList();
                        }
                    }
                    else
                    {
                        //this should result in no data, but it will signal to 
                        //the end user to edit/remove this widget
                        posts = posts.Where(x => x.CategoryId == categoryId).ToList();
                    }
                }

                pc = new PostCollection(posts.OrderByDescending(x => x.Views).Take(numberOfPosts).ToList());
                ZCache.InsertCache("Posts-Popular-" + numberOfPosts + "c:" + categoryId, pc, 60);
            }
            return pc;
		}

		/// <summary>
		/// Gets x amount of popular post
		/// </summary>
		/// <param name="numberOfPosts"></param>
		/// <returns></returns>
		public PostCollection PopularPosts(int numberOfPosts)
		{
			return PopularPosts(numberOfPosts, -1);
		}

		/// <summary>
		/// Gets x amount of popular post by the specified category Name
		/// </summary>
		/// <param name="numberOfPosts"></param>
		/// <param name="categoryName"></param>
		/// <returns></returns>
		public PostCollection PopularPosts(int numberOfPosts, string categoryName)
		{
			Category category = GetCategory(categoryName);
			return category == null ? null : PopularPosts(numberOfPosts, category.Id);
		}

		/// <summary>
		/// Gets x amount of popular posts from the specified category Id
		/// </summary>
		/// <param name="numberOfPosts"></param>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		public PostCollection PopularPosts(int numberOfPosts, int categoryId)
		{
			PostCollection pc = ZCache.Get<PostCollection>("Posts-Popular-" + numberOfPosts + "c:" + categoryId);
			if (pc == null)
			{
			    IList<Post> posts = _postService.FetchPosts();

				if (categoryId > 0)
				{
					Category category = _categoryService.FetchCachedCategory(categoryId, true);
					if (category != null)
					{
						if (category.ParentId > 0)
                            posts = posts.Where(x => x.CategoryId == category.Id).ToList();
						else
						{
							List<int> ids = new List<int>(category.Children.Count + 1);
							foreach (Category child in category.Children)
								ids.Add(child.Id);

							ids.Add(category.Id);

                            posts = posts.Where(x => ids.Contains(category.Id)).ToList();
						}
					}
					else
					{
						//this should result in no data, but it will signal to 
						//the end user to edit/remove this widget
                        posts = posts.Where(x => x.CategoryId == categoryId).ToList();
					}
				}

                pc = new PostCollection(posts.OrderByDescending(x => x.Views).Take(numberOfPosts).ToList());
				ZCache.InsertCache("Posts-Popular-" + numberOfPosts + "c:" + categoryId, pc, 60);
			}
			return pc;
		}

		/// <summary>
		/// Gets the specified min/max amount of Tags
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public TagWeightCollection Tags(int min, int max)
		{
			return TagWeightCloud.FetchTags(min, max);
		}

		/// <summary>
		/// Gets all tags
		/// </summary>
		/// <returns></returns>
		public TagCollection GetAllTags()
		{
			TagCollection tc = ZCache.Get<TagCollection>("AllTags");

			if (tc == null)
			{
				tc = new TagCollection();

				foreach (Tag t in _tagService.FetchAllTags())
				{
					Tag tag = tc.Find(
					  delegate(Tag tempTag)
					  {
						  return tempTag.Name == t.Name;
					  }
                    );

					if (tag == null)
						tc.Add(t);
				}

				ZCache.InsertCache("AllTags", tc, 90);
			}

			return tc;
		}

		/// <summary>
		/// Gets a reference to the SiteSettings object
		/// </summary>
		public SiteSettings Site
		{
			get { return SiteSettings.Get(); }
		}

		/// <summary>
		/// Gets a searchresult set by the specified query, pageIndex (usually 0) and max pageSize
		/// </summary>
		/// <param name="q"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public SearchResultSet<Post> Search(string q, int pageIndex, int pageSize)
		{
			SearchQuery sq = new SearchQuery();
			sq.QueryText = q;
			sq.PageIndex = pageIndex;
			sq.PageSize = pageSize;

			return new SearchIndex().Search(sq);
		}

		/// <summary>
		/// Gets x amount of similar posts by the postid
		/// </summary>
		/// <param name="postid"></param>
		/// <param name="numberOfRelatedPosts"></param>
		/// <returns></returns>
		public List<Post> SimilarSearch(int postid, int numberOfRelatedPosts)
		{
			return new SearchIndex().Similar(postid, numberOfRelatedPosts);
		}

		/// <summary>
		/// Gets all widgets in the specified WidgetLocation
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		private static List<Widget> GetWidgets(WidgetLocation location)
		{
			List<Widget> _widgets = Widgets.FetchByLocation(location);
			List<Widget> widgets = new List<Widget>();
			foreach (Widget w in _widgets)
				if (w.IsUserValid())
					widgets.Add(w);

			return widgets;
		}

		/// <summary>
		/// Returns all widgets on the Right sidebar
		/// </summary>
		/// <returns></returns>
		public List<Widget> RightWidgets()
		{
			return GetWidgets(WidgetLocation.Right);
		}

		/// <summary>
		/// Returns all widgets on the Left sidebar
		/// </summary>
		/// <returns></returns>
		public List<Widget> LeftWidgets()
		{
			return GetWidgets(WidgetLocation.Left);
		}

		/// <summary>
		/// Returns all of the queue widgets
		/// </summary>
		/// <returns></returns>
		public List<Widget> QueueWidgets()
		{
			return GetWidgets(WidgetLocation.Queue);
		}
	}
}