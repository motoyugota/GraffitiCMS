using System;
using System.Collections.Generic;

namespace Graffiti.Core 
{
    [Serializable]
    public class CommentCollection : List<Comment> 
    {

        public CommentCollection() : base(new List<Comment>()) { }


        public CommentCollection(IEnumerable<Comment> comments)
        {
            this.AddRange(comments);
        }

    }
}
