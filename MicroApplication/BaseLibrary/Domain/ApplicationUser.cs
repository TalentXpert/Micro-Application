
namespace BaseLibrary.Domain
{
    public class ApplicationUser : FormStoreBase
    {
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public string ContactNumber { get; protected set; }
        public string LoginId { get; protected set; }
        public string Password { get; protected set; }
        public int? Salt { get; protected set; }
        public bool IsBlocked { get; protected set; }
        public bool IsOrgAdmin { get; protected set; }
        public string? BlockReason { get; protected set; }
        public DateTime? LastLogin { get; protected set; }
        public Guid SessionId { get; protected set; }
        public string? PasswordResetCode { get; protected set; }
        public DateTime? PasswordResetCodeTimeStamp { get; protected set; }
        public string? Role { get; protected set; }
        public Guid? DefaultStudyId { get; protected set; }
        public string? TimeZone { get; protected set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        protected ApplicationUser() { }

        protected ApplicationUser(ApplicationUser loggedInUser) : base(loggedInUser)
        {
            SessionId= Guid.NewGuid();
            LastLogin = DateTime.UtcNow;
            SetCreatedOn();
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public static ApplicationUser CreateWithId(Guid id, string name, string email, string contactNumber)
        {
            var user = new ApplicationUser
            {
                Id = id,
                Name = name,
                Email = email,
                LoginId = email,
                ContactNumber = contactNumber,
                LastLogin=DateTime.UtcNow
            };
            user.SetDefaultPassword();
            return user;
        }

        public string SetDefaultPassword()
        {
            var password = "12345";
            if (ApplicationSettingBase.IsEmailEnabled)
                password = GenerateRandomPassword(12);
            SetPassword(password);
            return password;
        }

        public static ApplicationUser CreateUserWithLoginId(ApplicationUser loggedInUser, string name, string email, string contactNumber,string loginId)
        {
            var user = new ApplicationUser(loggedInUser)
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                Name = name,
                Email = email,
                ContactNumber = contactNumber,
                LoginId = loginId
            };
            user.SetDefaultPassword();
            return user;
        }
        public static ApplicationUser CreateUser(ApplicationUser loggedInUser)
        {
            var user = new ApplicationUser(loggedInUser)
            {
                Id = IdentityGenerator.NewSequentialGuid(),
            };
            user.SetDefaultPassword();
            return user;
        }

        public void Update(SmartFormTemplateRequest model)
        {
            base.UpdateData(model.ControlValues);
            Name = ControlReader.GetControlFirstValue(BaseControls.Name, model.ControlValues) ?? "";
            Email = ControlReader.GetControlFirstValue(BaseControls.Email, model.ControlValues) ?? ""; //GetControlValue(model.ControlValues, "Email"); 
            ContactNumber = ControlReader.GetControlFirstValue(BaseControls.ContactNumber, model.ControlValues) ?? ""; ;
            LoginId = ControlReader.GetControlFirstValue(BaseControls.LoginId, model.ControlValues) ?? "";
            LastLogin = DateTime.UtcNow;
            SetUpdatedOn();
        }
        public void SetDefaultStudyId(Guid studyId)
        {
            DefaultStudyId = studyId;
        }
        public bool IsPasswordHashMatching(string password)
        {
            if (Salt.HasValue)
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

        public bool IsPasswordResetCodeExpired()
        {
            if (PasswordResetCodeTimeStamp is null)
                return true;
            double totalMinutes = DateTime.UtcNow.Subtract(PasswordResetCodeTimeStamp.Value).TotalMinutes;
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
        public void MakeAdmin()
        {
            IsOrgAdmin = true;
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
            if (TimeZone is not null)
                return TimeZone.ToString();
            if (string.IsNullOrWhiteSpace(ApplicationSettingBase.DefaultTimeZone) == false)
                return ApplicationSettingBase.DefaultTimeZone;
            return TimeZoneInfo.Local.StandardName;
        }
        private static string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-+=<>?";
            int defaultLength = length; // Default length for the password

            byte[] randomBytes = new byte[defaultLength];
            System.Security.Cryptography.RandomNumberGenerator.Fill(randomBytes);

            // Convert random bytes to characters from validChars
            StringBuilder passwordBuilder = new StringBuilder(defaultLength);
            for (int i = 0; i < defaultLength; i++)
            {
                int index = randomBytes[i] % validChars.Length;
                passwordBuilder.Append(validChars[index]);
            }
            return passwordBuilder.ToString();
        }

        public void SetSessionId(Guid sessionId)
        {
            SessionId= sessionId;
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
