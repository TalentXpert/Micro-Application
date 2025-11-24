using BaseLibrary.Configurations;
using BaseLibrary.Configurations.Dashboards;
using BaseLibrary.Configurations.Dashboards.Charts;
using BaseLibrary.Configurations.DataSources.LinqDataSources;
using BaseLibrary.Configurations.DataSources.SqlDataSources;
using BaseLibrary.Security;

namespace BaseLibrary
{
    public interface IMicroAppContract
    {
        FormHandlerFactory GetFormHandlerFactory();
        BasePermission GetApplicationPermission();
        BaseSearch GetApplicationSearch();
        BaseMenu GetApplicationMenu();
        BaseHandlerFactory GetHandlerFactory();
        List<LinqDataSource> GetDataSources();
        AuthOptions? GetAuthOptions();
        List<AppControl> GetApplicationControls();
        BaseControl GetBaseControl();
        List<string> GetWebsiteAdminLogins();
        List<AppForm> GetApplicationFormsWithControls();
        List<AppPage> GetApplicationPages();
        List<SqlDataSource> GetSqlDataSources();
        List<ChartSchema> GetApplicationCharts();
        List<DashboardSchema> GetApplicationDashboards();
        BaseSqlDataSource GetBaseSqlDataSource();
        BaseDataSourceParameter GetBaseDataSourceParameter();
        BaseChart GetBaseChart();
        void TrackActivity(int activityType, string activityDataJson,ApplicationUser loggedInUser);
    }
}
