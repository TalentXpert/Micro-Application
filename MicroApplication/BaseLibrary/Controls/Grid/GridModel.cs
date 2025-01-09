namespace BaseLibrary.Controls.Grid
{
    public class GridModel
    {
        /// <summary>
        /// Will be used to display grid headers and filters
        /// </summary>
        public List<GridHeader> Headers { get; set; } 

        /// <summary>
        /// Data rows to be displayed to user
        /// </summary>
        public List<List<GridCell>> Rows { get; set; } 
        
        /// <summary>
        /// Paging information 
        /// </summary>
        public GridPagingInfo PagingInfo { get; set; } 
        
        public GridModel()
        {
            Rows = new List<List<GridCell>>();
            Headers = new List<GridHeader>();
            PagingInfo = new GridPagingInfo();
        }
    }
}
