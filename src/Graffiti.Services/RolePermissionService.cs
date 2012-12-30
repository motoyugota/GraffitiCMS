using System.Text;
using Graffiti.Core;
using Graffiti.Core.Services;
using Graffiti.Data;

namespace Graffiti.Services
{   
    public class RolePermissionService : IRolePermissionService
    {
        private ICategoryService _categoryService;
        private IPermissionRepository _permissionRepository;

        public RolePermissionService(IPermissionRepository permissionRepository, ICategoryService categoryService)
        {
            _permissionRepository = permissionRepository;
            _categoryService = categoryService;
        }

        public void MarkDirty()
        {
            rolePermissions = null;
            roleCategoryPermissions = null;
        }

        private static RolePermissionsCollection rolePermissions;
        private static RoleCategoryPermissionsCollection roleCategoryPermissions;

        public bool IsDuplicate(string roleName)
        {
            GetRolePermissions();

            foreach (RolePermissions rp in rolePermissions)
            {
                if (rp.RoleName.ToLower() == roleName.ToLower())
                    return true;
            }

            return false;
        }

        public RolePermissionsCollection GetRolePermissions()
        {
            if (rolePermissions == null)
            {
                rolePermissions = new RolePermissionsCollection(_permissionRepository.FetchAllRolePermissions());

                #region This block will only run the first time this method is called to insert the everyone/manager/contributor roles

                // check for and insert the everyone role
                RolePermissions temp = rolePermissions.Find(
                                            delegate(RolePermissions rp)
                                            {
                                                return rp.RoleName == GraffitiUsers.EveryoneRole;
                                            });

                if (temp == null)
                {
                    GraffitiUsers.AddUpdateRole(GraffitiUsers.EveryoneRole, true, false, false);
                    rolePermissions = new RolePermissionsCollection(_permissionRepository.FetchAllRolePermissions());
                }

                // check for and insert the manager role
                temp = rolePermissions.Find(
                                            delegate(RolePermissions rp)
                                            {
                                                return rp.RoleName == GraffitiUsers.ManagerRole;
                                            });

                if (temp == null)
                {
                    GraffitiUsers.AddUpdateRole(GraffitiUsers.ManagerRole, true, true, true);
                    rolePermissions = new RolePermissionsCollection(_permissionRepository.FetchAllRolePermissions());
                }

                // check for and insert the comtributor role
                temp = rolePermissions.Find(
                                            delegate(RolePermissions rp)
                                            {
                                                return rp.RoleName == GraffitiUsers.ContributorRole;
                                            });

                if (temp == null)
                {
                    GraffitiUsers.AddUpdateRole(GraffitiUsers.ContributorRole, true, true, false);
                    rolePermissions = new RolePermissionsCollection(_permissionRepository.FetchAllRolePermissions());
                }

                #endregion
            }

            return rolePermissions;
        }

        public RoleCategoryPermissionsCollection GetRoleCategoryPermissions()
        {
            if (roleCategoryPermissions == null)
            {
                roleCategoryPermissions = new RoleCategoryPermissionsCollection(_permissionRepository.FetchAllRoleCategoryPermissions());
            }

            return roleCategoryPermissions;
        }

        public void ClearPermissionsForRole(string role)
        {
            GetRolePermissions();
            GetRoleCategoryPermissions();

            foreach (RoleCategoryPermissions rcp in roleCategoryPermissions)
            {
                if (rcp.RoleName.ToLower() == role.ToLower())
                    _permissionRepository.DestroyRoleCategoryPermission(rcp.Id);
            }

            foreach (RolePermissions rp in rolePermissions)
            {
                if (rp.RoleName.ToLower() == role.ToLower())
                    _permissionRepository.DestroyRolePermission(rp.Id);
            }
        }

