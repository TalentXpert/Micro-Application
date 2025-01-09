using Microsoft.Data.SqlClient;

namespace BaseLibrary.Configurations.DataSources.SqlDataSources
{
    public abstract class BaseSqlDataSource : CleanCode
    {
        /// <summary>
        /// Implement this method to return your application SqlDataSources
        /// </summary>
        protected abstract List<SqlDataSource> GetApplicationSqlDataSources();

        /// <summary>
        /// Override this method if you want to change base SqlDataSources or their sequence. You can return empty list and return all SqlDataSources from GetApplicationSqlDataSources method.
        /// </summary>
        protected virtual List<SqlDataSource> GetBaseSqlDataSources()
        {
            return new List<SqlDataSource>()
           {
               new SqlDataSource("621225CA-CDE7-4756-9D0C-AA6CA350C3A3","User Active Status","select Case When IsBlocked=1 then 'Blocked' Else 'Active' End as TX_Status, Count(*) NU_Users from ApplicationUser where organizationid=@organizationid group by IsBlocked",SqlDataSourceType.Chart)
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

        public virtual List<SqlParameter> GetQueryParameters(string query, Dictionary<string, object>? parameters, ApplicationUser? user)
        {
            var param = new List<SqlParameter>();
            if (user != null)
            {
                if (user.OrganizationId.HasValue && ContainsIgnoreCase(query, "@organizationid"))
                    param.Add(new SqlParameter("@organizationid", user.OrganizationId.Value));
                if (ContainsIgnoreCase(query, "@userId"))
                    param.Add(new SqlParameter("@userId", user.Id));
            }
            if (parameters != null)
            {
                foreach (var key in parameters.Keys)
                {
                    if (param.Any(k => k.ParameterName == key))
                        continue;
                    param.Add(new SqlParameter(key, parameters[key]));
                }
            }
            return param;
        }
    }
}
