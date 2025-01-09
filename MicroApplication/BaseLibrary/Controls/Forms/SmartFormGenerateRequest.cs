using BaseLibrary.Domain;

namespace BaseLibrary.Controls.Forms
{
    public class SmartFormGenerateRequest 
    {
        public Guid FormId { get; set; } //programing use only
        public string FormMode { get; set; } //1-Add , 2 - Edit, 3 Copy, 4 View
        public Guid? DataKey { get; set; }
        public List<ControlValue> GlobalControls { get; set; }
        public SmartFormGenerateRequest()
        {
            GlobalControls = new List<ControlValue>();
            FormMode = "Add";
        }
    }
}
