using BaseLibrary.UI.Controls;

namespace BaseLibrary.Configurations.PageHandlers
{
    public class ReportPageHandler : DefaultPageHandler
    {
        public ReportPageHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser loggedInUser) : base(serviceFactory, loggedInUser, BasePage.ReportPage, BaseForm.UserManagement, null)
        {

        }
        public override List<GridHeader> GetGridHeaders(SmartGridConfigurationVM vm, bool includeExcludedHeaders)
        {
            return base.GetGridHeaders(LoggedInUser.OrganizationId, BaseForm.UserManagement.Id, null, includeExcludedHeaders, null);
        }

        public override List<UIControl> GetFilters(SmartGridConfigurationVM vm)
        {
            var formHandler = BSF.MicroAppContract.GetHandlerFactory().GetFormHandler(BaseForm.UserManagement.Id, LoggedInUser);
            var form = formHandler.ProcessFormGenerateRequest(new SmartFormGenerateRequest { FormId = BaseForm.UserManagement.Id, FormMode = "Add" }, ApplicationControlFactory);
            return form.UIControls;
        }

        public override List<SmartAction> GetPageActions()
        {
            return new List<SmartAction>()
            {
                new SmartAction(BaseForm.UserManagement,FormTypes.DynamicForm, "Add Country","","AddCountry", SmartActionFormMode.Add)
            };
        }

        public override List<List<GridCell>> GetRows(GridRequestVM model, out int totalRows)
        {
            var result = new List<List<GridCell>>();
            var countries = BaseLibraryServiceFactory.PageDataStoreService.GetFormData(null, LoggedInUser.OrganizationId, BaseForm.UserManagement.Id, null);
            foreach (var country in countries)
            {
                BuildAndAddGridRow(model, country, result);
            }
            totalRows = result.Count;
            return result;
        }

        

    }
}
