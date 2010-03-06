using System;
using System.Collections.Generic;

namespace Graffiti.Core
{
    [Serializable]
    public class TagWeightCollection : List<TagWeight> 
    {
        public TagWeightCollection() : base(new List<TagWeight>()) { }

        public TagWeightCollection(IEnumerable<TagWeight> tagWeights)
        {
            this.AddRange(tagWeights);
        }
    }
}
