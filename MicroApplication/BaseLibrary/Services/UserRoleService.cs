
namespace BaseLibrary.Services
{
    public interface IUserRoleService
    {
        List<UserRole> GetUserRoles(Guid userId);
        UserRole DeleteUserRole(Guid Id, Guid organizationId);
        void SaveUpdate(Guid userId, List<Guid> roles);
        List<SmartControlOption> GetRolePermissionOptions(Guid? roleId);
        List<string> GetUserAllPermissions(ApplicationUser user);
        bool IsInRole(ApplicationRole role, ApplicationUser user);
        bool IsOperationAllowed(ApplicationUser loggedInUser, Permission? permission);
        bool IsInWebsiteAdminRole(ApplicationUser loggedInUser);
        bool IsInOrganizationAdminRole(ApplicationUser loggedInUser);
    }

    public class UserRoleService : ServiceLibraryBase, IUserRoleService
    {
        public UserRoleService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<UserRoleService>())
        {

        }

        public void SaveUpdate(Guid userId, List<Guid> roleIds)
        {
            var userExistingRoles = SF.UserRoleService.GetUserRoles(userId);
            var userRoles = userExistingRoles.ToList();

            //remove role that is not required
            foreach (var userExistingRole in userRoles.ToList())
            {
                if (roleIds.Any(r => r == userExistingRole.RoleId))
                    continue;
                RF.UserRoleRepository.Remove(userExistingRole);
                userExistingRoles.Remove(userExistingRole);
            }
            //add if role not exists
            foreach (var roleId in roleIds)
            {
                if (userRoles.Any(r => r.RoleId == roleId))
                    continue;
                var userrole = UserRole.Create(roleId, userId);
                RF.UserRoleRepository.Add(userrole);
            }
        }

        public List<UserRole> GetUserRoles(Guid userId)
        {
            var userRoles = RF.UserRoleRepository.GetUserRoles(userId).ToList();
            return userRoles;
        }

        public UserRole DeleteUserRole(Guid Id, Guid organizationId)
        {
            var userRole = RF.UserRoleRepository.Get(Id);
            if (userRole != null)
            {
                var role = RF.RoleRepository.Get(userRole.RoleId);
                if(role?.OrganizationId==organizationId)
                    RF.UserRoleRepository.Remove(userRole);
            }

            return userRole;
        }

        public List<SmartControlOption> GetRolePermissionOptions(Guid? roleId)
        {
            var options = new List<SmartControlOption>();
            var permissions = SF.MicroAppContract.GetApplicationPermission().GetPermissions();
            var rolePermissions = new List<ApplicationRolePermission>();
            if (roleId.HasValue)
                rolePermissions = RF.ApplicationRolePermissionRepository.GetRolePermissions(roleId.Value);
            foreach (var permission in permissions)
            {
                var isSelected = false;
                var rolePermission = rolePermissions.FirstOrDefault(r => r.PermissionId == permission.Id);
                if (rolePermission != null)
                    isSelected = true;
                options.Add(new SmartControlOption(permission.Name, permission.Id.ToString(), isSelected));
            }
            return options;
        }

        public List<string> GetUserAllPermissions(ApplicationUser user)
        {
            if (IsInWebsiteAdminRole(user))
                return WebsiteAdminPermissions();
            if (IsInOrganizationAdminRole(user))
                return GetOrganizationAdminPermissions();
            return GetUserPermissions(user);
        }
        private List<string> GetUserPermissions(ApplicationUser user)
        {
            var userPermissions = RF.UserRoleRepository.GetUserPermissions(user.Id);
            var permissions = SF.MicroAppContract.GetApplicationPermission().GetPermissions();
            var result = new List<Permission>();
            foreach (var rolePermission in userPermissions)
            {
                var permission = permissions.First(p => p.Id == rolePermission.PermissionId);
                result.Add(permission);
            }
            return result.Select(x => x.Code).ToList();
        }

        private List<string> WebsiteAdminPermissions()
        {
            var permissions = SF.MicroAppContract.GetApplicationPermission().GetWebsiteAdminPermissions();
            return permissions.Select(x => x.Code).ToList();
        }

        private List<string> GetOrganizationAdminPermissions()
        {
            var permissions = SF.MicroAppContract.GetApplicationPermission().GetOrganizationAdminPermissions();
            return permissions.Select(x => x.Code).ToList();
        }

        public bool IsInRole(ApplicationRole role, ApplicationUser user)
        {
            var roles = RF.UserRoleRepository.GetUserRoles(user.Id);
            return roles.Any(r => r.RoleId == role.Id);
        }

        public bool IsOperationAllowed(ApplicationUser loggedInUser, Permission? permission)
        {
            return RF.UserRoleRepository.HasPermission(loggedInUser, permission);
        }

        public bool IsInWebsiteAdminRole(ApplicationUser loggedInUser)
        {
            var adminIds = SF.MicroAppContract.GetWebsiteAdminLogins();
            return adminIds.Any(l => C.AreEqualsIgnoreCase(l, loggedInUser.LoginId));
        }

        public bool IsInOrganizationAdminRole(ApplicationUser loggedInUser)
        {
            if(loggedInUser.IsOrgAdmin)
                return true;
            return IsInRole(ApplicationRole.OrganizationAdminRole, loggedInUser);
        }
    }
}
