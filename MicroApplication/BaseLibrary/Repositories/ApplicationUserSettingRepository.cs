namespace BaseLibrary.Repositories
{
    public interface IUserSettingRepository : IRepository<ApplicationUserSetting>
    {
        List<UserGridFilter> GetGridSavedFilters(Guid userId, string Identifier);
        List<SmartPageState> GetSmartPageStates(Guid userId, string Identifier);
        List<UserGridHeader> GetUserGridHeaders(Guid userId, string Identifier);
        List<ApplicationUserSetting> GetApplicationUserSetting(Guid userId, string Identifier, ApplicationUserSettingType applicationUserSettingType);
    }
    public class UserSettingRepository : Repository<ApplicationUserSetting>, IUserSettingRepository
    {
        IBaseDatabase unitOfWork;

        public UserSettingRepository(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<UserSettingRepository>())
        {
            this.unitOfWork = unitOfWork;
        }

        public List<UserGridFilter> GetGridSavedFilters(Guid userId, string Identifier)
        {
            var filters = this.unitOfWork.UserSettings.Where(x => x.UserId == userId && x.SettingType == ApplicationUserSettingType.GridFilter.Name && x.Identifier == Identifier).Select(x => x.GetGridSavedFilter()).ToList();
            return filters;
        }
        public List<SmartPageState> GetSmartPageStates(Guid userId, string Identifier)
        {
            var filters = this.unitOfWork.UserSettings.Where(x => x.UserId == userId && x.SettingType == ApplicationUserSettingType.PageState.Name && x.Identifier == Identifier).Select(x => x.GetSmartPageState()).ToList();
            return filters;
        }
        public List<UserGridHeader> GetUserGridHeaders(Guid userId, string Identifier)
        {
            List<UserGridHeader> headers = new List<UserGridHeader>();
            var userHeaders = this.unitOfWork.UserSettings.FirstOrDefault(x => x.UserId == userId && x.SettingType == ApplicationUserSettingType.GridHeader.Name && x.Identifier == Identifier);
            if (IsNotNull(userHeaders))
                return userHeaders.GetUserGridHeaders();
            return headers;
        }
        public List<ApplicationUserSetting> GetApplicationUserSetting(Guid userId, string Identifier, ApplicationUserSettingType applicationUserSettingType)
        {
            var settings = this.unitOfWork.UserSettings.Where(x => x.UserId == userId && x.SettingType == applicationUserSettingType.Name && x.Identifier == Identifier).ToList();
            return settings;
        }
    }
}