        public bool CanViewControlPanel(IGraffitiUser user)
        {
            if (user == null)
                return false;

            if (GraffitiUsers.IsAdmin(user))
                return true;

            foreach (string role in user.Roles)
            {
                foreach (RolePermissions rp in GetRolePermissions())
                {
                    if (rp.RoleName == role)
                    {
                        if (rp.HasEdit || rp.HasPublish)
                            return true;
                    }
                }

                foreach (RoleCategoryPermissions rcp in GetRoleCategoryPermissions())
                {
                    if (rcp.RoleName == role)
                    {
                        if (rcp.HasEdit || rcp.HasPublish)
                            return true;
                    }
                }
            }

            return false;
        }

        public string GetInClauseForReadPermissions(IGraffitiUser user)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");

            bool first = true;

            foreach (Category c in _categoryService.FetchAllCachedCategories())
            {
                if (GetPermissions(c.Id, user).Read)
                {
                    if (first)
                    {
                        sb.Append(c.Id.ToString());
                        first = false;
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(c.Id.ToString());
                    }
                }
            }

            sb.Append(")");

            return sb.ToString();
        }

        public Permission GetPermissions(int categoryId, IGraffitiUser user)
        {
            return GetPermissions(categoryId, user, false);
        }

        public Permission GetPermissions(int categoryId, IGraffitiUser user, bool calledFromMultipleCategoryPage)
        {
            string[] roles;

            // if there is no users, setup the roles collection to be everyone
            if (user == null)
            {
                roles = new string[1] { GraffitiUsers.EveryoneRole };
            }
            else // get the users roles
                roles = user.Roles;

            Permission p = new Permission();

            // if the user is an admin, they have access to everything
            if(GraffitiUsers.IsAdmin(user))
            {
                p.Read = true;
                p.Edit = true;
                p.Publish = true;

                return p;
            }

            // determines if category permissions are setup, which overrides individual role permissions
            bool setInCategoryPermissions = false;

            if (categoryId != -1 || calledFromMultipleCategoryPage)
            {
                foreach (string role in roles)
                {
                    foreach (RoleCategoryPermissions rcp in GetRoleCategoryPermissions())
                    {
                        if (rcp.RoleName == role)
                        {
                            if (rcp.CategoryId == categoryId || calledFromMultipleCategoryPage)
                            {
                                // only set it if it's false. if another permissions allowed this category,
                                // the user has permissions
                                if (!p.Read)
                                    p.Read = rcp.HasRead;

                                if (!p.Edit)
                                    p.Edit = rcp.HasEdit;

                                if (!p.Publish)
                                    p.Publish = rcp.HasPublish;
                            }

                            setInCategoryPermissions = true;
                        }
                    }
                }
            }

            if (!setInCategoryPermissions)
            {
                foreach (string role in roles)
                {
                    foreach (RolePermissions rp in GetRolePermissions())
                    {
                        if (rp.RoleName == role)
                        {
                            // only set it if it's false. if another permissions allowed,
                            // the user has permissions
                            if (!p.Read)
                                p.Read = rp.HasRead;

                            if (!p.Edit)
                                p.Edit = rp.HasEdit;

                            if (!p.Publish)
                                p.Publish = rp.HasPublish;
                        }
                    }
                }
            }
            
            return p;
        }

        public bool IsEveryoneAContentPublisher()
        {
            RolePermissionsCollection rpc = GetRolePermissions();

            foreach (RolePermissions rp in rpc)
            {
                if (rp.RoleName == GraffitiUsers.EveryoneRole)
                {
                    if (rp.HasEdit || rp.HasPublish)
                        return true;
                }
            }

            return false;
        }

        public void DestroyRolePermission(string roleName)
        {
            _permissionRepository.DestroyRolePermission(roleName);
        }

        public void DestroyRoleCategoryPermission(string roleName)
        {
            _permissionRepository.DestroyRoleCategoryPermission(roleName);
        }

        public RolePermissions SaveRolePermission(RolePermissions rolePermissions)
        {
            return _permissionRepository.SaveRolePermission(rolePermissions);
        }
        
        public RoleCategoryPermissions SaveRoleCategoryPermission(RoleCategoryPermissions roleCategoryPermissions)
        {
            return _permissionRepository.SaveRoleCategoryPermission(roleCategoryPermissions);       
        }

    }
}
