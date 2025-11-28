
using BaseLibrary.Controllers;
using BaseLibrary.Domain.ComponentSchemas;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace BaseLibrary.Services
{
    public interface IChartService
    {
        DashboardChart GetChart(GetDashboardChartInputVM model, ApplicationUser? loggedInUser);
        DashboardChart GetChartPreview(Guid chartId, ChartSchema chartSchema, ApplicationUser? loggedInUser, List<ControlValue> filterValues);
    }

    public class ChartService : ServiceLibraryBase, IChartService
    {
        public ChartService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<ChartService>())
        {
        }
        public DashboardChart GetChart(GetDashboardChartInputVM model, ApplicationUser? loggedInUser)
        {
            var chartSchema = SF.ComponentSchemaService.GetChartSchema(model.ChartId);
            return GetChartPreview(model.ChartId, chartSchema, loggedInUser, model.FilterValues);
        }
        public DashboardChart GetChartPreview(Guid chartId, ChartSchema chartSchema, ApplicationUser? loggedInUser, List<ControlValue> filterValues)
        {
            var datasource = SF.MicroAppContract.GetBaseDataSource().GetAllDataSources().FirstOrDefault(ds => ds.Id == chartSchema.DataSourceId);
            if (datasource is null)
                throw new ValidationException($"Data source with id {chartSchema.DataSourceId} not found for chart {chartId}.");
            if (MacroDataSourceType.AreEqual(datasource.DataSourceType, MacroDataSourceType.Sql))
                return GetDashboardChartFromSqlDataSource(datasource.GetSqlDataSource(), chartId, chartSchema, loggedInUser, filterValues);
            if (MacroDataSourceType.AreEqual(datasource.DataSourceType, MacroDataSourceType.CustomObjectList))
                return GetDashboardChartFromCustomObjectList(datasource, chartId, chartSchema, loggedInUser, filterValues);
            throw new ValidationException($"No datasource found for {chartId}.");
        }

        private DashboardChart GetDashboardChartFromCustomObjectList(MacroDataSource datasource, Guid chartId, ChartSchema chartSchema, ApplicationUser? loggedInUser, List<ControlValue> filterValues)
        {
            var dataObjects = SF.MicroAppContract.GetBaseSqlDataSource().GetCustomObjectList(datasource, loggedInUser, filterValues);
            var chart = new DashboardChart(chartSchema, dataObjects);
            return chart;
        }

        private DashboardChart GetDashboardChartFromSqlDataSource(MacroSqlDataSource datasource, Guid chartId, ChartSchema chartSchema, ApplicationUser? loggedInUser, List<ControlValue> filterValues)
        {
            var microSqlQuery = datasource.GetSqlQuery();
            if (microSqlQuery is null)
                throw new ValidationException($"Data source with id {chartSchema.DataSourceId} found without data query for chart {chartId}.");

            using (var db = new SqlCommandExecutor())
            {
                var param = SF.MicroAppContract.GetBaseSqlDataSource().GetQueryParameters(microSqlQuery, filterValues, loggedInUser, SF.MicroAppContract.GetBaseControl(), out string query);
                var dataTable = db.GetDataTable(query, param);
                var chart = new DashboardChart(chartSchema, dataTable);
                return chart;
            }
        }
    }
}