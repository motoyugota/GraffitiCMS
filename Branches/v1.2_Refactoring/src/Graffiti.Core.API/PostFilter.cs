using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Graffiti.Core.Services;

namespace Graffiti.Core.API
{
    public enum TrueFalseIgnore
    {
        True,
        False,
        Ignore = 0
    }

    /// <summary>
    /// Helper to managed converting querystring parameters to a Post Query
    /// </summary>
    public class PostFilter
    {
        private ICategoryService _categoryService;
        private IPostService _postService;

        private string _author = string.Empty;
        private int _categoryId = 0;
        private PostStatus _status = PostStatus.NotSet;
        private TrueFalseIgnore _isDeleted = TrueFalseIgnore.False;
        private TrueFalseIgnore _isHome = TrueFalseIgnore.Ignore;
        private int _pageIndex = 1;
        private int _pageSize = 10;
        private bool _includeChildCategories = false;
        private int _revision;
        private int _parentId;

        // private constructor
        // force creations to come from querystring parsing
        private PostFilter()
        {
            _categoryService = ServiceLocator.Get<ICategoryService>();
            _postService = ServiceLocator.Get<IPostService>();
        }


        // parse querystring into filter
        public static PostFilter FromQueryString(NameValueCollection queryString)
        {
            PostFilter postFilter = new PostFilter();

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
            int categoryId;
            string cid = queryString[QueryStringKey.CategoryId];
            if ((!string.IsNullOrEmpty(cid)) && (int.TryParse(cid, out categoryId)))
                postFilter.CategoryId = categoryId;

            if(postFilter.CategoryId > 0)
            {
                postFilter.IncludeChildCategories =
                    Util.AreEqualIgnoreCase(queryString[QueryStringKey.IncludeChildCategories], "true");
            }

            // parse and set status filter
            string s = queryString[QueryStringKey.Status];
            if (!string.IsNullOrEmpty(s))
            {
                PostStatus status = PostStatus.NotSet;
                try { status = (PostStatus)Enum.Parse(typeof(PostStatus), s, true); }
                catch { }
                postFilter.Status = status;
            }

            // parse and set deleted filter
            bool isDeleted = false;
            string d = queryString[QueryStringKey.IsDeleted];
            if ((!string.IsNullOrEmpty(d)) && (bool.TryParse(d, out isDeleted)))
                postFilter.IsDeleted = isDeleted ? TrueFalseIgnore.True : TrueFalseIgnore.False;

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

            int parnetId;
            string parId = queryString[QueryStringKey.ParentId];
            if ((!string.IsNullOrEmpty(parId)) && (int.TryParse(parId, out parnetId)))
                postFilter.ParentId = parnetId;

            return postFilter;
        }


        public IList<Post> ToQuery()
        {
            return ToQuery(true);
        }

        // generate query object from filter
        public IList<Post> ToQuery(bool paged)
        {
            IList <Post> posts = _postService.FetchPosts();

            if (Id <= 0)
            {
                if (!string.IsNullOrEmpty(Author))
                    posts = posts.Where(x => x.UserName == Author).ToList();

                if (CategoryId > 0)
                {
                    Category category = _categoryService.FetchCachedCategory(CategoryId, false);

                    if(IncludeChildCategories && category.HasChildren)
                    {
                        List<int> ids = new List<int>(category.Children.Count + 1);
                        foreach (Category child in category.Children)
                            ids.Add(child.Id);

                        ids.Add(category.Id);

                        posts = posts.Where(x => ids.Contains(x.CategoryId)).ToList();
                    }
                    else
                        posts = posts.Where(x => x.CategoryId == CategoryId).ToList();
                }
                if (Status == PostStatus.NotSet)
                    posts = posts.Where(x => x.Status == (int)PostStatus.Publish).ToList();
                else
                    posts = posts.Where(x => x.Status == (int)Status).ToList();

                if (IsDeleted != TrueFalseIgnore.Ignore)
                    posts = posts.Where(x => x.IsDeleted == (IsDeleted == TrueFalseIgnore.True ? true : false)).ToList();

                if (IsHome != TrueFalseIgnore.Ignore)
                    posts = posts.Where(x => x.IsHome == (IsHome == TrueFalseIgnore.True ? true : false)).ToList();

                if(ParentId > 0)
                    posts = posts.Where(x => x.ParentId == ParentId).ToList();

                posts = posts.OrderByDescending(x => x.Published).ToList();

                if (PageIndex > 0 && PageSize > 0 && posts.Count > PageSize && paged)
                    posts = posts.Skip(PageSize * (PageIndex - 1)).Take(PageSize).ToList();
            }
            else
            {
                posts = posts.Where(x => x.Id == Id).ToList();
            }

            return posts;
        }

        #region Properties...
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public int CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        public PostStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public TrueFalseIgnore IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }

        public TrueFalseIgnore IsHome
        {
            get { return _isHome; }
            set { _isHome = value; }
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

        public int ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
	
        public bool IncludeChildCategories
        {
            get { return _includeChildCategories; }
            set { _includeChildCategories = value; }
        }

        public int Revision
        {
            get { return _revision; }
            set { _revision = value; }
        }

        #endregion
    }
}