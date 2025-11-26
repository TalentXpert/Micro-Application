namespace BaseLibrary.Domain.ComponentSchemas
{
    /// <summary>
    /// This class represent chart data that is used to render chart on dashboard
    /// </summary>
    public class DashboardChart
    {
        public string ChartType { get; set; } //It is of type ChartType
        public string Title { get; set; }
        public int MinHeight { get; set; }
        public int MinWidth { get; set; }
        public List<ChartColumn> Columns { get; set; } = [];
        public List<List<string>> SeriesData { get; set; } = []; ///This hold series data for chart one cell for one column. 
        public DashboardChart(ChartSchema schema, DataTable data)
        {
            ChartType = schema.ChartType;
            Title = schema.Name;
            MinHeight = schema.MinHeight;
            MinWidth = schema.MinWidth;
            foreach (var c in schema.Columns)
                Columns.Add(new ChartColumn { Title = c.Title, DataType = c.DataType, Color = c.Color });

            foreach (DataRow dr in data.Rows)
            {
                var d = new List<string>();
                foreach (var c in schema.Columns)
                {
                    if (dr.IsNull(c.DatabaseColumnName))
                        continue;
                    var seriesData = dr[c.DatabaseColumnName]?.ToString()?.Trim();
                    if (seriesData != null)
                        d.Add(seriesData);
                }
                SeriesData.Add(d);
            }
        }
    }

    /// <summary>
    /// This class represent chart column that is used to define chart columns info for rendering chart
    /// </summary>
    public class ChartColumn
    {
        public string Title { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string Color { set; get; } = string.Empty;
    }
}
