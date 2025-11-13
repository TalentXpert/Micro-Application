namespace BaseLibrary.Configurations.FormHandlers
{
    public class UserManagementFormHandler : FormHandlerBase
    {
        public UserManagementFormHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser? loggedInUser)
            : base(serviceFactory, loggedInUser, BaseForm.UserManagement, null, null, true)
        {

        }

        public override string ProcessFormSaveRequest(SmartFormTemplateRequest model)
        {
            return BaseLibraryServiceFactory.UserService.SaveUpdate(model, LoggedInUser).Id.ToString();
        }

        public override FormStoreBase? GetStoreObject(Guid id)
        {
            return BaseLibraryServiceFactory.UserService.GetUser(id);
        }
        public override void DeleteData(Guid id)
        {
            //var asset = ServiceFactory.RepositoryFactory.AssetRepository.Get(id);
            //ServiceFactory.RepositoryFactory.AssetRepository.Remove(asset);
        }
    }
}
