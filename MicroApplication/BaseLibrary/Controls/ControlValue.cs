namespace BaseLibrary.Controls
{
    public class ControlValue
    {
        public Guid ControlId { get; set; }= Guid.Empty;
        public string ControlIdentifier { get; set; } = string.Empty;
        public List<string> Values { get; set; } = new List<string>(); //Date YYYY-MM-DD
        public string JsonValues { get; set; } //this is for complex control where value is being sent in json. 
        public ControlValue() { Values = new List<string>(); }
        public ControlValue(Guid controlId, string controlIdentifier, string? value) 
        {
            ControlId = controlId;
            ControlIdentifier = controlIdentifier;
            if(string.IsNullOrWhiteSpace(value)==false)
                Values = new List<string> { value };
        }
        public ControlValue(AppControl appControl) : this()
        {
            ControlId = appControl.Id;
            ControlIdentifier = appControl.ControlIdentifier;
        }
        public ControlValue(AppControl appControl, string value) : this(appControl.Id, appControl.ControlIdentifier, value)
        {

        }
        public bool IsMatching(AppControl appControl)
        {
            return ControlId == appControl.Id || ControlIdentifier == appControl.ControlIdentifier;
        }
        public bool IsMatching(Guid controlId)
        {
            return ControlId == controlId ;
        }
        public bool IsMatching(string controlIdentifier)
        {
            return ControlIdentifier == controlIdentifier;
        }
        public string? GetFirstValue()
        {
            return Values.FirstOrDefault();
        }
    }
}

