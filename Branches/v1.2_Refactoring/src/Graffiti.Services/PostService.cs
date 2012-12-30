using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Graffiti.Core;
using Graffiti.Core.Services;
using Graffiti.Data;

namespace Graffiti.Services 
{
    public class PostService : IPostService
    {
        private IPostRepository _postRepository;
        private ICategoryService _categoryService;

        public PostService(IPostRepository postRepository, ICategoryService categoryService)
        {
            _postRepository = postRepository;
            _categoryService = categoryService;
        }

        public Post FetchPost(int id)
        {
            return _postRepository.FetchPost(id);
        }

        public Post FetchPost(string id) {
            return _postRepository.FetchPost(id);
        }

        public Post FetchPostByUniqueId(Guid uniqueId)
        {
            return _postRepository.FetchPostByUniqueId(uniqueId);
        }

        public Post FetchCachedPost(int id) 
        {
            Post post = ZCache.Get<Post>("Post-" + id);
            if (post == null) {
                post = _postRepository.FetchPost(id);
                ZCache.InsertCache("Post-" + id, post, 10);
            }
            return post;
        }

        public IList<Post> FetchPostsByTag(string tag) 
        {
            return _postRepository.FetchPostsByTag(tag).ToList();
        }

        public IList<Post> FetchPostsByTagAndCategory(string tagName, int categoryId)
        {
            return _postRepository.FetchPostsByTagAndCategory(tagName, categoryId).ToList();
        }

        public IList<Post> FetchPostsByCategory(int categoryId)
        {
            return _postRepository.FetchPostsByCategory(categoryId).ToList();
        }

        public IList<Post> FetchPostsByUsername(string username)
        {
            return _postRepository.FetchPostsByUsername(username).ToList();
        }

        public IList<Post> FetchRecentPosts(int numberOfPosts)
        {
            return _postRepository.FetchRecentPosts(numberOfPosts).ToList();
        }

        public IList<Post> FetchPosts()
        {
            return FetchPosts(int.MaxValue, SortOrderType.Descending).ToList();            
        }

        public IList<Post> FetchPosts(int numberOfPosts, SortOrderType sortOrder)
        {
            return _postRepository.FetchPosts(numberOfPosts, sortOrder).ToList();
        }

        public int GetPostIdByName(string name)
        {
            return _postRepository.GetPostIdByName(name);
        }

        public void UpdateViewCount(int postId)
        {
            _postRepository.UpdateViewCount(postId);
        }

        public void UpdatePostStatus(int postId, PostStatus postStatus)
        {
            _postRepository.UpdatePostStatus(postId, postStatus);
        }

        public Post SavePost(Post post)
        {
            return _postRepository.SavePost(post);
        }

        public Post SavePost(Post post, string username)
        {
            return _postRepository.SavePost(post, username);
        }

        public Post SavePost(Post post, string username, DateTime updateDate)
        {
            return _postRepository.SavePost(post, username, updateDate);
        }

        public void DeletePost(object postId)
        {
            _postRepository.DeletePost(postId);
        }

        public void DestroyDeletedPost(Post post)
        {
            DeletePostDirectory(post);
            _postRepository.DestroyDeletedPost(post.Id);
        }

        public void DestroyDeletedPostCascadingForCategory(int categoryId)
        {
            List<Post> posts = FetchPostsByCategory(categoryId).Where(x => x.IsDeleted).ToList();

            if (posts.Count > 0) {
                foreach (Post p in posts) {
                    DestroyDeletedPost(p);
                }
            }
        }

        public void DestroyDeletedPosts()
        {
            _postRepository.DestroyDeletedPosts();
        }

        public List<PostCount> GetPostCounts(IGraffitiUser user, int categoryId, string username, Delegate categoryPermissionCheckCallback)
        {
            return _postRepository.GetPostCounts(user, categoryId, username, categoryPermissionCheckCallback);
        }

        public List<CategoryCount> GetCategoryCountForStatus(IGraffitiUser user, PostStatus postStatus, string author, Delegate categoryPermissionCheckCallback)
        {
            return _postRepository.GetCategoryCountForStatus(user, postStatus, author, categoryPermissionCheckCallback);
        }

