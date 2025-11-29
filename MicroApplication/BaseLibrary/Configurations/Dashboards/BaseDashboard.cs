namespace BaseLibrary.Configurations.Dashboards
{
    public abstract class BaseDashboard
    {
        /// <summary>
        /// Implement this method to return your application Dashboards 
        /// </summary>
        protected abstract List<IMacroDashboard> GetApplicationDashboards();

        /// <summary>
        /// Override this method if you want to change base Dashboards or their sequence. You can return empty list and return all Dashboards from GetApplicationDashboards method.
        /// </summary>
        protected virtual List<IMacroDashboard> GetBaseDashboards()
        {
            return new List<IMacroDashboard>()
            {

            };
        }

        public List<IMacroDashboard> GetDashboards()
        {
            var sqlDataSources = new List<IMacroDashboard>();
            sqlDataSources.AddRange(GetApplicationDashboards());
            sqlDataSources.AddRange(GetBaseDashboards());
            return sqlDataSources;
        }
        public IMacroDashboard? GetDashboardById(Guid id)
        {
            return GetDashboards().FirstOrDefault(ds => ds.Id == id);
        }
    }

    public class MacroDashboard : IMacroDashboard
    {
        public Guid Id { get; set; }

        public List<MacroDashboardParameter> MacroDataSourceParameters { get; set; } = [];

        public DashboardSchema Dashboard { get; set; }
        protected MacroDashboard() { }
        public MacroDashboard(Guid id, List<MacroDashboardParameter> macroDataSourceParameters, DashboardSchema dashboard)
        {
            Id = id;
            MacroDataSourceParameters = macroDataSourceParameters;
            Dashboard = dashboard;
        }
    }
    public interface IMacroDashboard
    {
        public Guid Id { get; }
        List<MacroDashboardParameter> MacroDataSourceParameters { get;}
        DashboardSchema Dashboard { get; }
    }

    public class MacroDashboardParameter
    {
        public Guid MacroDataSourceParameterId { get; set; } 
        public bool IsMandatory { get; set; } = false;
    }
}

