

using BaseLibrary.Configurations.DataSources.LinqDataSources;
using BaseLibrary.Configurations.DataSources.SqlDataSources;
using BaseLibrary.Domain.ComponentSchemas;
using BaseLibrary.Domain.DataSources;

namespace MicroAppAPI.Configurations
{
    public class MicroAppContract : IMicroAppContract
    {
        public IServiceFactory SF { get; }

        public MicroAppContract(IServiceFactory serviceFactory)
        {
            SF = serviceFactory;
        }
        public BasePermission GetApplicationPermission()
        {
            return new ApplicationPermission();
        }

        public AuthOptions? GetAuthOptions() 
        { 
            return ApplicationSettings.AuthOptions; 
        }
        

        public List<LinqDataSource> GetDataSources()
        {
            return DataSources.GetDataSources();
        }
        
        public List<string> GetWebsiteAdminLogins()
        {
            var logins = ApplicationSettings.WebsiteAdminLoginId;
            if(string.IsNullOrWhiteSpace(logins))
                return new List<string>();
            var ids = logins.Split(new string[]{ ",", ";" },StringSplitOptions.RemoveEmptyEntries);
            return ids.ToList();
        }
        public List<AppControl> GetApplicationControls()
        {
            return new ApplicationControl().GetControls();
        }

        public List<AppForm> GetApplicationFormsWithControls()
        {
            return new ApplicationForm().GetFormsWithControls();
        }

        public List<AppPage> GetApplicationPages()
        {
            return new ApplicationPage().GetPages();
        }

        public List<SqlDataSource> GetSqlDataSources()
        {
            return new ApplicationSqlDataSource().GetSqlDataSources();
        }

        public List<ChartSchema> GetApplicationCharts()
        {
            return new ApplicationChart().GetCharts();
        }

        public List<DashboardSchema> GetApplicationDashboards()
        {
            return new ApplicationDashboard().GetDashboards();
        }

        public BaseHandlerFactory GetHandlerFactory()
        {
            return new ApplicationHandlerFactory(SF);
        }

        public BaseMenu GetApplicationMenu()
        {
            return new ApplicationMenu();
        }

        public FormHandlerFactory GetFormHandlerFactory()
        {
            return new ApplicationFormHandlerFactory(SF);
        }

        public BaseSearch GetApplicationSearch()
        {
            return new ApplicationSearch(SF);
        }

        public BaseSqlDataSource GetBaseSqlDataSource()
        {
            return new ApplicationSqlDataSource();
        }

        public BaseDataSourceParameter GetBaseDataSourceParameter()
        {
            return new BaseDataSourceParameter();
        }
    }
}
