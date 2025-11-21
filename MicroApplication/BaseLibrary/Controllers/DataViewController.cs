namespace BaseLibrary.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DataViewController : BaseLibraryController
    {
        public ILoggerFactory LoggerFactory { get; }

        public DataViewController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<ApplicationPageController>())
        {
            LoggerFactory = loggerFactory;
        }

        public DataViewController(IBaseLibraryServiceFactory baseLibraryServiceFactory, ILogger logger) : base(baseLibraryServiceFactory, logger)
        {
        }
    }
}