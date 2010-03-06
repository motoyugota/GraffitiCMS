using System;
using System.Collections.Generic;

namespace Graffiti.Core 
{
    [Serializable]
    public class UserRoleCollection : List<UserRole> 
    {
        public UserRoleCollection() : base(new List<UserRole>()) { }

        public UserRoleCollection(IEnumerable<UserRole> userRoles)
        {
            this.AddRange(userRoles);
        }        
    }
}
