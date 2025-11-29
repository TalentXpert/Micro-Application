
using BaseLibrary.Controllers;
using BaseLibrary.Domain.ComponentSchemas;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace BaseLibrary.Services
{
    public interface IChartService
    {
        DashboardChart GetChart(GetDashboardChartInputVM model, ApplicationUser loggedInUser);
        DashboardChart GetChartPreview(Guid chartId, ChartSchema chartSchema, ApplicationUser loggedInUser, List<ControlValue> filterValues, Dictionary<string, string> globalFilterIds);
    }

    public class ChartService : ServiceLibraryBase, IChartService
    {
        public ChartService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<ChartService>())
        {
        }
        public DashboardChart GetChart(GetDashboardChartInputVM model, ApplicationUser loggedInUser)
        {
            var chartSchema = SF.ComponentSchemaService.GetChartSchema(model.ChartId);
            return GetChartPreview(model.ChartId, chartSchema, loggedInUser, model.ControlFilterValues,model.GlobalFilterValues);
        }
        public DashboardChart GetChartPreview(Guid chartId, ChartSchema chartSchema, ApplicationUser loggedInUser, List<ControlValue> filterValues, Dictionary<string, string> globalFilterIds)
        {
            var model = new GetDashboardChartInputVM { ChartId = chartId, ControlFilterValues = filterValues, GlobalFilterValues = globalFilterIds };
            return BuildChart(model, loggedInUser);
        }

        private DashboardChart BuildChart(GetDashboardChartInputVM model, ApplicationUser loggedInUser)
        {
            var chartSchema = SF.ComponentSchemaService.GetChartSchema(model.ChartId);
            var datasource = SF.MicroAppContract.GetBaseDataSource().GetAllDataSources().FirstOrDefault(ds => ds.Id == chartSchema.DataSourceId);
            if (datasource is null)
                throw new ValidationException($"Data source with id {chartSchema.DataSourceId} not found for chart {model.ChartId}.");
            if (MacroDataSourceType.AreEqual(datasource.DataSourceType, MacroDataSourceType.Sql))
                return GetDashboardChartFromSqlDataSource(datasource.GetSqlDataSource(), model.ChartId, chartSchema, loggedInUser, model.ControlFilterValues, model.GlobalFilterValues);
            if (MacroDataSourceType.AreEqual(datasource.DataSourceType, MacroDataSourceType.CustomObjectList))
                return GetDashboardChartFromCustomObjectList(datasource, model.ChartId, chartSchema, loggedInUser, model.ControlFilterValues, model.GlobalFilterValues);
            throw new ValidationException($"No datasource found for {model.ChartId}.");
        }

        private DashboardChart GetDashboardChartFromCustomObjectList(MacroDataSource datasource, Guid chartId, ChartSchema chartSchema, ApplicationUser loggedInUser, List<ControlValue> filterValues, Dictionary<string, string> globalFilterIds)
        {
            var dataObjects = SF.MicroAppContract.GetBaseSqlDataSource().GetCustomObjectList(SF,datasource, loggedInUser, filterValues, globalFilterIds);
            var chart = new DashboardChart(chartSchema, dataObjects);
            return chart;
        }

        private DashboardChart GetDashboardChartFromSqlDataSource(MacroSqlDataSource datasource, Guid chartId, ChartSchema chartSchema, ApplicationUser? loggedInUser, List<ControlValue> filterValues, Dictionary<string, string> globalFilterIds)
        {
            var microSqlQuery = datasource.GetSqlQuery();
            if (microSqlQuery is null)
                throw new ValidationException($"Data source with id {chartSchema.DataSourceId} found without data query for chart {chartId}.");

            using (var db = new SqlCommandExecutor())
            {
                var param = SF.MicroAppContract.GetBaseSqlDataSource().GetQueryParameters(microSqlQuery, filterValues, globalFilterIds, loggedInUser,SF.MicroAppContract.GetBaseDataSourceParameter(), out string query);
                var dataTable = db.GetDataTable(query, param);
                var chart = new DashboardChart(chartSchema, dataTable);
                return chart;
            }
        }
    }
}