namespace BaseLibrary.Controls.Forms
{
    public class SmartControlOption
    {
        public string Value { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public bool IsSelected { get; set; } = false;

        public SmartControlOption() { }
        public SmartControlOption(string label, string value)
        {
            Label= label;
            Value= value;
        }
        public SmartControlOption(string label, string value, bool isSelected)
        {
            Label = label;
            Value = value;
            IsSelected = isSelected;
        }
    }
}
