
using BaseLibrary.Configurations;

namespace BaseLibrary.Configurations.FormHandlers
{
    public class RolePermissionFormHandler : FormHandlerBase
    {
        public RolePermissionFormHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser loggedInUser)
            : base(serviceFactory, loggedInUser, BaseForm.RolePermission, null, null, true)
        {

        }
        public override void ProcessFormSaveRequest(SmartFormTemplateRequest model)
        {
            var roleId = model.DataKey;
            if (IsNullOrEmpty(model.DataKey))
                roleId = Convertor.ToGuid(model.ControlValues.GetControlFirstValue(BaseControl.Role));
            if (roleId == null)
                throw new ValidationException("Role Id can not be null.");

            var permissions = ControlReader.GetControlValues(BaseControl.PermissionSelection, model.ControlValues).Select(c => Convertor.ToGuid(c)).ToList();

            BaseLibraryServiceFactory.RolePermissionService.SaveUpdateRolePermissions(roleId.Value, permissions);
        }

        protected override List<ControlValue> GetFormControlValues(SmartFormGenerateRequest model)
        {
            var id = model.DataKey;
            UserRole? userRole = null;
            if (IsNotNullOrEmpty(id))
                userRole = BaseLibraryServiceFactory.RF.UserRoleRepository.Get(id.Value);

            var controlValues = new List<ControlValue>();
            if (IsNotNull(userRole))
            {
                controlValues.Add(new ControlValue(BaseControl.Role, model.DataKey?.ToString()));
                var cv = new ControlValue (BaseControl.PermissionSelection);
                cv.Values = BaseLibraryServiceFactory.RolePermissionService.GetRolePermissions(id.Value).Select(r => r.PermissionId.ToString()).ToList();
                controlValues.Add(cv);
            }
            return controlValues;
        }
    }
}
