using System;
using Graffiti.Core;

namespace Graffiti.Data 
{
    public interface IUserRepository 
    {
        User SaveUser(User user);
        User SaveUser(User user, string modifiedBy);
        User FetchUserByUsername(string username);
        User FetchUserById(int id);
        User FetchUserByUniqueId(Guid uniqueId);
        void DestroyUser(Guid uniqueId);
    }
}
