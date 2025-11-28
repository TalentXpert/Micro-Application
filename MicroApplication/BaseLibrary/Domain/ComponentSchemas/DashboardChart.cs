using System.Reflection;

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
                    {
                        d.Add(GetEmptyData(c.DataType));
                        continue;
                    }
                    var seriesData = dr[c.DatabaseColumnName]?.ToString()?.Trim();
                    if (seriesData != null)
                        d.Add(seriesData);
                }
                SeriesData.Add(d);
            }
        }
        public DashboardChart(ChartSchema schema, List<object> objectData)
        {
            ChartType = schema.ChartType;
            Title = schema.Name;
            MinHeight = schema.MinHeight;
            MinWidth = schema.MinWidth;
            foreach (var c in schema.Columns)
                Columns.Add(new ChartColumn { Title = c.Title, DataType = c.DataType, Color = c.Color });

            var myObject = objectData.FirstOrDefault();
            if (myObject is null)
                return;
            Type objectType = myObject.GetType();
            Dictionary<string, PropertyInfo> propertyInfo = new Dictionary<string, PropertyInfo>();
            foreach (var c in schema.Columns)
            {
                PropertyInfo? propInfo = objectType.GetProperty(c.DatabaseColumnName);
                if (propInfo is null)
                    continue;
                propertyInfo[c.DatabaseColumnName] = propInfo;
            }

            foreach (object o in objectData)
            {
                var d = new List<string>();
                foreach (var c in schema.Columns)
                {

                    if (propertyInfo.TryGetValue(c.DatabaseColumnName, out PropertyInfo pi))
                    {
                        object? propertyValue = pi.GetValue(o,null);
                        var seriesData = propertyValue?.ToString()?.Trim();
                        if (seriesData != null)
                            d.Add(seriesData);
                    }
                    else
                        d.Add(GetEmptyData(c.DataType));
                }
                SeriesData.Add(d);
            }
        }

        private string GetEmptyData(string dataType)
        {
            switch (dataType)
            {
                case "number":
                    return "0";
                case "date":
                    return DateTime.MinValue.ToString("yyyy-MM-dd");
                case "datetime":
                    return DateTime.MinValue.ToString("yyyy-MM-ddTHH:mm:ss");
                case "boolean":
                    return "false";
                case "string":
                    return "";
            }
            return "0";
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
