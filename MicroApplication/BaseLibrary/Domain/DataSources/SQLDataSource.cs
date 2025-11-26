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

    public class SqlDataSource : Entity
    {
        public string Name { get; protected set; } = string.Empty;
        public string Query { get; protected set; } = string.Empty;
        public string DataSourceType { get; protected set; } = SqlDataSourceType.Chart.Name;
        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
        protected SqlDataSource() { }

        public SqlDataSource(string id, string name, MicroSqlQuery query, SqlDataSourceType dataSourceType)
        {
            Id = Guid.Parse(id);
            Name = name;
            Query = NewtonsoftJsonAdapter.SerializeObject(query);
            DataSourceType = dataSourceType.Name;
            SetCreatedOn();
            SetUpdatedOn();
        }
        public SqlDataSource(string name, MicroSqlQuery query, SqlDataSourceType dataSourceType) : this(IdentityGenerator.NewSequentialGuid().ToString(),name,query, dataSourceType)
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
                if(string.IsNullOrWhiteSpace(Query)==false)
                    return new MicroSqlQuery(Query);
            }
            return null;    
        }
    }
}
