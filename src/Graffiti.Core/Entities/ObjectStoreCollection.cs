using System;
using System.Collections.Generic;

namespace Graffiti.Core 
{
    [Serializable]
    public class ObjectStoreCollection : List<ObjectStore> 
    {
        public ObjectStoreCollection() : base(new List<ObjectStore>()) { }

        public ObjectStoreCollection(IEnumerable<ObjectStore> objectStores)
        {
            this.AddRange(objectStores);
        }        
    }
}
