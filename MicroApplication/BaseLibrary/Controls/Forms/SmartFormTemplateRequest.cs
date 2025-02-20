namespace BaseLibrary.Controls.Forms
{
    public class SmartFormTemplateRequest
    {
        public Guid FormId { get; set; }
        public Guid? DataKey { get; set; } //row primary key
        public string FormMode { get; set; } //1-Add , 2 - Edit, 3 Copy, 4 delete
        public List<ControlValue> ControlValues { get; set; }
        public Guid? RemoveControlId { get; set; }
        public SmartFormTemplateRequest()
        {
            ControlValues = new List<ControlValue>();
            FormMode = "Add";
        }
    }
    public class FileUploadViewModel
    {
        public List<IFormFile> UploadFiles { get; set; }
        public required string FormId { get; set; }
        public required string DataKey { get; set; }
    }
}
