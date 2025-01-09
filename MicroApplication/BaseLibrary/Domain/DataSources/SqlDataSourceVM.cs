namespace BaseLibrary.Domain.DataSources
{
    public class SqlDataSourceVM
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Query { get; set; }
        public string DataSourceType { get; set; }
        public SqlDataSourceVM() { }
        public SqlDataSourceVM(string id, string name, string query, string dataSourceType)
        {
            Id = Guid.Parse(id);
            Name = name;
            Query = query;
            DataSourceType = dataSourceType;
        }
    }
}
