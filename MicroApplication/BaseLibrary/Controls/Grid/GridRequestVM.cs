namespace BaseLibrary.Controls.Grid
{
    public class GridRequestVM
    {
        public Guid PageId { get; set; }
        public string SortDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortHeaderText { get; set; }
        public List<ControlFilter> GlobalFilters { get; set; }
        public List<ControlFilter> Filters { get; set; }
        public Guid? FilterId { get; set; }
        public GridRequestVM()
        {
            Filters = new List<ControlFilter>();
            GlobalFilters = new List<ControlFilter>();
        }

        public ControlFilter? GetFilterControlValue(AppControl control)
        {
            var controlValues = Filters;
            if (controlValues.Any())
            {
                var controlValue = controlValues.FirstOrDefault(c => c.ControlIdentifier == control.ControlIdentifier);
                return controlValue;
            }
            return null;
        }
        public ControlFilter? GetFilterControlValue(string controlIdentifier)
        {
            var controlValues = Filters;
            if (controlValues.Any())
            {
                var controlValue = controlValues.FirstOrDefault(c => c.ControlIdentifier == controlIdentifier);
                return controlValue;
            }
            return null;
        }
        public Guid? GetGlobalFilterControlValue(AppControl control)
        {
            if (GlobalFilters.Any())
            {
                var controlValue = GlobalFilters.FirstOrDefault(c => c.ControlIdentifier == control.ControlIdentifier);
                return Convertor.ToGuid(controlValue?.Value);

            }
            return null;

        }

        public SmartGridConfigurationVM GetSmartGridConfigurationVM()
        {
            var vm = new SmartGridConfigurationVM();
            vm.PageId = PageId;
            if (GlobalFilters.Any())
                vm.GlobalControlValues = GlobalFilters.Select(c => new ControlValue { ControlId = c.ControlId, ControlIdentifier = c.ControlIdentifier, Values = new List<string> { c.Value } }).ToList();
            return vm;
        }
    }
}
