using BaseLibrary.Controllers;


namespace MicroAppAPI.Controllers
{
    public class BaseController : BaseLibraryController
    {
        public BaseController(IServiceFactory serviceFactory, ILogger logger) : base(serviceFactory,logger)
        {
            SF = serviceFactory;
            LoginService = SF.LoginService;
        }

        protected ILoginService LoginService { get; }
        protected IServiceFactory SF { get; }
        
    }
}