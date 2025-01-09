using BaseLibrary.Controls.Grid;
using BaseLibrary.UI.Controls;

namespace BaseLibrary.Configurations.PageHandlers
{
    public class ApplicationControlPageHandler : DefaultPageHandler
    {
        public ApplicationControlPageHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser loggedInUser)
            : base(serviceFactory, loggedInUser, BasePage.ApplicationControlPage, BaseForm.ApplicationControlForm, null)
        {

        }

        public override List<UIControl> GetFilters(SmartGridConfigurationVM model)
        {
            var formHandler = BSF.MicroAppContract.GetHandlerFactory().GetFormHandler(BaseForm.ApplicationControlForm.Id, LoggedInUser);
            var form = formHandler.ProcessFormGenerateRequest(new SmartFormGenerateRequest { FormId = BaseForm.ApplicationControlForm.Id, FormMode = "Add" },  ApplicationControlFactory);
            return form.UIControls;
        }

        public override List<GridHeader> GetGridHeaders(SmartGridConfigurationVM model, bool includeExcludedHeaders)
        {
            var headers = GetGridHeaders(LoggedInUser.OrganizationId, BaseForm.ApplicationControlForm.Id,null, includeExcludedHeaders, null);
            return headers;
        }

        public override List<SmartAction> GetPageActions()
        {
            return new List<SmartAction>
            {
                new SmartAction(BaseForm.ApplicationControlForm,FormTypes.DynamicForm, "Add","Add","Add",SmartActionFormMode.Add),
            };
        }
        //public override List<string> GetIgnoreExcelHeaders()
        //{
        //    return new List<string>
        //    {
        //        BaseControl.ControlIdentifier.ControlIdentifier
        //    };
        //}

        public override List<List<GridCell>> GetRows(GridRequestVM model, out int totalPages)
        {
            var result = BaseLibraryServiceFactory.AppControlService.GetAppControlRows(model.Filters, GetGridHeaders(model.GetSmartGridConfigurationVM(),false), LoggedInUser);
            totalPages = result.Count;
            return result;
        }
    }
}
