
namespace BaseLibrary.Domain
{
    public class ApplicationRolePermission : Entity
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }


        protected ApplicationRolePermission()
        {

        }

        public static ApplicationRolePermission Create(Guid roleId, Guid PermissionId)
        {
            var rolePermission = new ApplicationRolePermission
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                RoleId = roleId,
                PermissionId = PermissionId,
            };
            return rolePermission;
        }

        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            X.Validator.GuidValidator.CheckForEmpltyOrDefaulValue(Id, "Id", validationResults);
            X.Validator.GuidValidator.CheckForEmpltyOrDefaulValue(PermissionId, "PermissionId", validationResults);

            return validationResults;
        }
        public string GetValue(string controlIdentifier)
        {
            return "";
        }
    }

    public class ApplicationRolePermissions 
    {
        public Guid RoleId { get; set; }
        public List<RolePermissionVM> Permissions { get; set; }
        public ApplicationRolePermissions() { Permissions = new List<RolePermissionVM>(); }
    }

    public class RolePermissionVM
    {
        public Guid PermissionId { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }

        public RolePermissionVM() { }
        public RolePermissionVM(List<ApplicationRolePermission> rolePermissions, Permission permission)
        {
            var rolePermission = rolePermissions.FirstOrDefault(rp => rp.PermissionId == permission.Id);
            if (rolePermission != null)
            {
                IsSelected = true;
            }
            PermissionId = permission.Id;
            Name = permission.Name;
        }
    }

    public class RolePermissionListVM
    {
        public Guid RoleId { get; set; }
        public string Role { get; set; }
        public string Permissions { get; set; }

        public RolePermissionListVM() { }

        public RolePermissionListVM(ApplicationRole role, List<ApplicationRolePermission> rolePermissions, List<Permission> permissions)
        {
            RoleId = role.Id;
            Role = role.Name;
            var roleExistingPermissions = permissions.Where(p => rolePermissions.Any(rp => rp.PermissionId == p.Id)).Select(p => p.Name);
            Permissions = string.Join(", ", roleExistingPermissions);
        }
    }
}
