using BaseLibrary.Configurations;

namespace BaseLibrary.Domain
{
    public class ApplicationUser : FormStoreBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public int? Salt { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsOrgAdmin { get; set; }
        public string? BlockReason { get; set; }
        public DateTime? LastLogin { get; set; }
        public Guid SessionId { get; set; }
        public string? PasswordResetCode { get; set; }
        public DateTime? PasswordResetCodeTimeStamp { get; set; }
        public string? Role { get; set; }
        public Guid? DefaultStudyId { get;set;}
        public string? TimeZone { get; set; }
        public static ApplicationUser Create(Guid id,string name,string email,string contactNumber)
        {
            return new ApplicationUser
            {
                Id = id,
                Name = name,
                Email = email,
                LoginId = email,
                ContactNumber = contactNumber,
                Password = "12345"
            };
        }
        public bool IsPasswordHashMatching(string password)
        {
            if(Salt.HasValue)
                return Password == X.Security.PasswordHasher.ComputeSaltedHash(password, Salt.Value);
            return IsPasswordMatching(password);
        }
        public bool IsPasswordMatching(string password)
        {
            return Password == password;
        }
        public string GetPasswordHash(int salt)
        {
            return X.Security.PasswordHasher.ComputeSaltedHash(Password, salt);
        }

        public bool IsPasswordResetCodeExpire()
        {
            double totalMinutes = DateTime.UtcNow.Subtract((DateTime)PasswordResetCodeTimeStamp).TotalMinutes;
            if (totalMinutes <= 5)
                return false;
            else
                return true;
        }

        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }

        protected ApplicationUser() { }
        public ApplicationUser(ApplicationUser loggedInUser) : base(loggedInUser)
        {
            SetCreatedOn();
        }

        public void Update(SmartFormTemplateRequest model)
        {
            base.UpdateData(model.ControlValues);
            Name = ControlReader.GetControlFirstValue(BaseControls.Name, model.ControlValues);
            Email = ControlReader.GetControlFirstValue(BaseControls.Email, model.ControlValues); //GetControlValue(model.ControlValues, "Email"); 
            ContactNumber = ControlReader.GetControlFirstValue(BaseControls.ContactNumber, model.ControlValues);
            LoginId = ControlReader.GetControlFirstValue(BaseControls.LoginId, model.ControlValues);
            LastLogin = DateTime.UtcNow;
            SetPassword("12345");
            SetUpdatedOn();
        }
        public void SetPassword(string password)
        {
            Salt = PasswordHasher.CreateRandomSalt();
            Password = new PasswordHasher().ComputeSaltedHash(password, Salt.Value);
        }
        public void UpdateOrganization(SmartFormTemplateRequest model)
        {
            var org = ControlReader.GetControlFirstValue(BaseControls.Organization, model.ControlValues);
            if (Guid.TryParse(org, out var orgId))
            {
                OrganizationId = orgId;
                return;
            }
            throw new ValidationException("Invalid organization Id.");
        }
        public string GetTimeZone()
        {
            if(TimeZone is not null)
                return TimeZone.ToString();
            if(string.IsNullOrWhiteSpace(ApplicationSettingBase.DefaultTimeZone)==false)
                return ApplicationSettingBase.DefaultTimeZone;
            return TimeZoneInfo.Local.StandardName;
        }
    }

    public class UserFilterVM
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string LoginId { get; set; }
        public bool IsBlocked { get; set; }
        public Guid? OrganizationId { get; set; }
    }

    
    public class ApplicationUserListVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string LoginId { get; set; }

        public ApplicationUserListVM()
        {

        }

        public ApplicationUserListVM(ApplicationUser applicationUser)
        {
            Id = applicationUser.Id;
            Name = applicationUser.Name;
            Email = applicationUser.Email;
            ContactNumber = applicationUser.ContactNumber;
            LoginId = applicationUser.LoginId;
        }

    }
}
