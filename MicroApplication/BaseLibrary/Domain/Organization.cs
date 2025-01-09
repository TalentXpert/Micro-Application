

using BaseLibrary.Configurations;

namespace BaseLibrary.Domain
{
    public class Organization : FormStoreBase
    {
        public string Name { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        protected Organization()
        {

        }
        public Organization(ApplicationUser loggedInUser) : base(loggedInUser)
        {
            SetCreatedOn();
        }

        public void Update(SmartFormTemplateRequest model)
        {
            base.UpdateData(model.ControlValues);
            Name = ControlReader.GetControlFirstValue(BaseControls.Name, model.ControlValues);
            Website = ControlReader.GetControlFirstValue(BaseControls.Website, model.ControlValues);
            SetUpdatedOn();
        }

        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            X.Validator.StringValidator.CheckForNullOrEmpty(Name, "Name", validationResults);
            X.Validator.StringValidator.CheckForMaxLength(Name, "Name",64, validationResults);
            X.Validator.StringValidator.CheckForMaxLength(Website, "Website", 64, validationResults);
            return validationResults;
        }
    }
}
