using System.Collections.Generic;
using System.Linq;
using Graffiti.Core;
using Graffiti.Core.Services;
using Graffiti.Data;

namespace Graffiti.Services 
{
    public class CommentService : ICommentService
    {
        private ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public Comment FetchComment(object commentId)
        {
            return _commentRepository.FetchComment(commentId);
        }

        public IList<Comment> FetchComments() 
        {
            return _commentRepository.FetchComments().ToList();
        }

        public Comment SaveComment(Comment comment)
        {
            return _commentRepository.SaveComment(comment);
        }

        public Comment SaveComment(Comment comment, string username)
        {
            return _commentRepository.SaveComment(comment, username);
        }

        public void DeleteComment(int commentId)
        {
            _commentRepository.DeleteComment(commentId);
        }

        public void DeleteDeletedComments()
        {
            _commentRepository.DeleteDeletedComments();
        }
        
        public void DeleteUnpublishedComments()
        {
            _commentRepository.DeleteUnpublishedComments();
        }

        public int GetPublishedCommentCount(string username)
        {
            return _commentRepository.GetPublishedCommentCount(username);
        }
    }
}
