
using BaseLibrary.Configurations;

namespace BaseLibrary.Domain
{
    /// <summary>
    /// This class store any page data that is not being saved by any other domain entity
    /// </summary>
    public class FormDataStore : FormStoreBase
    {
        public Guid FormId { get; set; }
        public Guid? ParentId { get; set; } // Asset Group Id - Laptop and Furniture, Form Layout Controller 
        public string? Name { get; set; }
        public string? Description { get; set; }

        protected FormDataStore() { }
        public FormDataStore(Guid formId, ApplicationUser loggedInUser) : base(loggedInUser)
        {
            FormId = formId;
            SetCreatedOn();
        }
       
        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }

        public virtual void Update(SmartFormTemplateRequest model, Guid? parentId)
        {
            UpdateData(model.ControlValues);
            ParentId=parentId;
            Name = ControlReader.GetControlFirstValue(BaseControls.Name, model.ControlValues);
            Description = ControlReader.GetControlFirstValue(BaseControls.Description, model.ControlValues);
            SetUpdatedOn();
        }
        public virtual void Update(Dictionary<string,string?> values)
        {
            UpdateData(values);
        }
    }

    public class PageDataStoreVM
    {
        public Guid? Id { get; set; }
        public Guid DashboardId { get; set; }
        public Guid? ParentId { get; set; } 
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Dictionary<string, string?> Values { get; set; }
    }
}
