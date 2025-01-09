
using System.Data;


namespace BaseLibrary.Domain.ComponentSchemas
{
    public class ChartSchema
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid DataSourceId { get; set; }
        public string ChartType { get; set; }
        public int MinWidth { get; set; }
        public int MinHeight { get; set; }
        public List<ChartColumnSchema> Columns { get; set; }=new List<ChartColumnSchema>();
    }

    public class ChartColumnSchema
    {
        public string DatabaseColumnName { get; set; }
        public string Title { get; set; }
        public string DataType { get; set; }
        public bool IsMandatory { get; set; }
        public string Color { set; get; } = string.Empty;
        public ChartColumnSchema() { }
        public void UpdateWith(DataColumn column, IMicroAppContract microApp) 
        {
            DatabaseColumnName = column.ColumnName;
            DataType = microApp.GetBaseSqlDataSource().GetChartColumnDataType(column.ColumnName);
            IsMandatory = DataType == "string";
            if(string.IsNullOrWhiteSpace(Title))
                Title = microApp.GetBaseSqlDataSource().GetChartColumnTitle(column.ColumnName);
        }
        public ChartColumnSchema(string dbcolumnName,string title,string dataType,bool isMandatory)
        {
            DatabaseColumnName = dbcolumnName;
            Title = title;
            DataType = dataType;
            IsMandatory = isMandatory;
        }
    }
}
