
using BaseLibrary.Configurations.Models;

namespace BaseLibrary.Configurations.FormHandlers
{
    public class UserRoleSelectFromListFormHandler : SelectFromListFormHandler
    {
        IBaseLibraryServiceFactory SF { get; }
        public UserRoleSelectFromListFormHandler(IBaseLibraryServiceFactory serviceFactory)
        {
            SF = serviceFactory;
        }

        public override SelectFromListFormInput GetInput(Guid formId, Guid entityId, ApplicationUser loggedInUser, string parentControlValue = "")
        {
            var user = SF.UserService.GetUser(entityId);
            var roles = SF.RoleService.GetOrganizationRoles(loggedInUser.OrganizationId.Value);
            var userRoles = SF.UserRoleService.GetUserRoles(entityId);
            var result = new SelectFromListFormInput($"{user.Name} Roles", formId, entityId, "User", user.Name, "Roles");
            var permissions = SF.MicroAppContract.GetApplicationPermission().GetPermissions();
            foreach (var role in roles)
            {
                var item = new SelectFromListFormInputItem(role.Name, role.Id);
                var rolePermissions = SF.RolePermissionService.GetRolePermissions(role.Id);
                var permissionList = new List<string>();
                foreach (var rolePermission in rolePermissions)
                {
                    var p = permissions.FirstOrDefault(p => p.Id == rolePermission.PermissionId);
                    if (p != null)
                        permissionList.Add(p.Name);
                }
                item.DetailLines.Add(string.Join(",", permissionList));
                item.IsSelected = userRoles.Any(r => r.RoleId == role.Id);
                result.Items.Add(item);
            }
            return result;
        }

        public override void SaveData(SelectFromListFormData data, ApplicationUser loggedInUser)
        {
            SF.UserRoleService.SaveUpdate(data.EntityId, data.SelectedItems);
        }
    }
}
