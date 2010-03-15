using System;
using Graffiti.Core;
using Graffiti.Core.Services;
using Graffiti.Data;

namespace Graffiti.Services
{
    public class GraffitiCoreUserService : IGraffitiUserService
    {
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;

        public GraffitiCoreUserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public bool ChangePassword(string username, string old_password, string new_password)
        {
			username = username.ToLower();
            User user = Login(username, old_password) as User;
            if(user != null)
            {
                user.Password = new_password;
                GraffitiUsers.Save(user, null);
                return true;
            }

            return false;
        }

        public bool ChangePassword(string username, string password)
        {
			username = username.ToLower();
            User user = _userRepository.FetchUserByUsername(username);
            if(!user.IsNew)
            {
                user.Password = password;
                GraffitiUsers.Save(user,null);
                return true;
            }

            return false;
        }

        public IGraffitiUser Login(string username, string password)
        {
			username = username.ToLower();
            User user = GetUser(username) as User;
            if (user != null && user.IsLoaded && !user.IsNew)
            {
					if (password.Equals(user.Password))
						return user;
               else if (user.PasswordFormat == 1 && User.EncodePassword(password, user.PasswordSalt) == user.Password)
                  return user;
            }
            return null;

        }

        public void CreateUser(string username, string password, string email, string role)
        {
			username = username.ToLower();

            User user = new User();
            user.Name = username;
            user.Password = password;
            user.Email = email;

            _userRepository.SaveUser(user);

            UserRole userRole = new UserRole();
            userRole.UserId = user.Id;
            userRole.RoleName = role;
            userRole = _roleRepository.SaveUserRole(userRole);
        }

        public IGraffitiUser GetUser(string username)
        {
			username = username.ToLower();

            User user =  _userRepository.FetchUserByUsername(username);
            if (user.IsLoaded && !user.IsNew)
                return user;

            return null;
        }

        public IGraffitiUser GetUser(int id)
        {
            return _userRepository.FetchUserById(id);
        }

        public IGraffitiUser GetUser(Guid id) 
        {
            return _userRepository.FetchUserByUniqueId(id);
        }

        //public List<IGraffitiUser> GetUsers(string role)
        //{
        //    throw new NotImplementedException();
        //}

        public string[] GetUsersInRole(string roleName)
        {
            return _roleRepository.FetchUsernamesInRole(roleName).ToArray();
        }

        public void RemoveUserFromRole(string userName, string roleName)
        {
			userName = userName.ToLower();
            UserRoleCollection urCol = new UserRoleCollection(_roleRepository.FetchUserRolesForUserByRoleName((GraffitiUsers.GetUser(userName) as User).Id, roleName));
            if (urCol.Count > 0)
                _roleRepository.DestroyUserRole(urCol[0].Id);         
        }

        public void AddUserToRole(string userName, string RoleName)
        {
			userName = userName.ToLower();
            User user = (User)GraffitiUsers.GetUser(userName);

            UserRoleCollection urCol = new UserRoleCollection(_roleRepository.FetchUserRolesForUserByRoleName(user.Id, RoleName));

            if (urCol.Count == 0)
            {
                UserRole ur = new UserRole();
                ur.UserId = user.Id;
                ur.RoleName = RoleName;
                ur = _roleRepository.SaveUserRole(ur);
            }
        }

        public void Save(IGraffitiUser user, string modifed_by)
        {
            User internal_User = user as User;
            internal_User = _userRepository.SaveUser(internal_User, modifed_by);
        }

        public void AddRole(string roleName)
        {
        }

        public void DeleteRole(string roleName)
        {
        }

        public bool CanDeleteUsers
        {
            get { return true; }
        }

        public void DeleteUser(IGraffitiUser user)
        {
            _userRepository.DestroyUser(user.UniqueId);
        }

        public bool CanRenameUsers
        {
            get { return true; }
        }

        public void RenameUser(string oldUserName, string newUserName)
        {
            oldUserName = oldUserName.ToLower();
            User user = (User)GraffitiUsers.GetUser(oldUserName);
            user.Name = newUserName;
            GraffitiUsers.Save(user, null);
        }


    }
}