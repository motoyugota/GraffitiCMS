using System;
using System.Collections.Generic;

namespace Graffiti.Core 
{
    [Serializable]
    public class RoleCategoryPermissionsCollection : List<RoleCategoryPermissions> 
    {
        public RoleCategoryPermissionsCollection() : base(new List<RoleCategoryPermissions>()) { }

        public RoleCategoryPermissionsCollection(IEnumerable<RoleCategoryPermissions> roleCategoryPermissions)
        {
            this.AddRange(roleCategoryPermissions);
        }
    }
}
