namespace BaseLibrary.UI.Controls
{
    public class MicroControl : CleanCode
    {
        public Guid ControlId { get; protected set; }
        public int Position { get; protected set; }
        public string ControlIdentifier { get; private set; }
        public string DataType { get; protected set; }
        public string ControlType { get; protected set; }
        public string DisplayLabel { get; protected set; }

        public string? Value { get; protected set; } //multiple value can be send as comma separated values. 
        public bool IsParent { get; protected set; } // is this control a parent of any other control. to raise event on value change. 
        public string? ParentControlIdentifier { get; protected set; } // parent control identifier to listen for event if parent changes value and update itself if needed. 
        public List<SmartControlOption> Options { get; set; } //options for this control lile dropdown options. 
        public string? Tooltip { get; protected set; }
        public MicroControl(Guid controlId, int position, string controlIdentifier, string dataType, string controlType, string displayLabel, string? value, bool isParent, string? parentControlIdentifier, List<SmartControlOption> options, string? tooltip)
        {
            ControlId = controlId;
            Position = position;
            ControlIdentifier = controlIdentifier;
            DataType = dataType;
            ControlType = controlType;
            DisplayLabel = displayLabel;
            Value = value;
            IsParent = isParent;
            ParentControlIdentifier = parentControlIdentifier;
            Options = options;
            Tooltip = tooltip;
        }

        public MicroControl(AppControl appControl, List<string>? values)
        {
            ControlIdentifier = appControl.ControlIdentifier;
            ControlId = appControl.Id;
            DataType = appControl.DataType;
            ControlType = appControl.ControlType;
            DisplayLabel = appControl.DisplayLabel;
            if (values != null && values.Any())
                Value = string.Join(",", values);
            Options = new List<SmartControlOption>();
            IsParent = appControl.IsParent;
            ParentControlIdentifier = appControl.ParentControlIdentifier;
        }
        public void SetPosition(int position)
        {
            Position = position;
        }
        public void SetValue(string? value)
        {
            Value = value;
        }
        public void AddOptions(List<SmartControlOption> options)
        {
            if (IsNull(options))
                options = new List<SmartControlOption>();
            Options = options;
        }

        public void AddOptions(List<string> options)
        {
            if (IsNull(options))
                options = new List<string>();
            Options = options.Select(o => new SmartControlOption(o, o)).ToList(); ;
        }

        public bool IsComplexControl()
        {
            return ControlTypes.IsComplexControl(this);
        }
    }
}
