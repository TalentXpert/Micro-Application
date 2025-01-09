namespace BaseLibrary.Controls
{
    public class SmartPageState
    {
        public Guid PageId { get; set; }
        public List<ControlValue> GlobalControls { get; set; }
        public Guid? CurrentGridFilterId { get; set; }
        public int PageSize { get; set; }

        public SmartPageState(Guid id)
        {
            PageId = id;
            GlobalControls = new List<ControlValue>();
            PageSize = 100;
        }
        public string? GetGlobalControlValue(AppControl? appControl)
        {
            if (appControl != null && GlobalControls != null && GlobalControls.Any())
            {
                var globalControlValue = GlobalControls.FirstOrDefault(c => c.ControlId == appControl.Id || c.ControlIdentifier == appControl.ControlIdentifier)?.GetFirstValue();
                return globalControlValue;
            }
            return null;
        }
    }
}
