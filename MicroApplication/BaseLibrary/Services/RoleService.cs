
namespace BaseLibrary.Services
{
    public interface IRoleService
    {
        ApplicationRole SaveUpdate(RoleVM model, ApplicationUser loggedInUser);
        List<ApplicationRole> GetOrganizationRoles(Guid OrganizationId);
        ApplicationRole Delete(Guid roleId, Guid OrganizationId);
    }

    public class RoleService : ServiceLibraryBase, IRoleService
    {
        public RoleService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<RoleService>())
        {

        }

        public ApplicationRole SaveUpdate(RoleVM model, ApplicationUser loggedInUser)
        {
            ApplicationRole? role = null;
            if (model.Id.HasValue)
                role = RF.RoleRepository.Get(model.Id.Value);
            if (role == null)
            {
                GaurdForDuplicateRoleName(model.Name, loggedInUser.GetOrganizationId());
                role = ApplicationRole.Create(model, loggedInUser.GetOrganizationId());
                RF.RoleRepository.Add(role);
            }
            role.Update(model);
            return role;
        }

        public List<ApplicationRole> GetOrganizationRoles(Guid OrganizationId)
        {
            var roles = RF.RoleRepository.GetOrganizationRoles(OrganizationId);
            return roles.ToList();
        }

        private void GaurdForDuplicateRoleName(string role, Guid organizationId)
        {
            var roles = RF.RoleRepository.GetOrganizationRoles(organizationId);
            if (roles.Any(r => AreEqualsIgnoreCase(r.Name, role)))
                throw new ValidationException($"Role with the {role} name already exists.");
        }

        public ApplicationRole Delete(Guid roleId, Guid organizationId)
        {
            ApplicationRole? role = RF.RoleRepository.Get(roleId);

            if (role is not null && role.OrganizationId == organizationId)
            {
                if (AreEqualsIgnoreCase(role.Name, "Organization Admin"))
                    throw new ValidationException("Cannot delete Organization Admin role.");
                RF.RoleRepository.Remove(role);
                return role;
            }
            throw new ValidationException("Role not found.");

        }
    }
}
