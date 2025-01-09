namespace BaseLibrary.Domain.DataSources
{
    public class SqlDataSourceType
    {
        public string Name { get; }
        private SqlDataSourceType(string name)
        {
            Name = name;
        }

        public static SqlDataSourceType Chart = new SqlDataSourceType("Chart");
    }
}
