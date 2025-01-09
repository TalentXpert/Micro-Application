namespace BaseLibrary.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PermissionController : BaseLibraryController
    {
        public PermissionController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<PermissionController>())
        {
        }
        
        [HttpGet("Gets")]
        public IActionResult Gets()
        {
            try
            {
                var permissions = BSF.MicroAppContract.GetApplicationPermission().GetPermissions();
                return Ok(permissions.Select(p => new PermissionVM(p)));
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }
        
    }

    

}
