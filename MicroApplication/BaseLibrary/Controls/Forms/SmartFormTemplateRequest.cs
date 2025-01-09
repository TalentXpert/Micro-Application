
namespace BaseLibrary.Controls.Forms
{

    public class SmartFormTemplateRequest
    {
        public Guid FormId { get; set; }
        public Guid? DataKey { get; set; } //row primary key
        public string FormMode { get; set; } //1-Add , 2 - Edit, 3 Copy, 4 delete
        public List<ControlValue> ControlValues { get; set; }       
        public SmartFormTemplateRequest()
        {
            ControlValues = new List<ControlValue>();
            FormMode = "Add";
        }
    }
    
}
