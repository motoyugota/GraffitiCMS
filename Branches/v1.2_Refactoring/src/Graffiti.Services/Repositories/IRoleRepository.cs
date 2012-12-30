using System.Collections.Generic;
using System.Linq;
using Graffiti.Core;

namespace Graffiti.Data 
{
    public interface IRoleRepository 
    {
        IQueryable<UserRole> FetchUserRolesForUserByRoleName(int userId, string roleName);
        UserRole SaveUserRole(UserRole userRole);
        void DestroyUserRole(int userRoleId);
        List<string> FetchUsernamesInRole(string roleName);
    }
}
