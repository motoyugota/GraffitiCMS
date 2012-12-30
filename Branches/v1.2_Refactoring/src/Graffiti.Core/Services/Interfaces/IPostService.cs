using System;
using System.Collections.Generic;

namespace Graffiti.Core.Services 
{
    public interface IPostService 
    {
        Post FetchPost(int id);
        Post FetchPost(string id);
        Post FetchCachedPost(int id);
        Post FetchPostByUniqueId(Guid uniqueId);

        int GetPostIdByName(string name);

        IList<Post> FetchPostsByCategory(int categoryId);
        IList<Post> FetchPostsByTag(string tag);
        IList<Post> FetchPosts();
        IList<Post> FetchPosts(int numberOfPosts, SortOrderType sortOrder);
        IList<Post> FetchPostsByTagAndCategory(string tagName, int categoryId);
        IList<Post> FetchPostsByUsername(string username);
        IList<Post> FetchRecentPosts(int numberOfPosts);
        IList<Post> DefaultQuery();
        IList<Post> DefaultQuery(int pageIndex, int pageSize, SortOrderType sot);
        IList<Post> HomeQueryOverride(int pageIndex, int pageSize);

        List<PostCount> GetPostCounts(IGraffitiUser user, int categoryId, string username, Delegate categoryPermissionCheckCallback);
        List<CategoryCount> GetCategoryCountForStatus(IGraffitiUser user, PostStatus postStatus, string author, Delegate categoryPermissionCheckCallback);
        List<AuthorCount> GetAuthorCountForStatus(IGraffitiUser user, PostStatus status, string categoryId, Delegate categoryPermissionCheckCallback);

        void UpdateViewCount(int postId);
        void UpdatePostStatus(int postId, PostStatus postStatus);
        Post SavePost(Post post);
        Post SavePost(Post post, string username);
        Post SavePost(Post post, string username, DateTime updateDate);

        void WritePostPages(Post post); 

        void DeletePost(object postId);
        void DestroyDeletedPost(Post post);
        void DestroyDeletedPosts();
        void DestroyDeletedPostCascadingForCategory(int categoryId);

    }
}
