namespace BaseLibrary.Domain
{
    public class ApplicationSetting : Entity
    {
        public bool SeedPageConfiguration { get; set; }
        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }
        public static ApplicationSetting Create()
        {
            var setting = new ApplicationSetting 
            { 
                Id = Guid.Parse("D4AAB8BC-34D4-4688-B413-57BAF0C50FA8"),
                SeedPageConfiguration = true 
            };
            setting.SetCreatedOn();
            setting.SetUpdatedOn();
            return setting;

        }

        public  void Update(ApplicationSetting applicationSetting)
        {
            SeedPageConfiguration = applicationSetting.SeedPageConfiguration;
        }
    }
}
