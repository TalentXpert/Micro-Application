
namespace BaseLibrary.Services
{
    public interface IChartService
    {
        DashboardChart GetChart(Guid chartId, Guid? organizationId, ApplicationUser? loggedInUser);
        DashboardChart GetChartPreview(ChartSchema chartSchema, Guid? organizationId, ApplicationUser? loggedInUser);
    }

    public class ChartService : ServiceLibraryBase, IChartService
    {
        public ChartService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory)
        {
        }
        public DashboardChart GetChart(Guid chartId, Guid? organizationId, ApplicationUser? loggedInUser)
        {
            var ds = SF.ComponentSchemaService.GetComponent(chartId);
            if (ds == null)
                throw new ValidationException($"Chart with {chartId} not found.");
            var chartSchema = ds.GetChartSchema();
            return GetChartPreview(chartSchema, organizationId, loggedInUser);
        }
        public DashboardChart GetChartPreview(ChartSchema chartSchema, Guid? organizationId, ApplicationUser? loggedInUser)
        {
            var datasource = SF.RF.SqlDataSourceRepository.Get(chartSchema.DataSourceId);
            var query = datasource.Query;
            using (var db = new SqlCommandExecutor())
            {
                var param = SF.MicroAppContract.GetBaseSqlDataSource().GetQueryParameters(query,null,loggedInUser);
                var dataTable = db.GetDataTable(query, param);
                var chart = new DashboardChart(chartSchema, dataTable);
                return chart;
            }
        }
    }
}