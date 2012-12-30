using System;
using System.Collections.Generic;

namespace Graffiti.Core 
{
    [Serializable]
    public class RolePermissionsCollection : List<RolePermissions> 
    {
        public RolePermissionsCollection() : base(new List<RolePermissions>()) { }

        public RolePermissionsCollection(IEnumerable<RolePermissions> rolePermissions)
        {
            this.AddRange(rolePermissions);
        }           
    }
}
