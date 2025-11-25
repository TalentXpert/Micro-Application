namespace BaseLibrary.Domain.ComponentSchemas
{
    public class DashboardChart
    {
        public string ChartType { get; set; } //ChartType
        public string Title { get; set; }
        public int MinHeight { get; set; }
        public int MinWidth { get; set; }
        public List<ChartColumn> Columns { get; set; } = [];
        public List<List<string>> SeriesData { get; set; } = [];
        public DashboardChart(ChartSchema schema,DataTable data)
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
                    if(dr[c.DatabaseColumnName] is not null)
                        d.Add(dr[c.DatabaseColumnName].ToString().Trim());
                }
                SeriesData.Add(d);
            }
        }
    }

    public class ChartColumn
    {
        public string Title { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string Color { set; get; } = string.Empty;
    }
}
