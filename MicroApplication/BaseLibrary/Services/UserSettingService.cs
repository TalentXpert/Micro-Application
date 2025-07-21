using BaseLibrary.Repositories;
using static BaseLibrary.Services.UserSettingService;

namespace BaseLibrary.Services
{
    public interface IUserSettingService
    {
        void SaveSmartPageState(SmartPageState smartPageState, ApplicationUser loggedInUser);
        SmartPageState GetSmartPageState(Guid pageId, ApplicationUser loggedInUser);
        void UpdateSmartPageState(Guid pageId, ApplicationUser loggedInUser, Guid? currentGridFilterId, int pageSize);

        List<UserGridFilter> GetFilters(Guid userId, Guid pageId);
        UserGridFilter SaveFilter(UserGridFilter model, ApplicationUser loggedInUser);
        UserGridFilter? GetFilter(Guid filterId);
        void DeleteFilter(Guid filterId);

        void SaveGridHeaders(Guid pageId, List<UserGridHeader> headers, ApplicationUser loggedInUser);
        List<UserGridHeader> GetUserConfiguredGridHeaders(Guid userId, Guid pageId);
    }

    public class UserSettingService : ServiceLibraryBase, IUserSettingService
    {
        public IUserSettingRepository UserSettingRepository { get; }
        public UserSettingService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<UserSettingService>())
        {
            UserSettingRepository = RF.UserSettingRepository;
        }

        public void SaveSmartPageState(SmartPageState smartPageState, ApplicationUser loggedInUser)
        {

            ApplicationUserSetting? pageState = UserSettingRepository.GetApplicationUserSetting(loggedInUser.Id, smartPageState.PageId.ToString(), ApplicationUserSettingType.PageState).FirstOrDefault();
            if (IsNull(pageState))
            {
                var pageStateSetting = ApplicationUserSetting.CreateSmartPageState(smartPageState, loggedInUser);
                UserSettingRepository.Add(pageStateSetting);
            }
            else
                pageState.UpdatePageState(smartPageState);


        }
        public SmartPageState GetSmartPageState(Guid pageId, ApplicationUser loggedInUser)
        {

            var state = UserSettingRepository.GetSmartPageStates(loggedInUser.Id, pageId.ToString()).FirstOrDefault();
            if (IsNull(state))
                state = new SmartPageState(pageId);
            return state;
        }
        public void UpdateSmartPageState(Guid pageId, ApplicationUser loggedInUser, Guid? currentGridFilterId, int pageSize)
        {
            var pageState = GetSmartPageState(pageId, loggedInUser);
            pageState.CurrentGridFilterId = currentGridFilterId;
            pageState.PageSize = pageSize;
            SaveSmartPageState(pageState, loggedInUser);
        }

        #region Grid Filter
        public UserGridFilter SaveFilter(UserGridFilter model, ApplicationUser loggedInUser)
        {

            ApplicationUserSetting filter = null;
            if (IsNotNullOrEmpty(model.Id))
                filter = UserSettingRepository.Get(model.Id);

            if (IsNull(filter))
            {
                filter = ApplicationUserSetting.CreateSaveGridFilter(model, loggedInUser);
                UserSettingRepository.Add(filter);
            }
            else
            {
                filter.UpdateFilter(model);
            }

            model.Id = filter.Id;
            return model;

        }

        public UserGridFilter? GetFilter(Guid filterId)
        {
            var filter = UserSettingRepository.Get(filterId);
            if (IsNull(filter))
                return null;
            return NewtonsoftJsonAdapter.DeserializeObject<UserGridFilter>(filter.Setting);
        }

        public void DeleteFilter(Guid filterId)
        {
            var filter = UserSettingRepository.Get(filterId);
            if (filter == null)
                throw new ValidationException($"No filter found with id-{filterId}");
            UserSettingRepository.Remove(filter);

        }
        public List<UserGridFilter> GetFilters(Guid userId, Guid pageId)
        {
            var savedFilters = UserSettingRepository.GetGridSavedFilters(userId, pageId.ToString());
            return savedFilters;
        }

        #endregion
        #region GridHeaders
        public void SaveGridHeaders(Guid pageId, List<UserGridHeader> headers, ApplicationUser loggedInUser)
        {

            ApplicationUserSetting? header = UserSettingRepository.GetApplicationUserSetting(loggedInUser.Id, pageId.ToString(), ApplicationUserSettingType.GridHeader).FirstOrDefault();

            if (IsNull(header))
            {
                header = ApplicationUserSetting.CreateUserGridHeaders(pageId, headers, loggedInUser);
                UserSettingRepository.Add(header);
            }
            else
            {
                header.UpdateGridHeader(headers);
            }


        }

        public List<UserGridHeader> GetUserConfiguredGridHeaders(Guid userId, Guid pageId)
        {
            var savedFilters = UserSettingRepository.GetUserGridHeaders(userId, pageId.ToString());
            return savedFilters;
        }
        #endregion
    }
}