        public List<AuthorCount> GetAuthorCountForStatus(IGraffitiUser user, PostStatus status, string categoryId, Delegate categoryPermissionCheckCallback)
        {
            return _postRepository.GetAuthorCountForStatus(user, status, categoryId, categoryPermissionCheckCallback);
        }

        public IList<Post> DefaultQuery()
        {
            return DefaultQuery(SortOrderType.Descending).ToList();
        }

        public IList<Post> DefaultQuery(SortOrderType sot) 
        {
            IList<Post> posts = FetchPosts()
                .Where(x => x.IsPublished && !x.IsDeleted)
                .Where(x => x.Published <= SiteSettings.CurrentUserTime).ToList();

            if (SiteSettings.Get().FilterUncategorizedPostsFromLists)
                posts = posts.Where(x => x.CategoryId != _categoryService.UnCategorizedId()).ToList();

            switch (sot) {
                case SortOrderType.Ascending:
                    posts = posts.OrderBy(x => x.Published).ToList();
                    break;
                case SortOrderType.Views:
                    posts = posts.OrderBy(x => x.Views).ToList();
                    break;
                case SortOrderType.Custom:
                    posts = posts.OrderBy(x => x.SortOrder).ToList();
                    break;
                case SortOrderType.Alphabetical:
                    posts = posts.OrderBy(x => x.Title).ToList();
                    break;
                default:
                    posts = posts.OrderByDescending(x => x.Published).ToList();
                    break;
            }
            return posts;
        }

        public IList<Post> DefaultQuery(int pageIndex, int pageSize, SortOrderType sot) 
        {
            IList<Post> posts = DefaultQuery(sot);
            if (pageIndex > 0 && pageSize > 0 && posts.Count > pageSize)
                posts = posts.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return posts;
        }

        public IList<Post> HomeQueryOverride(int pageIndex, int pageSize) 
        {
            IList<Post> posts =  FetchPosts()
                .Where(x => x.IsPublished && !x.IsDeleted && x.IsHome && x.Published <= SiteSettings.CurrentUserTime)
                .OrderBy(x => x.HomeSortOrder).ToList();
            if (pageIndex > 0 && pageSize > 0 && posts.Count > pageSize)
                posts = posts.Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();
            return posts;
        }

        #region helpers

        private static void DeletePostDirectory(Post p) {
            string dir = HttpContext.Current.Server.MapPath(p.Url);

            if (Directory.Exists(dir)) {
                try {
                    Directory.Delete(dir, true);
                } catch (Exception ex) {
                    Log.Error("Post Directory Delete", "The directory for {0} could not be deleted. {1}", p.Title, ex.Message);
                }
            }
        }

        #endregion


        public void WritePostPages(Post post) 
        {
            PageTemplateToolboxContext templateContext = new PageTemplateToolboxContext();
            templateContext.Put("PostId", post.Id);
            templateContext.Put("CategoryId", post.CategoryId);
            templateContext.Put("PostName", post.Name);
            templateContext.Put("Name", post.Name);
            templateContext.Put("CategoryName", post.Category.LinkName);
            templateContext.Put("MetaDescription",
                                !string.IsNullOrEmpty(post.MetaDescription)
                                    ? post.MetaDescription
                                    : HttpUtility.HtmlEncode(Util.RemoveHtml(post.PostBody, 255) ?? string.Empty));

            templateContext.Put("MetaKeywords",
                                !string.IsNullOrEmpty(post.MetaKeywords)
                                    ? post.MetaKeywords
                                    : post.TagList);

            string pageName = null;

            if (_categoryService.UnCategorizedId() != post.CategoryId)
                pageName = _categoryService.FetchCachedCategory(post.CategoryId, false).LinkName + "/";

            pageName = "~/" + pageName + post.Name + "/" + Util.DEFAULT_PAGE;

            PageWriter.Write("post.view", pageName, templateContext);

        }

        #region old helper code

//        public static Post FromXML(string xml) {
//            Post the_Post = ObjectManager.ConvertToObject<Post>(xml);
//            the_Post.Loaded();
//            the_Post.ResetStatus();

//            return the_Post;
//        }

//        public string ToXML() {
//            SerializeCustomFields();
//            return ObjectManager.ConvertToString(this);
//        }

        #endregion

    }
}
