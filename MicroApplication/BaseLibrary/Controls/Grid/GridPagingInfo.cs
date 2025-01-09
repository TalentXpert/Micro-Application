namespace BaseLibrary.Controls.Grid
{
    public class GridPagingInfo
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        /// <summary>
        /// Total number of rows - it might be different from page size for last page
        /// </summary>
        public int TotalRows { get; set; }

    }
}
