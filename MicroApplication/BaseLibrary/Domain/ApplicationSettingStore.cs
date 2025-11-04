namespace BaseLibrary.Domain
{
    public class ApplicationSettingStore: Entity
    {
        public Guid? ReferenceId { get; set; }
        public string SettingCode { get; set; } = "";
        public string SettingName { get; set; } = "";
        public string SettingValue { get; set; } = "";
        protected ApplicationSettingStore() { }
        public ApplicationSettingStore(string settingCode, string settingName, string settingValue, Guid? referenceId)
        {
            Id = IdentityGenerator.NewSequentialGuid();
            ReferenceId = referenceId;
            SettingName = settingName;
            SettingValue = settingValue;
            SettingCode= settingCode;
        }
        public void Update(string settingName, string settingValue)
        {
            SettingName = settingName;
            SettingValue = settingValue;
        }

        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }
    }
}
