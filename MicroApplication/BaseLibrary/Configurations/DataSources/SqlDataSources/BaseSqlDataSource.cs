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
        protected abstract SqlParameter? GetParameter(BaseControl baseControl, string parameter, bool isMandatory, List<ControlValue> filterValues);

        /// <summary>
        /// Return all parameter list to display on dashboard builder to be used in data source query. These are the only parameters that can be used in data source queries.
        /// </summary>
        /// <returns></returns>
        public abstract List<string> GetAllParameters();

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

        //public List<SqlParameter> GetQueryParameters(List<string> allQueryParameters, MicroSqlQuery microSqlQuery, List<ControlValue> filterValues, ApplicationUser? user, BaseControl baseControl, out string query)
        //{
        //    var parameters = new List<SqlParameter>();
        //    foreach (var param in allQueryParameters)
        //    {
        //        if (microSqlQuery.QueryTextWithMandatoryParameters.Contains(param, StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            var sqlParameter = GetStandardParameter(user, param);
        //            if (sqlParameter is null)
        //                GetParameter(baseControl, param, true, filterValues);
        //            if (sqlParameter == null)
        //                throw new Exception($"Mandatory parameter {param} is missing for query.");
        //            parameters.Add(sqlParameter);
        //            continue;
        //        }
        //        if (microSqlQuery.QueryTextWithMandatoryParameters.Contains(param, StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            var sqlParameter = GetStandardParameter(user, param);
        //            if (sqlParameter is null)
        //                sqlParameter = GetParameter(baseControl, param, false, filterValues);
        //            if (sqlParameter != null)
        //                parameters.Add(sqlParameter);
        //            continue;
        //        }
        //    }
        //    query = BuildQuery(microSqlQuery, parameters);
        //    return parameters;
        //}
        public List<SqlParameter> GetQueryParameters(MicroSqlQuery microSqlQuery, List<ControlValue> filterValues, ApplicationUser? user, BaseControl baseControl, out string query)
        {
            var parameters = new List<SqlParameter>();
            var mandatoryParameters = microSqlQuery.GetAllParameters(microSqlQuery.QueryTextWithMandatoryParameters);

            foreach (var param in mandatoryParameters)
            {
                var sqlParameter = GetParameter(filterValues, user, baseControl, param, true);
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
                    SqlParameter? sqlParameter = GetParameter(filterValues, user, baseControl, param,false);
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

        private SqlParameter? GetParameter(List<ControlValue> filterValues, ApplicationUser? user, BaseControl baseControl, string param, bool isMandatory)
        {
            var sqlParameter = GetStandardParameter(user, param);
            if (sqlParameter is null)
                sqlParameter = GetParameter(baseControl, param, isMandatory, filterValues);
            return sqlParameter;
        }

        private static SqlParameter? GetStandardParameter(ApplicationUser? user, string parameter)
        {
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
    }
}
