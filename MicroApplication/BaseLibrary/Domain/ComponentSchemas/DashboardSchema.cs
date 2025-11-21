
namespace BaseLibrary.Domain.ComponentSchemas
{
    public class DashboardSchema
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }= string.Empty;
        public string Description { get; set; }= string.Empty;
        public int MinimumRowHeight { get; set; }
        public Guid MenuId { get; set; }
        public int Position { get; set; }
        public List<DashboardRow> Rows { get; set; } = new List<DashboardRow>();
        public List<string> DataSources { get; set; } = new List<string>();
        public List<Guid> FilterControlIds { get; set; } = new List<Guid>();
        public List<UIControl> Filters { get; set; } = []; // These control will be on list page at the top with a search button and read only on add or update form
    }
    
    public class DashboardRow
    {
        public int Position { get; set; }
        public int Height { get; set; }
        public List<DashboardPanel> Panels { get; set; } = new List<DashboardPanel>();
        
        public void AddPanel(DashboardPanel panel)
        {
            panel.Position = Panels.Count + 1;
            Panels.Add(panel);
        }
    }

    public class DashboardPanel
    {
        public string Title { get; set; } = string.Empty;
        public int Width { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public int Position { get; set; }
        public Guid? ContentId { get; set; }
        public DashboardPanel() { }
        public DashboardPanel(DashboardPanelContentType contentType)
        {
            ContentType = contentType.ContentType;
        }
        public DashboardPanel(DashboardPanelContentType contentType, Guid contentId, string title, int position, int width):this(contentType)
        {
            ContentId = contentId;
            Title = title;
            Position= position;
            Width = width;
        }
    }

    public class DashboardPanelContentType
    {
        public string ContentType { get; private set; }
        private DashboardPanelContentType(string contentType)
        {
            ContentType = contentType;
        }
        public static DashboardPanelContentType Graph = new DashboardPanelContentType("Graph");
        public static DashboardPanelContentType Summary = new DashboardPanelContentType("Summary");
        public static DashboardPanelContentType Grid = new DashboardPanelContentType("Grid");
        public static DashboardPanelContentType ListView = new DashboardPanelContentType("ListView");
        public static List<string> ContentTypes => new List<string> { Graph.ContentType, Summary.ContentType, Grid.ContentType, ListView.ContentType };
    }
}
