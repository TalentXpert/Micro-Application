
using System.Data;
using BaseLibrary.UI.Controls;

namespace BaseLibrary.Configurations.PageHandlers
{
    public class DefaultPageHandler : PageHandlerBase
    {
        public IBaseLibraryServiceFactory BSF { get; }
        public AppControl? ParentControl { get; }
        public DefaultPageHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser loggedInUser, AppPage appPage, AppForm appForm, AppControl? parentControl, bool hasServerFilter = false, string defaultSortDirection = "asc", string defaultSortHeader = "id") : base(serviceFactory, loggedInUser, appPage, appForm, hasServerFilter, defaultSortDirection, defaultSortHeader)
        {
            BSF = serviceFactory;
            ParentControl = parentControl;
        }

        public override List<SmartAction> GetPageActions()
        {
            return new List<SmartAction>()
            {
                new SmartAction(AppForm,FormTypes.DynamicForm, "Add","","Add", SmartActionFormMode.Add)
            };
        }

        public override List<GridHeader> GetGridHeaders(SmartGridConfigurationVM vm, bool includeExcludedHeaders)
        {
            Guid? globalControlValue = null;
            if (ParentControl is not null)
                globalControlValue = vm.GetControlValue(ParentControl);
            var headers = GetGridHeaders(LoggedInUser.OrganizationId, AppForm.Id, globalControlValue, includeExcludedHeaders, null);
            return headers;
        }

        public override List<UIControl> GetFilters(SmartGridConfigurationVM vm)
        {
            var formHandler = BSF.MicroAppContract.GetHandlerFactory().GetFormHandler(AppForm.Id, LoggedInUser);
            var form = formHandler.ProcessFormGenerateRequest(new SmartFormGenerateRequest { FormId = AppForm.Id, FormMode = "Add", GlobalControls = vm.GlobalControlValues }, ApplicationControlFactory);
            return form.UIControls.Where(c => c.IsGlobalControl == false).ToList();
        }

        public override List<List<GridCell>> GetRows(GridRequestVM model, out int totalRows)
        {
            var result = new List<List<GridCell>>();
            Guid? parentId = null;
            if (ParentControl is not null)
                parentId = model.GetGlobalFilterControlValue(ParentControl);
            var countries = BaseLibraryServiceFactory.PageDataStoreService.GetFormData(null, LoggedInUser.OrganizationId, AppForm.Id, parentId);
            foreach (var country in countries)
            {
                BuildAndAddGridRow(model, country, result);
            }
            totalRows = result.Count;
            return result;
        }

        protected void BuildAndAddGridRow(GridRequestVM model, FormStoreBase rowData, List<List<GridCell>> rows)
        {
            var cells = new List<GridCell>();
            if (CheckIfFiltered(model, rowData))
                return;

            foreach (var h in Grid.Headers)
            {
                GridCell cell;
                if (h.HeaderIdentifier == "Id")
                {
                    cell = new GridCell { T = rowData.Id.ToString() };
                    cells.Add(cell);
                    continue;
                }

                var data = rowData.GetValue(h.HeaderIdentifier)?.ToString();
                if (Guid.TryParse(data, out var id))
                {
                    data = ApplicationControlFactory.GetTextFromId(h.HeaderIdentifier, id);
                }
                else if (Guid.TryParse(data, out var number))
                {
                    data = ApplicationControlFactory.GetTextFromId(h.HeaderIdentifier, id);
                }
                cell = new GridCell { T = data };
                cells.Add(cell);
            }
            rows.Add(cells);
        }
        protected void BuildAndAddGridRow(GridRequestVM model, FormStoreBase rowData, List<List<GridCell>> rows, Func<string?,string, string> convertToString)
        {
            var cells = new List<GridCell>();
            if (CheckIfFiltered(model, rowData))
                return;

            foreach (var h in Grid.Headers)
            {
                GridCell cell;
                if (h.HeaderIdentifier == "Id")
                {
                    cell = new GridCell { T = rowData.Id.ToString() };
                    cells.Add(cell);
                    continue;
                }

                var data = rowData.GetValue(h.HeaderIdentifier)?.ToString();
                data = convertToString(data, h.HeaderIdentifier);
                cell = new GridCell { T = data };
                cells.Add(cell);
            }
            rows.Add(cells);
        }

        public override List<string> GetIgnoreExcelHeaders()
        {
            return new List<string>();
        }

        public override List<List<GridCell>> GetRow(Guid datakey)
        {
            return new List<List<GridCell>>();
        }
    }
}
