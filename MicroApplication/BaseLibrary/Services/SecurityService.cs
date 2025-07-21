using BaseLibrary.Domain;

namespace BaseLibrary.Services
{
    public interface ISecurityService
    {
        void IsOperationAllowed(ApplicationUser loggedInUser, string permissionCode);
    }
    public class SecurityService : ServiceLibraryBase, ISecurityService
    {
        public SecurityService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<SecurityService>())
        {
        }

        public void IsOperationAllowed(ApplicationUser loggedInUser, string permissionCode)
        {
            
        }
    }
}
