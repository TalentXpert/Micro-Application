using BaseLibrary.Domain;
using BaseLibrary.UI.Controls;

namespace BaseLibrary.Controls.Grid
{
    public class SmartGrid
    {
        public Guid PageId { get; set; }
        /// <summary>
        /// This list contains user saved filters with their values
        /// </summary>
        public List<UserGridFilter> UserGridFilters { get; set; }
        /// <summary>
        /// Filter control those will display when user will click on filter icon.
        /// </summary>
        public List<UIControl> Filters { get; set; }
        public SmartGrid(Guid pageId, List<UserGridFilter> userGridFilters, List<UIControl> filters)
        {
            PageId = pageId;
            UserGridFilters = userGridFilters;
            Filters = filters;
        }
    }

    public class SmartGridConfigurationVM
    {
        public Guid PageId { get; set; }
        public List<ControlValue> GlobalControlValues { get; set; }
        public SmartGridConfigurationVM()
        {
            GlobalControlValues = new List<ControlValue>();
        }
        public Guid? GetControlValue(AppControl appControl)
        {
            if (GlobalControlValues.Any())
            {
                var globalControlValue = GlobalControlValues.FirstOrDefault(c => c.ControlId == appControl.Id || c.ControlIdentifier == appControl.ControlIdentifier)?.GetFirstValue();
                if (string.IsNullOrWhiteSpace(globalControlValue) == false)
                {
                    if (Guid.TryParse(globalControlValue, out Guid id))
                        return id;
                }
            }
            return null;
        }
    }
    public class GridCell
    {
        /// <summary>
        /// Text value visible to user
        /// </summary>
        public string T { get; set; } = string.Empty;
        /// <summary>
        /// Style to be applied to grid cell
        /// </summary>
        public string S { get; set; } = string.Empty;
        /// <summary>
        /// Value to be used for sorting for non string columns
        /// </summary>
        public int V { get; set; } = 0;
        public List<SmartAction> Actions { get; set; }=new List<SmartAction>();
    }
}
