using System;
using System.Collections.Generic;
using System.Linq;
using Graffiti.Core;

namespace Graffiti.Data 
{
    public interface IPostRepository 
    {
        Post FetchPost(int id);
        Post FetchPost(string id);
        Post FetchPostByUniqueId(Guid unqiueId);
        IQueryable<Post> FetchPostsByTag(string tag);
        IQueryable<Post> FetchPostsByTagAndCategory(string tagName, int categoryId);
        IQueryable<Post> FetchPostsByCategory(int categoryId);
        IQueryable<Post> FetchPostsByUsername(string username);
        IQueryable<Post> FetchRecentPosts(int numberOfPosts);
        IQueryable<Post> FetchPosts(int numberOfPosts, SortOrderType sortOrder);
        IQueryable<Post> FetchPosts();

        int GetPostIdByName(string name);
        List<PostCount> GetPostCounts(IGraffitiUser user, int categoryId, string username, Delegate categoryPermissionCheckCallback);
        List<CategoryCount> GetCategoryCountForStatus(IGraffitiUser user, PostStatus postStatus, string author, Delegate categoryPermissionCheckcallback);
        List<AuthorCount> GetAuthorCountForStatus(IGraffitiUser user, PostStatus status, string categoryId, Delegate categoryPermissionCheckCallback);

        void UpdateViewCount(int postId);
        void UpdatePostStatus(int postId, PostStatus status);
        Post SavePost(Post post);
        Post SavePost(Post post, string username);
        Post SavePost(Post post, string username, DateTime updateDate);

        void DeletePost(object postId);
        void DestroyDeletedPost(int postId);
        void DestroyDeletedPosts();
    }
}
