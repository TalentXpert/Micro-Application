
namespace BaseLibrary.Domain
{
    public class ApplicationUserSettingType
    {
        public string Name { get; private set; }
        private ApplicationUserSettingType(string name)
        {
            Name = name;
        }

        public static ApplicationUserSettingType GridFilter = new ApplicationUserSettingType("GridFilter");
        public static ApplicationUserSettingType PageState = new ApplicationUserSettingType("PageState");
        public static ApplicationUserSettingType GridHeader = new ApplicationUserSettingType("GridHeader");
    }

    public class ApplicationUserSetting : Entity
    {
        public string Identifier { get; set; }
        public Guid UserId { get; set; }
        public string Setting { get; set; }
        public string SettingType { get; set; }
        protected ApplicationUserSetting() { }
        protected ApplicationUserSetting(string identifier, Guid userId, string setting, ApplicationUserSettingType settingType, Guid? id = null)
        {
            if (id.HasValue)
                Id = id.Value;
            else
                Id = IdentityGenerator.NewSequentialGuid();
            Identifier = identifier;
            UserId = userId;
            Setting = setting;
            SettingType = settingType.Name;
            SetCreatedOn();
            SetUpdatedOn();
        }

        public static ApplicationUserSetting CreateSaveGridFilter(UserGridFilter filter, ApplicationUser user)
        {
            filter.Id = IdentityGenerator.NewSequentialGuid();
            return new ApplicationUserSetting(filter.PageId.ToString(), user.Id, NewtonsoftJsonAdapter.SerializeObject(filter), ApplicationUserSettingType.GridFilter, filter.Id);
        }
        public void UpdateFilter(UserGridFilter filter)
        {
            Setting = NewtonsoftJsonAdapter.SerializeObject(filter);
        }

        public UserGridFilter GetGridSavedFilter()
        {
            return NewtonsoftJsonAdapter.DeserializeObject<UserGridFilter>(Setting);
        }

        public void UpdateGridHeader(List<UserGridHeader> headers)
        {
            Setting = NewtonsoftJsonAdapter.SerializeObject(headers);
        }
        public static ApplicationUserSetting CreateUserGridHeaders(Guid pageId, List<UserGridHeader> headers, ApplicationUser user)
        {
            return new ApplicationUserSetting(pageId.ToString(), user.Id, NewtonsoftJsonAdapter.SerializeObject(headers), ApplicationUserSettingType.GridHeader);
        }

        public List<UserGridHeader> GetUserGridHeaders()
        {
            return NewtonsoftJsonAdapter.DeserializeObject<List<UserGridHeader>>(Setting);
        }

        public static ApplicationUserSetting CreateSmartPageState(SmartPageState smartPageState, ApplicationUser user)
        {
            return new ApplicationUserSetting(smartPageState.PageId.ToString(), user.Id, NewtonsoftJsonAdapter.SerializeObject(smartPageState), ApplicationUserSettingType.PageState);
        }
        public void UpdatePageState(SmartPageState smartPageState)
        {
            Setting = NewtonsoftJsonAdapter.SerializeObject(smartPageState);
        }
        public SmartPageState GetSmartPageState()
        {
            return NewtonsoftJsonAdapter.DeserializeObject<SmartPageState>(Setting);
        }

        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }

        
    }
}
