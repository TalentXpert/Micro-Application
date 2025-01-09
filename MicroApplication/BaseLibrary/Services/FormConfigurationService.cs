
namespace BaseLibrary.Services
{
    public interface IFormConfigurationService
    {
        List<AppFormList> GetAllCustomizableForms();
    }
    public class FormConfigurationService : ServiceLibraryBase, IFormConfigurationService
    {
        public FormConfigurationService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory)
        {

        }
        public List<AppFormList> GetAllCustomizableForms()
        {
            var forms = RF.AppFormRepository.GetAllCustomizableForms();
            return forms.Select(f => new AppFormList(f)).ToList();
        }

    }
 }
