
namespace BaseLibrary.Configurations.FormHandlers
{
    public class OrganizationFormHandler : FormHandlerBase
    {
        public OrganizationFormHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser? loggedInUser)
            : base(serviceFactory, loggedInUser, BaseForm.ManageOrganizationForm, null, null, true)
        {

        }

        public override string ProcessFormSaveRequest(SmartFormTemplateRequest model)
        {
            return BaseLibraryServiceFactory.OrganizationService.SaveUpdateOrganization(model, LoggedInUser).ToString();
        }

        public override FormStoreBase? GetStoreObject(Guid id)
        {
            var user = BaseLibraryServiceFactory.RF.OrganizationRepository.Get(id);
            return user;
        }
        public override void DeleteData(Guid id)
        {
            //var asset = ServiceFactory.RepositoryFactory.AssetRepository.Get(id);
            //ServiceFactory.RepositoryFactory.AssetRepository.Remove(asset);
        }
        public override List<SmartControlOption> GetControlOptions(Guid? organizationId, Guid? parentId, string searchTerm)
        {
            return BaseLibraryServiceFactory.RF.OrganizationRepository.GetSearchResult(searchTerm);
        }
    }
}
