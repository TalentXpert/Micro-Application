

namespace BaseLibrary.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RolePermissionController : BaseLibraryController
    {
        public RolePermissionController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<RolePermissionController>())
        {

        }

        [HttpGet("GetOrganizationRoles")]
        public IActionResult GetOrganizationRoles()
        {
            try
            {
                IsInOrganizationAdminRole("View Roles");
                var permissions = BSF.MicroAppContract.GetApplicationPermission().GetPermissions();
                var allOrganisationRoles = BSF.RoleService.GetOrganizationRoles(LoggedInUser.GetOrganizationId());
                var vm = new List<RolePermissionListVM>();
                foreach (var role in allOrganisationRoles)
                {
                    var rolePermissions = BSF.RolePermissionService.GetRolePermissions(role.Id);
                    if(rolePermissions.Count > 0)
                    {
                        var rolePermission = new RolePermissionListVM(role, rolePermissions, permissions);
                        vm.Add(rolePermission);
                    }
                }
                return Ok(vm);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpGet("Gets/{roleId}")]
        public IActionResult Gets(Guid roleId)
        {
            try
            {
                IsInOrganizationAdminRole("View Role Permissions");
                var rolePermissions = BSF.RolePermissionService.GetRolePermissions(roleId);
                var permissions = BSF.MicroAppContract.GetApplicationPermission().GetPermissions();
                var vm = new ApplicationRolePermissions { RoleId = roleId };
                vm.Permissions = permissions.Select(p => new RolePermissionVM(rolePermissions, p)).ToList();
                return Ok(vm);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody] ApplicationRolePermissions model)
        {
            try
            {
                BSF.RolePermissionService.SaveUpdateRolePermissions(model);
                CommitTransaction();
                return Ok(true);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpDelete("Delete/{id}")]

        public IActionResult Delete(Guid id)
        {
            try
            {
                var rolepermission = BSF.RolePermissionService.Delete(id);
                CommitTransaction();
                return Ok(true);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }
    }
}