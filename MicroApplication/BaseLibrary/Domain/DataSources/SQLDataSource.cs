namespace BaseLibrary.Domain.DataSources
{

    public class SqlDataSource : Entity
    {
        public string Name { get; protected set; }=string.Empty;
        public string Query { get; protected set; } = string.Empty;
        public string DataSourceType { get; protected set; } = string.Empty;
        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
        protected SqlDataSource() { }
        public SqlDataSource(string id, string name, string query, SqlDataSourceType dataSourceType)
        {
            Id = Guid.Parse(id);
            Name = name;
            Query = query;
            DataSourceType = dataSourceType.Name;
            SetCreatedOn();
            SetUpdatedOn();
        }
        public void Update(string name, string query, string dataSourceType)
        {
            Name = name;
            Query = query;
            DataSourceType = dataSourceType;
        }
        public static SqlDataSource Create(SqlDataSourceVM vm)
        {
            if (vm.Id.HasValue == false)
                vm.Id = IdentityGenerator.NewSequentialGuid();
            var ds = new SqlDataSource
            {
                Id = vm.Id.Value,
            };
            ds.SetCreatedOn();
            ds.Update(vm);
            return ds;
        }
        public void Update(SqlDataSourceVM vm)
        {
            Name = vm.Name;
            Query = vm.Query;
            DataSourceType = vm.DataSourceType;
            SetUpdatedOn();
        }
    }
}
