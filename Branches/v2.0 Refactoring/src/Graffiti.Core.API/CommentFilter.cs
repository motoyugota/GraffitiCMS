using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Graffiti.Core.Services;

namespace Graffiti.Core.API
{
    /// <summary>
    /// Helper to managed converting querystring parameters to a Comment Query
    /// </summary>
    public class CommentFilter
    {

        private ICommentService _commentService;

        private string _author = string.Empty;
        private int _categoryId = 0;
        private bool _isDeleted = false;
        private TrueFalseIgnore _isPublished = TrueFalseIgnore.Ignore;
        private int _pageIndex = 1;
        private int _pageSize = 10;
        private string _ipAddress;
        private string _name;
        private int _spamScore;

        
        private CommentFilter()
        {
            _commentService = ServiceLocator.Get<ICommentService>();
        }


        // parse querystring into filter
        public static CommentFilter FromQueryString(NameValueCollection queryString)
        {
            CommentFilter postFilter = new CommentFilter();

            string id = queryString[QueryStringKey.Id];
            if(!string.IsNullOrEmpty(id))
            {
                postFilter.Id = Int32.Parse(id);
                return postFilter;
            }

            // parse and set author filter
            string author = queryString[QueryStringKey.Author];
            if (!string.IsNullOrEmpty(author))
                postFilter.Author = author;

            // parse and set category filter
            int postId;
            string cid = queryString[QueryStringKey.PostId];
            if ((!string.IsNullOrEmpty(cid)) && (int.TryParse(cid, out postId)))
                postFilter.PostId = postId;


            string sc = queryString[QueryStringKey.Spam];
            if (sc != null)
                postFilter.SpamScore = Int32.Parse(sc);

            postFilter.IPAddress = queryString[QueryStringKey.IPAddress];
            postFilter.Name = queryString[QueryStringKey.Name];

          

            // parse and set deleted filter
            bool isDeleted = false;
            string d = queryString[QueryStringKey.IsDeleted];
            if ((!string.IsNullOrEmpty(d)) && (bool.TryParse(d, out isDeleted)))
                postFilter.IsDeleted = isDeleted;

            bool isPublished = false;
            string ip = queryString[QueryStringKey.IsPublished];
            if ((!string.IsNullOrEmpty(ip)) && (bool.TryParse(ip, out isPublished)))
                postFilter.IsPublished = isPublished ? TrueFalseIgnore.True : TrueFalseIgnore.False;

            // parse and set page index filter
            int pageIndex;
            string pi = queryString[QueryStringKey.PageIndex];
            if ((!string.IsNullOrEmpty(pi)) && (int.TryParse(pi, out pageIndex)))
                postFilter.PageIndex = pageIndex;

            // parse and set page size filter
            int pageSize;
            string ps = queryString[QueryStringKey.PageSize];
            if ((!string.IsNullOrEmpty(ps)) && (int.TryParse(ps, out pageSize)))
                postFilter.PageSize = pageSize;

            return postFilter;
        }


        public IList<Comment> ToQuery()
        {
            return ToQuery(true);
        }

        // generate query object from filter
        public IList<Comment> ToQuery(bool paged)
        {
            IList<Comment> comments = _commentService.FetchComments();

            if (Id <= 0)
            {
                if (!string.IsNullOrEmpty(Author))
                    comments = comments.Where(x => x.UserName == Author).ToList();

                if (PostId > 0)
                {
                    comments = comments.Where(x => x.PostId == PostId).ToList();
                }

                if (SpamScore > 0)
                    comments = comments.Where(x => x.SpamScore >= SpamScore).ToList();

                if (!string.IsNullOrEmpty(IPAddress))
                    comments = comments.Where(x => x.IPAddress == IPAddress).ToList();

                if (!string.IsNullOrEmpty(Name))
                    comments = comments.Where(x => x.Name == Name).ToList();

                comments = comments.Where(x => x.IsDeleted == IsDeleted).ToList();

                if (IsPublished != TrueFalseIgnore.Ignore)
                    comments = comments.Where(x => x.IsPublished == (IsPublished == TrueFalseIgnore.True ? true : false)).ToList();

                comments = comments.OrderByDescending(x => x.Published).ToList();

                if (PageIndex > 0 && PageSize > 0 && comments.Count > PageSize && paged)
                    comments = comments.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
            else
            {
                comments = comments.Where(x => x.Id == Id).ToList();
            }

            return comments;
        }

        #region Properties...
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string IPAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        public int PostId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        public int SpamScore
        {
            get { return _spamScore; }
            set { _spamScore = value; }
        }

        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }

        public TrueFalseIgnore IsPublished
        {
            get { return _isPublished; }
            set { _isPublished = value; }
        }

        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _startDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        private DateTime _endDate;

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }
	

        #endregion
    }
}