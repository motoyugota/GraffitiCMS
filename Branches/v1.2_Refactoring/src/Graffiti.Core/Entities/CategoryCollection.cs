using System;
using System.Collections.Generic;

namespace Graffiti.Core 
{
    [Serializable]
    public class CategoryCollection : List<Category> 
    {
        public CategoryCollection() : base(new List<Category>()) { }

        public CategoryCollection(IEnumerable<Category> categories)
        {
            this.AddRange(categories);
        }
    }
}
