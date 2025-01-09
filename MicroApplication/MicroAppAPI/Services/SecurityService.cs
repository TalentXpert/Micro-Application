
namespace MicroAppAPI.Services
{
    public class SecurityService : ServiceBase, ISecurityService
    {
        public SecurityService(ServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory)
        {
        }

        public void IsOperationAllowed(ApplicationUser loggedInUser, string permissionCode)
        {
            throw new NotImplementedException();
        }
    }


}
