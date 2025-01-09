
using BaseLibrary.Configurations;

namespace BaseLibrary.Configurations.FormHandlers
{

    public class OrganizationAdminFormHandler : FormHandlerBase
    {
        public OrganizationAdminFormHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser loggedInUser)
            : base(serviceFactory, loggedInUser, BaseForm.ManageOrganizationAdminForm, null, null, true)
        {

        }

        public override void ProcessFormSaveRequest(SmartFormTemplateRequest model)
        {
            var user = BaseLibraryServiceFactory.UserService.SaveUpdateOrganizationAdmin(model, LoggedInUser);
            user.IsOrgAdmin = true;
        }

        public override FormStoreBase? GetStoreObject(Guid id)
        {
            var user = BaseLibraryServiceFactory.UserService.GetUser(id);
            return user;
        }
        public override void DeleteData(Guid id)
        {
            //var asset = ServiceFactory.RepositoryFactory.AssetRepository.Get(id);
            //ServiceFactory.RepositoryFactory.AssetRepository.Remove(asset);
        }
    }
}
