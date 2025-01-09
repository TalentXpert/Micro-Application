namespace BaseLibrary.UI.Controls
{
    public class FilterControl : MicroControl
    {
        public FilterControl(AppControl appControl, List<string>? values) : base(appControl, values)
        {
        }
    }

    //public class ControlValue
    //{
    //    public Guid ControlId { get; set; } = Guid.Empty;
    //    public string ControlIdentifier { get; set; } = string.Empty;
    //    public List<string> Values { get; set; } = new List<string>(); //Date YYYY-MM-DD
    //    public ControlValue() { Values = new List<string>(); }
    //    public ControlValue(Guid controlId, string controlIdentifier, string? value)
    //    {
    //        ControlId = controlId;
    //        ControlIdentifier = controlIdentifier;
    //        if (string.IsNullOrWhiteSpace(value) == false)
    //            Values = new List<string> { value };
    //    }
    //}
}
