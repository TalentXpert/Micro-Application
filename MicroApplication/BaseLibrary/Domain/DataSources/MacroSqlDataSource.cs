namespace BaseLibrary.Domain.DataSources
{
    public class SqlQueryConstants
    {
        public const string OptionFilterPlaceHolder = "##OptionalFilterPlaceHolder##";
    }

    public class MicroSqlQueryParameter
    {
        public string Name { get; set; } = string.Empty;
        public bool IsMandatory { get; set; }
        public MicroSqlQueryParameter() { }
        public MicroSqlQueryParameter(string name, bool isMandatory)
        {
            Name = name;
            IsMandatory = isMandatory;
        }
    }

    public class MicroSqlQuery
    {
        /// <summary>
        /// This is the base query text that contains mandatory parameters with placeholders for optional filters.
        /// </summary>
        public string QueryTextWithMandatoryParameters { get; set; } = "select * from user where organization=@orgId ##OptionalFilterPlaceHolder##";
        /// <summary>
        /// This is a list of optional query texts that can be appended to the base query. Each optional query text may contain its own parameters. Only those optional query texts whose parameters are provided will be included in the final query.
        /// </summary>
        public List<string> OptionalFilterQueryTextWithParameters { get; set; } = []; // e.g. "and (age > @age || age < @age)"
        public MicroSqlQuery() { }
        public MicroSqlQuery(string queryTextWithMandatoryParameters)
        {
            QueryTextWithMandatoryParameters = queryTextWithMandatoryParameters;
        }
        public MicroSqlQuery(string queryTextWithMandatoryParameters, List<string> optionalQueryTextWithParameters) : this(queryTextWithMandatoryParameters)
        {
            OptionalFilterQueryTextWithParameters.AddRange(optionalQueryTextWithParameters);
        }
        public void AddOptionalFilterQueryTextWithParameter(string optionQuery)
        {
            OptionalFilterQueryTextWithParameters.Add(optionQuery);
        }
        public List<string> GetAllParameters(string query)
        {
            return X.Extension.String.GetAllWordStartWith(query, "@");
        }
        public List<MicroSqlQueryParameter> GetAllQueryParameters()
        {
            var parameters = new List<MicroSqlQueryParameter>();
            var mandatoryParams = GetAllParameters(QueryTextWithMandatoryParameters);
            foreach (var param in mandatoryParams)
            {
                parameters.Add(new MicroSqlQueryParameter(param, true));
            }
            foreach (var optionalQuery in OptionalFilterQueryTextWithParameters)
            {
                var optionalParams = GetAllParameters(optionalQuery);
                foreach (var param in optionalParams)
                {
                    if (parameters.Any(p => p.Name == param))
                        continue;
                    parameters.Add(new MicroSqlQueryParameter(param, false));
                }
            }
            return parameters;
        }
    }

    public class MacroDataSourceType
    {
        private MacroDataSourceType() { }
        public string Name { get; protected set; } = string.Empty;
        public int Value { get; protected set; }
        public static MacroDataSourceType Sql = new MacroDataSourceType() { Value = 1, Name = "SQL" };
        public static MacroDataSourceType CustomObjectList = new MacroDataSourceType() { Value = 2, Name = "COL" };
        public static bool AreEqual(string name, MacroDataSourceType dataSource)
        {
            return C.AreEqualsIgnoreCase(name, dataSource.Name);
        }
        public static bool AreEqual(int value, MacroDataSourceType dataSource)
        {
            return value == dataSource.Value;
        }
    }

    public class MacroDataSource : Entity
    {
        public string Name { get; protected set; } = string.Empty;
        /// <summary>
        /// MacroDataSourceType class defines type of data source like SQL, CustomObjectList etc.
        /// </summary>
        public int DataSourceType { get; protected set; }
        public string TargetContentType { get; protected set; }= string.Empty;
        public Guid? OrganizationId { get; protected set; }
        public Guid? UserId { get; protected set; }
        public string JSONData { get; protected set; } = string.Empty;
        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
        public MacroSqlDataSource GetSqlDataSource()
        {
            try
            {
                var d = NewtonsoftJsonAdapter.DeserializeObject<MacroSqlDataSource>(JSONData) ?? throw new ValidationException("Invalid Sql Data Source JSON data.");
                return d;
            }
            catch
            {
                throw new ValidationException("Invalid Sql Data Source JSON data.");
            }
        }
        public static MacroDataSource CreateSqlDataSource(MacroSqlDataSource sqlDataSource)
        {
            var dataSource = new MacroDataSource()
            {
                Name = sqlDataSource.Name,
                DataSourceType = MacroDataSourceType.Sql.Value,
                TargetContentType = sqlDataSource.DataSourceType,
                JSONData = NewtonsoftJsonAdapter.SerializeObject(sqlDataSource),
                CreatedOn = DateTime.UtcNow,
                Id= sqlDataSource.Id,
                UpdatedOn = DateTime.UtcNow
            };
            return dataSource;
        }
    }

    public class MacroSqlDataSource
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; } = string.Empty;
        public string Query { get; protected set; } = string.Empty;
        public string DataSourceType { get; protected set; } = SqlDataSourceType.Chart.Name;

        protected MacroSqlDataSource() { }

        public MacroSqlDataSource(string id, string name, MicroSqlQuery query, SqlDataSourceType dataSourceType)
        {
            Id = Guid.Parse(id);
            Name = name;
            Query = NewtonsoftJsonAdapter.SerializeObject(query);
            DataSourceType = dataSourceType.Name;
        }
        public MacroSqlDataSource(string name, MicroSqlQuery query, SqlDataSourceType dataSourceType) : this(IdentityGenerator.NewSequentialGuid().ToString(), name, query, dataSourceType)
        {

        }
        public void Update(string name, MicroSqlQuery query, string dataSourceType)
        {
            Name = name;
            Query = NewtonsoftJsonAdapter.SerializeObject(query);
            DataSourceType = dataSourceType;
        }
        public MicroSqlQuery? GetSqlQuery()
        {
            try
            {
                return NewtonsoftJsonAdapter.DeserializeObject<MicroSqlQuery>(Query);
            }
            catch
            {
                if (string.IsNullOrWhiteSpace(Query) == false)
                    return new MicroSqlQuery(Query);
            }
            return null;
        }
    }
}
