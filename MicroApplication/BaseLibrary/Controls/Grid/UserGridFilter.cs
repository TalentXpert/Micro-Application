namespace BaseLibrary.Controls.Grid
{
    /// <summary>
    /// User Saved Filter for a Page
    /// </summary>
    public class UserGridFilter
    {
        public Guid Id { get; set; }
        public Guid PageId { get; set; }
        public string FilterName { get; set; } = string.Empty;
        public List<ControlFilter> Filters { get; set; }
        public UserGridFilter()
        {
            Filters = new List<ControlFilter>();
        }
    }
}
