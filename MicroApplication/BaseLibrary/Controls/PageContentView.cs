namespace BaseLibrary.Controls
{
    public class PageContentView
    {
        public List<PagePanelContentView> Contents { get; } = new List<PagePanelContentView>();
    }

    public class PagePanelContentView
    {
        public string Title { get; set; }
        public List<PageContentItemView> PageContentItemViews { get; } = new List<PageContentItemView>();
        public PagePanelContentView(string title)
        {
            Title = title;
        }
        public void AddPageContentItem(PageContentItemView item)
        {
            if (string.IsNullOrWhiteSpace(item.Label)) 
                return;
            PageContentItemViews.Add(item);
        }
    }
    public class PageContentItemView
    {
        public string? Label { get; set; }
        public string? Value { get; set; }
        public int Column { get; set; }
        public bool IsSingleLine { get; set; }
        public PageContentItemView(string label, string? value)
        {
            Label = label;
            Value = value;
            Column = 1;
        }
        public PageContentItemView(AppControl? control, string? value)
        {
            Label = control?.DisplayLabel;
            Value = value;
            Column = 1;
        }
    }
}

