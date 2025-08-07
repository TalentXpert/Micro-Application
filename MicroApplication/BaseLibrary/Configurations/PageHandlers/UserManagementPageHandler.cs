using BaseLibrary.UI.Controls;

namespace BaseLibrary.Configurations.PageHandlers
{
    public class UserManagementPageHandler : DefaultPageHandler
    {
        public UserManagementPageHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser loggedInUser) : base(serviceFactory, loggedInUser, BasePage.UserManagementPage, BaseForm.UserManagement, null)
        {

        }

        public override List<List<GridCell>> GetRows(GridRequestVM model, out int totalRows)
        {
            var result = new List<List<GridCell>>();
            if (LoggedInUser != null && LoggedInUser.OrganizationId.HasValue)
            {
                var users = BaseLibraryServiceFactory.UserService.GetUsers(LoggedInUser.OrganizationId.Value, model);
                foreach (var user in users)
                {
                    BuildAndAddGridRow(model, user, result);
                }
            }
            totalRows = result.Count;
            return result;
        }

        public override List<GridHeader> GetGridHeaders(SmartGridConfigurationVM vm, bool includeExcludedHeaders)
        {
            var headers = GetGridHeaders(LoggedInUser.OrganizationId, BaseForm.UserManagement.Id, null, includeExcludedHeaders, null);
            return headers;
        }

        public override List<SmartAction> GetPageActions()
        {
            return new List<SmartAction>
            {
                new SmartAction(BaseForm.UserManagement,FormTypes.DynamicForm, "Add","Add","Add",SmartActionFormMode.Add),
               
            };
        }

        public override List<UIControl> GetFilters(SmartGridConfigurationVM vm)
        {
            var formHandler = BSF.MicroAppContract.GetHandlerFactory().GetFormHandler(BaseForm.UserManagement.Id, LoggedInUser);
            var form = formHandler.ProcessFormGenerateRequest(new SmartFormGenerateRequest { FormId = AppForm.Id, FormMode = "Add" }, ApplicationControlFactory);
            return form.UIControls.Where(c => c.IsGlobalControl == false).ToList();
        }
        protected override List<SmartAction> PrepareColumnActions(AppForm form)
        {
            var actions = new List<SmartAction>
            {
                new SmartAction(form,FormTypes.DynamicForm, "View","View","View", SmartActionFormMode.View),
                new SmartAction(form,FormTypes.DynamicForm,"Edit","Edit","Edit", SmartActionFormMode.Edit),
                new SmartAction(BaseForm.UserRoleForm,FormTypes.SelectFromListForm, "Manage Roles","ManageRoles","ManageRoles",SmartActionFormMode.Add),
            };
            return actions;
        }
    }
}
