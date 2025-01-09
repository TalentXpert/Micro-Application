namespace BaseLibrary.Services
{
    public interface IAppFormService
    {
        AppForm GetForm(Guid formId);
        
    }
    public class AppFormService : ServiceLibraryBase, IAppFormService
    {
        public AppFormService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory)
        {
        }

        public AppForm GetForm(Guid formId)
        {
            AppForm form = RF.AppFormRepository.Get(formId);
            return form;
        }

        
        
    }
}
