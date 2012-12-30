using System.Linq;
using Graffiti.Core;

namespace Graffiti.Data 
{
    public interface ICommentRepository 
    {
        Comment FetchComment(object commentId);
        IQueryable<Comment> FetchComments();
        Comment SaveComment(Comment comment);
        Comment SaveComment(Comment comment, string username);
        void DeleteComment(int commentId);
        void DeleteDeletedComments();
        void DeleteUnpublishedComments();
        int GetPublishedCommentCount(string username);
    }
}
