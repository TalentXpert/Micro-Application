
namespace BaseLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserRoleController : BaseLibraryController
    {
        public UserRoleController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<UserRoleController>())
        {

        }

        [HttpGet("Gets/{userId}")]
        public IActionResult Gets(Guid userId)
        {
            try
            {
                var userRoles = BSF.UserRoleService.GetUserRoles(userId);
                var permissions = BSF.MicroAppContract.GetApplicationPermission().GetPermissions();
                var allOrganisationRoles = BSF.RoleService.GetOrganizationRoles(LoggedInUser.GetOrganizationId());
                var vm = new UserRoles { UserId = userId };
                foreach ( var role in allOrganisationRoles)
                {
                    var rolePermissions = BSF.RolePermissionService.GetRolePermissions(role.Id);
                    var userRole = new UserRoleItemVM(role, rolePermissions, permissions);
                    userRole.IsSelected = userRoles.Any(r=>r.RoleId == role.Id);
                    vm.Roles.Add(userRole);
                }
                return Ok(vm);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }

        }

        

        [HttpDelete]

        public IActionResult Delete(Guid Id)
        {
            try
            {
                IsInOrganizationAdminRole("Delete Role");
                var user_role = BSF.UserRoleService.DeleteUserRole(Id,LoggedInUser.GetOrganizationId());
                CommitTransaction();
                return Ok(new UserRoleVM(user_role));
            }

            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }
    }

}
