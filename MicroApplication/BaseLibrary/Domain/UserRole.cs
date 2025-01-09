using System.Data;

namespace BaseLibrary.Domain
{

    public class UserRole : Entity
    {
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }

        public static UserRole Create(Guid roleId, Guid userId)
        {
            var user_role = new UserRole
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                RoleId = roleId,
                UserId = userId,
            };
            
            return user_role;
        }

        public void Update(UserRoleVM model)
        {
            RoleId = model.RoleId;
            UserId = model.UserId;
            SetUpdatedOn();
        }

        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }

        internal string? GetValue(string controlIdentifier)
        {
            throw new NotImplementedException();
        }
    }

    public class UserRoleVM
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
       // public List<Permission> Permissions { get; }



        public UserRoleVM()
        {
            // public List<Permission> Permissions { get; set; }

        }
        public UserRoleVM(UserRole user_role)
        {
            Id = user_role.Id;
            RoleId = user_role.RoleId;
            UserId = user_role.UserId;
        }
    }

    public class UserRoles
    {
        public Guid UserId { get; set; }
         public List<UserRoleItemVM> Roles { get; set; }
        public UserRoles()
        {
            Roles = new List<UserRoleItemVM>();
        }
    }

    public class UserRoleItemVM
    {
        public Guid RoleId { get; set; }
        public string Role { get; set; }
        public string Permissions { get; set; }
        public bool IsSelected { get; set; }
        public UserRoleItemVM() { }

        public UserRoleItemVM(ApplicationRole role, List<ApplicationRolePermission> rolePermissions, List<Permission> permissions)
        {
            RoleId = role.Id;
            Role = role.Name;
            var roleExistingPermissions = permissions.Where(p => rolePermissions.Any(rp => rp.PermissionId == p.Id)).Select(p => p.Name);
            Permissions = string.Join(", ", roleExistingPermissions);
        }
    }
}
