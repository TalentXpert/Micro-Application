using BaseLibrary.Domain.ComponentSchemas;

namespace BaseLibrary.Configurations.Dashboards
{
    public abstract class BaseDashboard
    {
        /// <summary>
        /// Implement this method to return your application Dashboards 
        /// </summary>
        protected abstract List<DashboardSchema> GetApplicationDashboards();

        /// <summary>
        /// Override this method if you want to change base Dashboards or their sequence. You can return empty list and return all Dashboards from GetApplicationDashboards method.
        /// </summary>
        protected virtual List<DashboardSchema> GetBaseDashboards()
        {
            return new List<DashboardSchema>()
            {

            };
        }

        public List<DashboardSchema> GetDashboards()
        {
            var sqlDataSources = new List<DashboardSchema>();
            sqlDataSources.AddRange(GetApplicationDashboards());
            sqlDataSources.AddRange(GetBaseDashboards());
            return sqlDataSources;
        }
        public DashboardSchema? GetDashboardById(Guid id)
        {
            return GetDashboards().FirstOrDefault(ds => ds.Id == id);
        }
        
    }
    
}

