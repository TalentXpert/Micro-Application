
namespace BaseLibrary.Domain.ComponentSchemas
{
    /// <summary>
    /// This class represent chart schema which mainlly has data source and columns info
    /// </summary>
    public class ChartSchema
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid DataSourceId { get; set; }
        public string ChartType { get; set; } = "";
        public int MinWidth { get; set; }
        public int MinHeight { get; set; }
        public List<ChartColumnSchema> Columns { get; set; } = new List<ChartColumnSchema>();
    }

    /// <summary>
    /// This class represent chart column schema that help to define chart columns info which is used to prepare data series from actual data source
    /// </summary>
    public class ChartColumnSchema
    {
        /// <summary>
        /// Database column name that hold this column data
        /// </summary>
        public string DatabaseColumnName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public bool IsMandatory { get; set; }
        public string Color { set; get; } = string.Empty;
        public ChartColumnSchema() { }
        public void UpdateWith(DataColumn column, IMicroAppContract microApp)
        {
            DatabaseColumnName = column.ColumnName;
            DataType = microApp.GetBaseSqlDataSource().GetChartColumnDataType(column.ColumnName);
            IsMandatory = DataType == "string";
            if (string.IsNullOrWhiteSpace(Title))
                Title = microApp.GetBaseSqlDataSource().GetChartColumnTitle(column.ColumnName);
        }
        public ChartColumnSchema(string dbcolumnName, string title, string dataType, bool isMandatory, string color = "")
        {
            DatabaseColumnName = dbcolumnName;
            Title = title;
            DataType = dataType;
            IsMandatory = isMandatory;
            Color = color;
        }
        public void SetColor(string color)
        {
            Color = color;
        }
    }
}
