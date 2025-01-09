namespace BaseLibrary.Services
{
    public interface IApplicationSettingService
    {
        ApplicationSetting GetApplicationSetting();
        void UpdateApplicationSetting(ApplicationSetting applicationSetting);
    }

    public class ApplicationSettingService : ServiceLibraryBase, IApplicationSettingService
    {
        public ApplicationSettingService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory)
        {
        }
        public ApplicationSetting GetApplicationSetting()
        {
            
                var settings = RF.ApplicationSettingRepository.GetAll();
                if (settings.Any())
                    return settings.First();
                var setting = ApplicationSetting.Create();
                RF.ApplicationSettingRepository.Add(setting);
                
                return setting;
            
        }

        public void UpdateApplicationSetting(ApplicationSetting applicationSetting)
        {
            
                var setting = GetApplicationSetting();
                setting.Update(applicationSetting);
                
        }
    }
}
