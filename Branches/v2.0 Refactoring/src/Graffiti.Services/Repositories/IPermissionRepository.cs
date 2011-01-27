using System.Linq;
using Graffiti.Core;

namespace Graffiti.Data 
{
    public interface IPermissionRepository 
    {
        IQueryable<RolePermissions> FetchAllRolePermissions();
        IQueryable<RoleCategoryPermissions> FetchAllRoleCategoryPermissions();
        void DestroyRolePermission(int id);
        void DestroyRolePermission(string roleName);
        void DestroyRoleCategoryPermission(int id);
        void DestroyRoleCategoryPermission(string roleName);
        RolePermissions SaveRolePermission(RolePermissions rolePermissions);
        RoleCategoryPermissions SaveRoleCategoryPermission(RoleCategoryPermissions roleCategoryPermissions);
    }
}
