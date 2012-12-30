using System;
using System.Collections.Generic;

namespace Graffiti.Core 
{
    [Serializable]
    public class VersionStoreCollection : List<VersionStore> 
    {
        public VersionStoreCollection() : base(new List<VersionStore>()) { }

        public VersionStoreCollection(IEnumerable<VersionStore> versionStores)
        {
            this.AddRange(versionStores);
        }    
    }
}
