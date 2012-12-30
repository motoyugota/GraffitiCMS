using System;
using System.Collections.Generic;

namespace Graffiti.Core 
{
    [Serializable]
    public class PostCollection : List<Post> 
    {
        public PostCollection() : base(new List<Post>()) { }

        public PostCollection(IEnumerable<Post> posts)
        {
            if (posts != null)
                AddRange(posts);
        }
    }

}
