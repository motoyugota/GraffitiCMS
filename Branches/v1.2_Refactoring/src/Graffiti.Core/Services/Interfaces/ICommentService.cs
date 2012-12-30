using System.Collections.Generic;

namespace Graffiti.Core.Services 
{
    public interface ICommentService 
    {
        Comment FetchComment(object commentId);
        IList<Comment> FetchComments();
        Comment SaveComment(Comment comment);
        Comment SaveComment(Comment comment, string username);
        void DeleteComment(int commentId);
        void DeleteDeletedComments();
        void DeleteUnpublishedComments();
        int GetPublishedCommentCount(string username);
    }
}
