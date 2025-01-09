using BaseLibrary.Controls.Charts;
using BaseLibrary.Domain.ComponentSchemas;
using BaseLibrary.Domain.DataSources;
using System.Data;

namespace BaseLibrary.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ChartBuilderController : BaseLibraryController
    {
        public ILoggerFactory LoggerFactory { get; }

        public ChartBuilderController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<ChartBuilderController>())
        {
            LoggerFactory = loggerFactory;
        }

        /// <summary>
        /// This API return all data sources
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDataSources")]
        public IActionResult GetDataSources()
        {
            IsInRole(ApplicationRole.OrganizationAdminRole, "View Data Sources");
            var sources = BSF.SqlDataSourceService.GetDatasources(SqlDataSourceType.Chart);
            return Ok(sources.Select(s => new { Id = s.Id, Name = s.Name }));
        }

        [HttpGet("GetChartTypes")]
        public IActionResult GetChartTypes()
        {
            var sources = ChartType.GetGraphTypes();
            return Ok(sources.Select(s => new { Name = s.Type }));
        }


        [HttpGet("GetDataSourceColumns/{id}")]
        public IActionResult GetDataSourceColumns(Guid id)
        {
            IsInRole(ApplicationRole.OrganizationAdminRole, "View Data Source Columns");
            var result = new List<ChartColumnSchema>();
            var dt = BSF.SqlDataSourceService.GetDataSourceColumns(id);
            foreach (DataColumn column in dt.Columns)
            {
                var chartColumnSchema = new ChartColumnSchema();
                chartColumnSchema.UpdateWith(column, BSF.MicroAppContract);
                result.Add(chartColumnSchema);
            }
            return Ok(result);
        }

        [HttpGet("GetCharts")]
        public IActionResult GetCharts()
        {
            try
            {
                var ds = BSF.ComponentSchemaService.GetComponents(ComponentTypes.Chart, LoggedInUser);
                return Ok(ds.Select(d => d.GetChartSchema()).ToList());
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpGet("GetChart/{id}")]
        public IActionResult GetChart(Guid id)
        {
            try
            {
                var ds = BSF.ComponentSchemaService.GetComponent(id);
                return Ok(ds.GetChartSchema());
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] ChartSchema vm)
        {
            try
            {
                IsInRole(ApplicationRole.OrganizationAdminRole, "Save or Update Chart");
                BSF.ComponentSchemaService.SaveUpdateChartSchema(vm, LoggedInUser.OrganizationId, LoggedInUser.Id);
                CommitTransaction();
                return Ok(true);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpPost("GetChartPreview")]
        public IActionResult GetChartPreview([FromBody] ChartSchema vm)
        {
            try
            {
                BSF.ChartService.GetChartPreview(vm, LoggedInUser.OrganizationId, LoggedInUser);
                CommitTransaction();
                return Ok(true);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }
    }
}
