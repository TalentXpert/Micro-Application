namespace BaseLibrary.Services
{
    public interface IOrganizationService
    {
        List<Organization> GetOrganizations(ApplicationUser loggedInUser, GridRequestVM model);
        Guid SaveUpdateOrganization(SmartFormTemplateRequest model, ApplicationUser loggedInUser);
    }
    public class OrganizationService : ServiceLibraryBase, IOrganizationService
    {
        public OrganizationService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<OrganizationService>())
        {
        }

        public List<Organization> GetOrganizations(ApplicationUser loggedInUser, GridRequestVM model)
        {
            return RF.OrganizationRepository.GetOrganizations(loggedInUser, model);
        }

        public Guid SaveUpdateOrganization(SmartFormTemplateRequest model, ApplicationUser loggedInUser)
        {
            Organization organization;

            if (model.DataKey.IsNullOrEmpty())
            {
                organization = new Organization(loggedInUser);
                organization.Update(model);
                RF.OrganizationRepository.Add(organization);
            }
            else
            {
                organization = RF.OrganizationRepository.Get(model.DataKey.Value);
                organization.Update(model);
            }
            return organization.Id;
        }
    }
}
