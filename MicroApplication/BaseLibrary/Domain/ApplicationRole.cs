

namespace BaseLibrary.Domain
{
    public class ApplicationRoles
    {
        public const string OrganizationAdminRole = "Organization Admin";
    }
    public class ApplicationRole : Entity
    {
        public static ApplicationRole WebsiteAdminRole = new ApplicationRole("9066BFC1-184C-4BE4-AD12-F3AC944AE378","WebsiteAdmin");
        public static ApplicationRole OrganizationAdminRole = new ApplicationRole("604901E8-96D0-469B-88D3-325F8570177D", "OrganizationAdmin");
        public Guid OrganizationId { get; set; } 
        public string Name { get; set; } = string.Empty;

        public static ApplicationRole Create(RoleVM model,Guid organizationId)
        {
            var rol = new ApplicationRole
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                OrganizationId= organizationId
            };
            rol.Update(model);
            rol.SetCreatedOn();
            return rol;
        }
        public static ApplicationRole Create(string name, Guid organizationId)
        {
            var rol = new ApplicationRole
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                OrganizationId = organizationId,
                Name = name
            };
            rol.SetUpdatedOn();
            rol.SetCreatedOn();
            return rol;
        }

        public void Update(RoleVM model)
        {
            Name = model.Name;
            SetUpdatedOn();
        }
        protected ApplicationRole() { }
        protected ApplicationRole(string id,string name)
        {
            Id = Guid.Parse(id);
            Name = name;
        }

        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            X.Validator.GuidValidator.CheckForEmpltyOrDefaulValue(Id, "Id", validationResults);
            X.Validator.GuidValidator.CheckForEmpltyOrDefaulValue(OrganizationId, "OrganizationId", validationResults);
            

            X.Validator.StringValidator.CheckForMaxLength(Name, "Name", 32, validationResults);
            
            X.Validator.StringValidator.CheckForNullOrEmpty(Name, "Name", validationResults);
            
            return validationResults;
        }

    }


    public class RoleVM
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        public RoleVM() { }
        public RoleVM(ApplicationRole role)
        {
            Id = role.Id;
            Name = role.Name;
        }
    }
}
