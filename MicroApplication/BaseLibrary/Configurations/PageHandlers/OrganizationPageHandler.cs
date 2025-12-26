using BaseLibrary.UI.Controls;

namespace BaseLibrary.Configurations.PageHandlers
{
    public class OrganizationPageHandler : DefaultPageHandler
    {
        public OrganizationPageHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser loggedInUser) : base(serviceFactory, loggedInUser, BasePage.ManageOrganizationPage, BaseForm.ManageOrganizationForm, null)
        {

        }

        public override List<List<GridCell>> GetRows(GridRequestVM model, out int totalRows)
        {
            var result = new List<List<GridCell>>();
            if (LoggedInUser is not null)
            {
                var organizations = BaseLibraryServiceFactory.OrganizationService.GetOrganizations(LoggedInUser, model);
                foreach (var organization in organizations)
                {
                    BuildAndAddGridRow(model, organization, result);
                }
            }
            totalRows = result.Count;
            return result;
        }

        public override List<GridHeader> GetGridHeaders(SmartGridConfigurationVM vm, bool includeExcludedHeaders)
        {
            var headers = GetGridHeaders(LoggedInUser.OrganizationId, BaseForm.ManageOrganizationForm.Id, null, includeExcludedHeaders, null);
            return headers;
        }

        public override List<SmartAction> GetPageActions()
        {
            return new List<SmartAction>
            {
                new SmartAction(BaseForm.ManageOrganizationForm,FormTypes.DynamicForm, "Add","Add","Add",SmartActionFormMode.Add),
            };
        }

        public override List<UIControl> GetFilters(SmartGridConfigurationVM vm)
        {
            var formHandler = BSF.MicroAppContract.GetHandlerFactory().GetFormHandler(BaseForm.ManageOrganizationForm.Id, LoggedInUser);
            var form = formHandler.ProcessFormGenerateRequest(new SmartFormGenerateRequest { FormId = AppForm.Id, FormMode = "Add" }, ApplicationControlFactory);
            return form.UIControls.Where(c => c.IsGlobalControl == false).ToList();
        }

    }
}
