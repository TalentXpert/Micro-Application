using BaseLibrary.UI.Controls;

namespace BaseLibrary.Configurations.PageHandlers
{
    public class OrganizationAdminPageHandler : DefaultPageHandler
    {
        public OrganizationAdminPageHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser loggedInUser) : base(serviceFactory, loggedInUser, BasePage.ManageOrganizationAdminPage, BaseForm.ManageOrganizationAdminForm, null)
        {

        }

        public override List<List<GridCell>> GetRows(GridRequestVM model, out int totalRows)
        {
            var result = new List<List<GridCell>>();
            if (LoggedInUser != null && LoggedInUser.OrganizationId.HasValue)
            {
                var users = BaseLibraryServiceFactory.RF.UserRepository.GetOrganizationAdmins(model);
                foreach (var user in users)
                {
                    if (user.IsOrgAdmin) 
                        BuildAndAddGridRow(model, user, result);
                }
            }
            totalRows = result.Count;
            return result;
        }

        public override List<GridHeader> GetGridHeaders(SmartGridConfigurationVM vm, bool includeExcludedHeaders)
        {
            var headers = GetGridHeaders(LoggedInUser.OrganizationId, BaseForm.ManageOrganizationAdminForm.Id, null, includeExcludedHeaders, null);
            return headers;
        }

        public override List<SmartAction> GetPageActions()
        {
            return new List<SmartAction>
            {
                new SmartAction(BaseForm.ManageOrganizationAdminForm,FormTypes.DynamicForm, "Add","Add","Add",SmartActionFormMode.Add),
                //new SmartAction(BaseForm.UserRoleForm,FormTypes.SelectFromListForm, "Manage Roles","ManageRoles","ManageRoles",SmartActionFormMode.Add),
            };
        }

        public override List<UIControl> GetFilters(SmartGridConfigurationVM vm)
        {
            var formHandler = BSF.MicroAppContract.GetHandlerFactory().GetFormHandler(BaseForm.ManageOrganizationAdminForm.Id, LoggedInUser);
            var form = formHandler.ProcessFormGenerateRequest(new SmartFormGenerateRequest { FormId = AppForm.Id, FormMode = "Add" }, ApplicationControlFactory);
            return form.UIControls.Where(c => c.IsGlobalControl == false).ToList();
        }

    }
}
