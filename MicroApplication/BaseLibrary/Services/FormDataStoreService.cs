using BaseLibrary.Repositories;


namespace BaseLibrary.Services
{

    public interface IFormDataStoreService
    {
        void DeleteData(Guid id, ApplicationUser loggedInUser);
        List<FormDataStore> GetFormData(Guid? userId, Guid? organizationId, Guid formId, Guid? parentId);
        FormDataStore GetFormDatum(Guid id);
        string SaveFormDatum(SmartFormTemplateRequest model, Guid? parentId, ApplicationUser loggedInUser);
        List<SmartControlOption> GetSearchResult(Guid? organizationId, Guid formId, Guid? parentId, string searchTerm);
        void SaveDashboardDatum(PageDataStoreVM pageDataVM, ApplicationUser loggedInUser);
    }
    public class FormDataStoreService : ServiceLibraryBase, IFormDataStoreService
    {
        public IFormDataStoreRepository PageDataStoreRepository { get; }
        public FormDataStoreService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory)
        {
            PageDataStoreRepository = RF.PageDataStoreRepository;
        }

        public List<FormDataStore> GetFormData(Guid? userId, Guid? organizationId, Guid formId, Guid? parentId)
        {
            return PageDataStoreRepository.GetFormData(userId, organizationId, formId, parentId);
        }

        public FormDataStore GetFormDatum(Guid id)
        {
            return PageDataStoreRepository.Get(id);
        }

        public string SaveFormDatum(SmartFormTemplateRequest model, Guid? parentId, ApplicationUser loggedInUser)
        {
            FormDataStore? pageData;
            if (IsNullOrEmpty(model.DataKey))
            {
                pageData = new FormDataStore(model.FormId, loggedInUser);
                PageDataStoreRepository.Add(pageData);
            }
            else
                pageData = GetFormDatum(model.DataKey.Value);

            pageData.Update(model, parentId);
            return pageData.Id.ToString();
        }

        public void SaveDashboardDatum(PageDataStoreVM pageDataVM, ApplicationUser loggedInUser)
        {
            FormDataStore pageData;
            if (pageDataVM.Id.IsNullOrEmpty())
            {
                pageData = new FormDataStore(pageDataVM.DashboardId, loggedInUser);
                PageDataStoreRepository.Add(pageData);
            }
            else
                pageData = GetFormDatum(pageDataVM.Id.Value);
            pageData.Name = pageDataVM.Name;
            pageData.Description = pageDataVM.Description;
            pageData.ParentId = pageDataVM.ParentId;
            pageData.Update(pageDataVM.Values);
        }

        public void DeleteData(Guid id, ApplicationUser loggedInUser)
        {

            var data = PageDataStoreRepository.Get(id);
            if (data.OrganizationId != loggedInUser.OrganizationId)
                throw new ValidationException("This data belong to other organization so you can not delete it.");
            PageDataStoreRepository.Remove(data);


        }

        public List<SmartControlOption> GetSearchResult(Guid? organizationId, Guid formId, Guid? parentId, string searchTerm)
        {
            return PageDataStoreRepository.GetSearchResult(organizationId, formId, parentId, searchTerm);
        }
    }
}
