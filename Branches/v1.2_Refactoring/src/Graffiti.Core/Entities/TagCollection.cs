using System;
using System.Collections.Generic;

namespace Graffiti.Core 
{
    [Serializable]
    public class TagCollection : List<Tag>
    {
        public TagCollection() : base(new List<Tag>()) { }

        public TagCollection(IEnumerable<Tag> tags)
        {
            this.AddRange(tags);
        }        
    }
}
