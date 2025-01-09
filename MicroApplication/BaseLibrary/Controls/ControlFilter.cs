namespace BaseLibrary.Controls
{
    public class ControlFilter
    {
        public Guid ControlId { get; set; }
        public string ControlIdentifier { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
    }
}

