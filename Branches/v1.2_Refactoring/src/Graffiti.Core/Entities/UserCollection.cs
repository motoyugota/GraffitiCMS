using System;
using System.Collections.Generic;

namespace Graffiti.Core 
{
    [Serializable]
    public class UserCollection : List<User> 
    {
        public UserCollection() : base(new List<User>()) { }

        public UserCollection(IEnumerable<User> users)
        {
            this.AddRange(users);
        }        
    }
}
