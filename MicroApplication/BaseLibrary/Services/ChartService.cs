
using BaseLibrary.Controllers;
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
            var datasource = SF.MicroAppContract.GetSqlDataSources().FirstOrDefault(ds => ds.Id == chartSchema.DataSourceId);
            if (datasource is null)
                datasource = SF.RF.SqlDataSourceRepository.Get(chartSchema.DataSourceId);
            if (datasource is null)
                throw new ValidationException($"Data source with id {chartSchema.DataSourceId} not found for chart {chartId}.");
            var microSqlQuery = datasource.GetSqlQuery();
            if (microSqlQuery is null)
                throw new ValidationException($"Data source with id {chartSchema.DataSourceId} found without data query for chart {chartId}.");
            
            using (var db = new SqlCommandExecutor())
            {
                var param = SF.MicroAppContract.GetBaseSqlDataSource().GetQueryParameters(microSqlQuery, filterValues, loggedInUser, SF.MicroAppContract.GetBaseControl(), out string  query);
                var dataTable = db.GetDataTable(query, param);
                var chart = new DashboardChart(chartSchema, dataTable);
                return chart;
            }
        }
    }
}