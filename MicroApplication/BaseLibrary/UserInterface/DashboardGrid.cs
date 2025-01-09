namespace BaseLibrary.UserInterface
{
    public class DashboardGrid
    {
        public string Title { get; private set; }
        /// <summary>
        /// Will be used to display grid headers and filters
        /// </summary>
        public List<DashboardGridHeader> Headers { get; set; }

        /// <summary>
        /// Data rows to be displayed to user
        /// </summary>
        public List<List<GridCell>> Rows { get; set; }

        public DashboardGrid(string title)
        {
            Title= title;
            Rows = new List<List<GridCell>>();
            Headers = new List<DashboardGridHeader>();
        }
    }

    public class DashboardGridHeader
    {
        public string HeaderIdentifier { get; protected set; }
        public int Position { get; protected set; }
        public string HeaderText { get; protected set; } // visible to  user and can be localized. do not write any logic on it. 
        public string Alignment { get; protected set; }
        public string DataType { get; protected set; }
        public int MinWidth { get; protected set; } // mini width required for this column.
    }
}
