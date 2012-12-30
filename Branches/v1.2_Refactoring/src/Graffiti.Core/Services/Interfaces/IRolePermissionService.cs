
namespace Graffiti.Core.Services
{
    public interface IRolePermissionService
    {
        bool IsDuplicate(string roleName);
        RolePermissionsCollection GetRolePermissions();
        RoleCategoryPermissionsCollection GetRoleCategoryPermissions();
        void ClearPermissionsForRole(string role);
        bool CanViewControlPanel(IGraffitiUser user);
        string GetInClauseForReadPermissions(IGraffitiUser user);
        Permission GetPermissions(int categoryId, IGraffitiUser user);
        Permission GetPermissions(int categoryId, IGraffitiUser user, bool calledFromMultipleCategoryPage);
        bool IsEveryoneAContentPublisher();
        void DestroyRolePermission(string roleName);
        void DestroyRoleCategoryPermission(string roleName);
        RolePermissions SaveRolePermission(RolePermissions rolePermissions);
        RoleCategoryPermissions SaveRoleCategoryPermission(RoleCategoryPermissions roleCategoryPermissions);
        void MarkDirty();
    }

    public delegate Permission CategoryPermissionCheck(int categoryId, IGraffitiUser user);

    public delegate string GetInClauseForReadPermissionsCheck(IGraffitiUser user);

}