
namespace BaseLibrary.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RoleController : BaseLibraryController
    {
        public RoleController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<RoleController>())
        {

        }


        [HttpGet("Gets")]
        public IActionResult Gets()
        {
            try
            {
                IsInOrganizationAdminRole("View Roles");
                var roles = BSF.RoleService.GetOrganizationRoles(LoggedInUser.GetOrganizationId());
                return Ok((roles.Select(r => new RoleVM(r))));
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody] RoleVM model)
        {
            try
            {
                IsInOrganizationAdminRole("View Roles");
                var role = BSF.RoleService.SaveUpdate(model, LoggedInUser);
                CommitTransaction();
                return Ok(new RoleVM(role));
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }


        [HttpDelete("{roleId}")]
        public IActionResult Delete(Guid roleId)
        {
            try
            {
                IsInOrganizationAdminRole("Delete Role");
                var role = BSF.RoleService.Delete(roleId, LoggedInUser.GetOrganizationId());
                CommitTransaction();
                return Ok(new RoleVM(role));
            }

            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }
    }
}