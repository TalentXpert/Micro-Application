using Microsoft.Extensions.Logging;

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
            if (applicationUser is null)
                throw new ValidationException("Either login or password is wrong. Please try with right login and password.");
            return GetUserAfterValidatingUserCredential(applicationUser, login, password);
        }

        public ApplicationUser GetUserAfterValidatingUserCredential(string key)
        {
            throw new NotImplementedException();
        }

        private ApplicationUser GetUserAfterValidatingUserCredential(ApplicationUser user, string loginId, string password)
        {
            if (user == null)
                throw new ValidationException("Either login or password is wrong. Please try with right login and password.");

            if (user.IsBlocked)
                throw new ValidationException($"Your account has been blocked. Please contact support team for more information. Message from blocking - {user.BlockReason}");

            if (user.Salt ==null && user.IsPasswordMatching(password))
            {
                var salt = BaseLibrary.Utilities.PasswordHasher.CreateRandomSalt();
                SF.UserService.ChangePasswordToHash(user,salt);
                return user;
            }
            if (!user.IsPasswordHashMatching(password))
                throw new ValidationException("Wrong password. Please use forgot password link to reset your password.");

            //if (user.Organization != null && user.Organization.IsBlocked == true)
            //    throw new ValidationException($"Your organization has been blocked because of - {user.Organization.BlockingReason}");

            return user;
        }
    }
}
