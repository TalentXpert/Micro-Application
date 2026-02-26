namespace BaseLibrary.Services
{
    public interface ILoginService
    {
        ApplicationUser GetUserAfterValidatingUserCredential(string login, string password);
        ApplicationUser GetUserAfterValidatingUserCredential(string key);
    }
    public class LoginService : ServiceLibraryBase, ILoginService
    {
        public LoginService(IBaseLibraryServiceFactory repositoryFactory, ILoggerFactory loggerFactory) : base(repositoryFactory, loggerFactory.CreateLogger<LoginService>())
        {

        }

        public ApplicationUser GetUserAfterValidatingUserCredential(string login, string password)
        {
            var applicationUser = SF.UserService.GetUserByLoginId(login);
            if (applicationUser is null && AreEqualsIgnoreCase(login, ApplicationSettingBase.WebsiteAdminLoginId))
                return GetAdminUser(login, password);
            if (applicationUser is null)
                throw new ValidationException($"Login Id – {login} doesn’t exists. Please use correct login id.");
            return GetUserAfterValidatingUserCredential(applicationUser, login, password);
        }

        public ApplicationUser GetUserAfterValidatingUserCredential(string key)
        {
            throw new NotImplementedException();
        }

        private ApplicationUser GetUserAfterValidatingUserCredential(ApplicationUser user, string loginId, string password)
        {
            if (user is null)
                throw new ValidationException($"Login Id – {loginId} doesn’t exists. Please use correct login id.");

            if (user.IsBlocked)
                throw new ValidationException($"Your account has been blocked. Please contact support team for more information. Message from blocking - {user.BlockReason}");

            if (user.Salt == null && user.IsPasswordMatching(password))
            {
                var salt = BaseLibrary.Utilities.PasswordHasher.CreateRandomSalt();
                SF.UserService.ChangePasswordToHash(user, salt);
                return user;
            }
            if (!user.IsPasswordHashMatching(password))
                throw new ValidationException("Wrong password. Please use forgot password link to reset your password.");

            //if (user.Organization != null && user.Organization.IsBlocked == true)
            //    throw new ValidationException($"Your organization has been blocked because of - {user.Organization.BlockingReason}");

            return user;
        }
        private ApplicationUser GetAdminUser(string login, string password)
        {
            var adminUser = SF.UserService.GetUserByLoginId(login);
            if (adminUser is null)
            {
                adminUser = ApplicationUser.CreateWebsiteAdmin("Admin", login, password);
                RF.UserRepository.Add(adminUser);
            }
            return adminUser;
        }
    }
}
