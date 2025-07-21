
namespace BaseLibrary.Services
{
    public interface IRolePermissionService
    {
        List<ApplicationRolePermission> GetRolePermissions(Guid roleId);
        void SaveUpdateRolePermissions(ApplicationRolePermissions model);
        ApplicationRolePermission Delete(Guid id);
        void SaveUpdateRolePermissions(Guid roleId, List<Guid?> permissions);
    }
    public class RolePermissionService : ServiceLibraryBase, IRolePermissionService
    {
        public RolePermissionService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<RolePermissionService>())
        {

        }
        public List<ApplicationRolePermission> GetRolePermissions(Guid roleId)
        {
            return RF.ApplicationRolePermissionRepository.GetRolePermissions(roleId);
        }

        public void SaveUpdateRolePermissions(ApplicationRolePermissions model)
        {
            var rolePermissions = RF.ApplicationRolePermissionRepository.GetRolePermissions(model.RoleId);
            model.Permissions = model.Permissions.Where(p => p.IsSelected).ToList();
            var existingPermissions = rolePermissions.ToList();
            foreach (var existingPermission in existingPermissions)
            {
                if (model.Permissions.Any(p => p.PermissionId == existingPermission.PermissionId))
                    continue;
                RF.ApplicationRolePermissionRepository.Remove(existingPermission);
                rolePermissions.Remove(existingPermission);
            }

            foreach (var permission in model.Permissions)
            {
                if (rolePermissions.Any(p => p.PermissionId == permission.PermissionId))
                    continue;
                var newPermission = ApplicationRolePermission.Create(model.RoleId, permission.PermissionId);
                RF.ApplicationRolePermissionRepository.Add(newPermission);
            }
        }

        public ApplicationRolePermission Delete(Guid id)
        {
            ApplicationRolePermission rolepermission = null;
            if (id != Guid.Empty)
                rolepermission = RF.ApplicationRolePermissionRepository.Get(id);

            if (rolepermission != null)
            {
                RF.ApplicationRolePermissionRepository.Remove(rolepermission);
            }
            return rolepermission;
        }

        public void SaveUpdateRolePermissions(Guid roleId, List<Guid?> permissions)
        {
            var rolePermissions = RF.ApplicationRolePermissionRepository.GetRolePermissions(roleId);
            permissions = permissions.Where(p => p.HasValue).ToList();
            var existingPermissions = rolePermissions.ToList();
            foreach (var existingPermission in existingPermissions)
            {
                if (permissions.Any(p => p == existingPermission.PermissionId))
                    continue;
                RF.ApplicationRolePermissionRepository.Remove(existingPermission);
                rolePermissions.Remove(existingPermission);
            }

            foreach (var permission in permissions)
            {
                if (rolePermissions.Any(p => p.PermissionId == permission))
                    continue;
                var newPermission = ApplicationRolePermission.Create(roleId, permission.Value);
                RF.ApplicationRolePermissionRepository.Add(newPermission);
            }
        }
    }
}
