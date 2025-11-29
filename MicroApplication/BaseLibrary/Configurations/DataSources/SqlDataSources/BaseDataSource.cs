
namespace BaseLibrary.Configurations.DataSources.SqlDataSources
{
    public abstract class BaseDataSource : CleanCode
    {
        
        /// <summary>
        /// Implement this method to return your application SqlDataSources
        /// </summary>
        protected abstract List<MacroSqlDataSource> GetApplicationSqlDataSources();
        protected abstract List<MacroCustomObjectDataSource> GetApplicationCustomObjectDataSources();

        /// <summary>
        /// Implement this method to prepare SqlParameter from give parameter and filter values. 
        /// </summary>
        /// <param name="baseControl"></param>
        /// <param name="parameter"></param>
        /// <param name="filterValues"></param>
        /// <returns></returns>
        protected abstract SqlParameter? GetParameter(MacroDataSourceParameter parameter, bool isMandatory, List<ControlValue> filterValues, Dictionary<string, string> globalFilterIds);

        /// <summary>
        /// Return all parameter list to display on dashboard builder to be used in data source query. These are the only parameters that can be used in data source queries.
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// Override this method if you want to change base SqlDataSources or their sequence. You can return empty list and return all SqlDataSources from GetApplicationSqlDataSources method.
        /// </summary>
        protected virtual List<MacroSqlDataSource> GetBaseSqlDataSources()
        {
            var sqlQuery = new MicroSqlQuery("select Case When IsBlocked=1 then 'Blocked' Else 'Active' End as TX_Status, Count(*) NU_Users from ApplicationUser where organizationid=@organizationid group by IsBlocked");
            return new List<MacroSqlDataSource>()
           {
               new MacroSqlDataSource("621225CA-CDE7-4756-9D0C-AA6CA350C3A3","User Active Status",sqlQuery,SqlDataSourceType.Chart)
           };
        }
        public List<MacroSqlDataSource> GetSqlDataSources()
        {
            var sqlDataSources = new List<MacroSqlDataSource>();
            sqlDataSources.AddRange(GetApplicationSqlDataSources());
            sqlDataSources.AddRange(GetBaseSqlDataSources());
            return sqlDataSources;
        }
        public List<MacroDataSource> GetAllDataSources()
        {
            var dataSources = new List<MacroDataSource>();
            dataSources.AddRange(GetSqlDataSources().Select(s=>MacroDataSource.CreateSqlDataSource(s)));
            dataSources.AddRange(GetApplicationCustomObjectDataSources().Select(s=>MacroDataSource.CreateCustomObjectDataSource(s)));
            //var sqlDataSources = GetSqlDataSources();
            //dataSources.AddRange(sqlDataSources);
            return dataSources;
        }
        public virtual string GetChartColumnDataType(string name)
        {
            if (name.Contains("nu", StringComparison.InvariantCultureIgnoreCase))
                return "number";
            return "string";
        }
        /// <summary>
        /// Remove prefix of length two and concat remaining to get column title TX_NU_Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual string GetChartColumnTitle(string name)
        {
            var parts = name.Split("_");
            string title = string.Empty;
            foreach (var part in parts)
            {
                if (part.Length == 2)
                    continue;
                title += " " + part;
            }
            return title.Trim();
        }
        
        public List<SqlParameter> GetQueryParameters(MicroSqlQuery microSqlQuery, List<ControlValue> filterValues, Dictionary<string, string> globalFilterIds, ApplicationUser? user, BaseDataSourceParameter baseDataSourceParameter, out string query)
        {
            var parameters = new List<SqlParameter>();
            var mandatoryParameters = microSqlQuery.GetAllParameters(microSqlQuery.QueryTextWithMandatoryParameters);

            foreach (var param in mandatoryParameters)
            {
                var parameter = baseDataSourceParameter.GetMacroDataSourceParameterBySqlParameterName(param);
                if(parameter == null)
                    throw new Exception($"Parameter {param} is not defined in data source parameters.");
                var sqlParameter = GetParameter(parameter, true, filterValues, globalFilterIds, user);
                if (sqlParameter == null)
                    throw new Exception($"Mandatory parameter {param} is missing for query.");
                parameters.Add(sqlParameter);
            }
            var optionalQueryText = new StringBuilder();
            foreach (var optionalQuery in microSqlQuery.OptionalFilterQueryTextWithParameters)
            {
                var optionalParameters = microSqlQuery.GetAllParameters(optionalQuery);
                var allParametersFound = true;
                foreach (var param in optionalParameters)
                {
                    var parameter = baseDataSourceParameter.GetMacroDataSourceParameterBySqlParameterName(param);
                    if (parameter == null)
                        throw new Exception($"Parameter {param} is not defined in data source parameters.");
                    SqlParameter? sqlParameter = GetParameter(parameter,false,filterValues, globalFilterIds, user);
                    if (sqlParameter != null)
                        parameters.Add(sqlParameter);
                    else
                        allParametersFound = false;
                }
                if (allParametersFound)
                    optionalQueryText.Append(" " + optionalQuery);
            }
            query = microSqlQuery.QueryTextWithMandatoryParameters.Replace(SqlQueryConstants.OptionFilterPlaceHolder, optionalQueryText.ToString());
            return parameters;
        }

        private SqlParameter? GetParameter(MacroDataSourceParameter parameter, bool isMandatory, List<ControlValue> filterValues, Dictionary<string, string> globalFilterIds, ApplicationUser? user)
        {
            var sqlParameter = GetStandardParameter(user, parameter.SqlParameterName);
            if (sqlParameter is null)
                sqlParameter = GetParameter(parameter, isMandatory, filterValues, globalFilterIds);
            return sqlParameter;
        }

        private static SqlParameter? GetStandardParameter(ApplicationUser? user, string? parameter)
        {
            if(parameter is null)
                return null;
            if (user is not null && user.OrganizationId.HasValue)
            {
                var orgParamsList = new List<string>()
                {
                    "@orgid",
                    "@organizationId",
                    "@OrganisationId",
                };
                if (orgParamsList.Any(p=>AreEqualsIgnoreCase(parameter,p)))
                    return new SqlParameter(parameter, user.OrganizationId.Value);
            }
            if (user is not null && AreEqualsIgnoreCase(parameter, "@userId"))
            {
                return new SqlParameter("@userId", user.Id);
            }

            return null;
        }

        public abstract List<object> GetCustomObjectList(IBaseLibraryServiceFactory sF, MacroDataSource datasource, ApplicationUser loggedInUser, List<ControlValue> filterValues, Dictionary<string, string> globalFilterIds);
    }
}
