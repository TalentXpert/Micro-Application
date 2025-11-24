namespace BaseLibrary.Configurations.DataSources.SqlDataSources
{
    public abstract class BaseSqlDataSource : CleanCode
    {
        /// <summary>
        /// Implement this method to return your application SqlDataSources
        /// </summary>
        protected abstract List<SqlDataSource> GetApplicationSqlDataSources();

        /// <summary>
        /// Implement this method to prepare SqlParameter from give parameter and filter values. 
        /// </summary>
        /// <param name="baseControl"></param>
        /// <param name="parameter"></param>
        /// <param name="filterValues"></param>
        /// <returns></returns>
        protected abstract SqlParameter? GetParameter(BaseControl baseControl, MicroSqlQueryParameter parameter, List<ControlValue> filterValues);

        /// <summary>
        /// Return all this parameter list to display on dashboard builder to be used in data source query.
        /// </summary>
        /// <returns></returns>
        protected abstract List<string> GetAllParameters();

        /// <summary>
        /// Override this method if you want to change base SqlDataSources or their sequence. You can return empty list and return all SqlDataSources from GetApplicationSqlDataSources method.
        /// </summary>
        protected virtual List<SqlDataSource> GetBaseSqlDataSources()
        {
            var sqlQuery = new MicroSqlQuery("select Case When IsBlocked=1 then 'Blocked' Else 'Active' End as TX_Status, Count(*) NU_Users from ApplicationUser where organizationid=@organizationid group by IsBlocked");
            return new List<SqlDataSource>()
           {
               new SqlDataSource("621225CA-CDE7-4756-9D0C-AA6CA350C3A3","User Active Status",sqlQuery,SqlDataSourceType.Chart)
           };
        }
        public List<SqlDataSource> GetSqlDataSources()
        {
            var sqlDataSources = new List<SqlDataSource>();
            sqlDataSources.AddRange(GetApplicationSqlDataSources());
            sqlDataSources.AddRange(GetBaseSqlDataSources());
            return sqlDataSources;
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

        public virtual List<SqlParameter> GetQueryParameters(MicroSqlQuery microSqlQuery, List<ControlValue> filterValues, ApplicationUser? user, BaseControl baseControl, out string query)
        {
            var parameters = new List<SqlParameter>();
            foreach (var parameter in microSqlQuery.Parameters)
            {
                if (user is not null && user.OrganizationId.HasValue && AreEqualsIgnoreCase(parameter.Name, "@organizationid") || AreEqualsIgnoreCase(parameter.Name, "@orgid"))
                { parameters.Add(new SqlParameter("@organizationid", user.OrganizationId.Value)); continue; }
                if (user is not null && AreEqualsIgnoreCase(parameter.Name, "@userId"))
                { parameters.Add(new SqlParameter("@userId", user.Id)); continue; }
                var param = GetParameter(baseControl, parameter, filterValues);
                if (param != null)
                    parameters.Add(param);
            }
            query = BuildQuery(microSqlQuery, parameters);
            return parameters;
        }

        private string BuildQuery(MicroSqlQuery microSqlQuery, List<SqlParameter> parameters)
        {
            var parametersWithValue = parameters.Select(p => p.ParameterName).ToList();
            var optionalQueryText = new StringBuilder();
            foreach (var optionalQuery in microSqlQuery.OptionalFilterQueryTextWithParameters)
            {
                var optionalQueryParameters = microSqlQuery.GetAllParameters(optionalQuery);
                bool hasAllQueryParameters = true;
                foreach (var parameter in optionalQueryParameters)
                {
                    if (parametersWithValue.Any(p => AreNotEqualsIgnoreCase(p, parameter)) == false)
                        hasAllQueryParameters = false;
                }
                if (hasAllQueryParameters)
                    optionalQueryText.Append(" " + optionalQuery);
            }
            var query = microSqlQuery.QueryTextWithMandatoryParameters.Replace(SqlQueryConstants.OptionFilterPlaceHolder, optionalQueryText.ToString());
            return query;
        }
    }
}
