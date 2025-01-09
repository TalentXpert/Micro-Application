namespace BaseLibrary.Configurations.FormHandlers
{
    public class OrganizationFormHandler : FormHandlerBase
    {
        public OrganizationFormHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser loggedInUser)
            : base(serviceFactory, loggedInUser, BaseForm.ManageOrganizationForm, null, null, true)
        {

        }

        public override void ProcessFormSaveRequest(SmartFormTemplateRequest model)
        {
            BaseLibraryServiceFactory.OrganizationService.SaveUpdateOrganization(model, LoggedInUser);
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
    }
}
