namespace BaseLibrary.Configurations.FormHandlers
{
    public class UserManagementFormHandler : FormHandlerBase
    {
        public UserManagementFormHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser? loggedInUser)
            : base(serviceFactory, loggedInUser, BaseForm.UserManagement, null, null, true)
        {

        }

        public override void ProcessFormSaveRequest(SmartFormTemplateRequest model)
        {
            BaseLibraryServiceFactory.UserService.SaveUpdate(model, LoggedInUser);
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
