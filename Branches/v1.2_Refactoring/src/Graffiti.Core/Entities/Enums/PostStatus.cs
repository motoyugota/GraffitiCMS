
namespace Graffiti.Core
{
    /// <summary>
    /// Defines the state of a post.
    /// </summary>
    public enum PostStatus 
    {
        NotSet = 0,
        Publish = 1,
        Draft = 2,
        PendingApproval = 3,
        RequiresChanges = 4
    }

}
