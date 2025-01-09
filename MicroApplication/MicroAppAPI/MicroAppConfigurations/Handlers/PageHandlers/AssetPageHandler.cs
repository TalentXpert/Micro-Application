using BaseLibrary.Configurations.PageHandlers;
using BaseLibrary.Services;
using MicroAppAPI.Services.Interfaces;

namespace MicroAppAPI.Configurations
{

    public class AssetPageHandler : DefaultPageHandler
    {
        IServiceFactory SF;
        public AssetPageHandler(IServiceFactory serviceFactory, ApplicationUser loggedInUser) : base(serviceFactory, loggedInUser, ApplicationPage.AssetPage, ApplicationForm.Asset,null)
        {
            SF=serviceFactory;
        }

        public override List<GridHeader> GetGridHeaders(SmartGridConfigurationVM vm, bool includeExcludedHeaders)
        {
            var global = vm.GetControlValue(ApplicationControl.AssetSubgroup);
            var headers = GetGridHeaders(LoggedInUser.OrganizationId, AppForm.Id, global,includeExcludedHeaders, null);
            return headers;
        }

        public override List<SmartAction> GetPageActions()
        {
            return new List<SmartAction>
            {
                new SmartAction(ApplicationForm.Asset,FormTypes.DynamicForm, "Add","Add","Add",SmartActionFormMode.Add),
            };
        }

        public override List<List<GridCell>> GetRows(GridRequestVM model, out int totalRows)
        {
            var result = new List<List<GridCell>>();
            Guid? subgroupId = model.GetGlobalFilterControlValue(ApplicationControl.AssetSubgroup);
            if (subgroupId == null)
                throw new ValidationException("Asset subgroup id is missing. Please send subgroup id.");

            var assets = SF.RepositoryFactory.AssetRepository.GetAssets(LoggedInUser.GetOrganizationId(), subgroupId.Value, model);
            foreach (var asset in assets)
            {
                BuildAndAddGridRow(model, asset, result);
            }
            totalRows = result.Count;
            return result;
        }
    }
    
}

