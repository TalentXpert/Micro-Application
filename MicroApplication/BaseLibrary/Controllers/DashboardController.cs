using BaseLibrary.Controls.Dashboard;
using System.Data;

namespace BaseLibrary.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DashboardController : BaseLibraryController
    {
        public DashboardController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<DashboardController>())
        {
        }

        /// <summary>
        /// This method will be used by dashboard render to get information needed to render a dashboard.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDashboard/{id}")]
        public IActionResult GetDashboard(Guid id)
        {
            try
            {
                var ds = BSF.ComponentSchemaService.GetComponent(id);
                return Ok(ds.GetDashboardSchema());
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API return all data sources
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDataSources")]
        public IActionResult GetDataSources()
        {
            try
            {
                var dataSouces = BSF.MicroAppContract.GetDataSources();
                return Ok(dataSouces.Select(d => d.Name).ToList());
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API return page saved state so user should see last visited view of this page (CurrentGridFilterId,PageActions and GlobalControls)
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDataSourceColumns/{dataSource}")]
        public IActionResult GetDataSourceColumns(string dataSource)
        {
            try
            {
                var ds = BSF.MicroAppContract.GetDataSources().FirstOrDefault(d => d.Name == dataSource);
                if (ds == null)
                    throw new ValidationException($"No data source found with this name {dataSource}.");
                var controls = new List<AppControlVM>();
                foreach (var form in ds.Forms)
                {
                    var formControls = BSF.AppFormControlService.GetFormControlsWithControl(form.Id, null, LoggedInUser.OrganizationId);
                    controls.AddRange(formControls.Where(c => c.AppControl.DataType != ControlDataTypes.Guid).Select(c => new AppControlVM(c.AppControl)).ToList());
                }
                return Ok(controls);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API returns user saved filters and filter controls for given global control value if present 
        /// </summary>
        [HttpPost("SaveDashboardGrid")]
        public IActionResult SaveDashboardGrid([FromBody] SaveDashboardGridVM vm)
        {
            try
            {
                var pageData = vm.GetPageDataStore();
                BSF.PageDataStoreService.SaveDashboardDatum(pageData, LoggedInUser);
                return Ok(true);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpGet("GetDashboardChart/{chartId}")]
        public IActionResult GetDashboardChart(Guid chartId)
        {
            try
            {
                var chart = BSF.ChartService.GetChart(chartId, LoggedInUser.OrganizationId, LoggedInUser);
                if (chart != null && chart.SeriesData.Count != 0)
                    return Ok(chart);
                throw new ValidationException("No Chart with given id exits.");
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }
    }
}