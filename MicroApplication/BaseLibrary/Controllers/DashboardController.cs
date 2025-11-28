using BaseLibrary.Controls;
using BaseLibrary.Controls.Dashboard;
using BaseLibrary.Domain;
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
                var dashboard = BSF.ComponentSchemaService.GetDashboardSchema(id);
                var parameters = BSF.ComponentSchemaService.GetMicroSqlQueryParameters(dashboard);
                var factory = BaseLibraryServiceFactory.ApplicationControlBaseFactory;
                
                // Generating filter controls based on parameters
                foreach (var parameter in parameters)
                {
                    var appControl = BSF.MicroAppContract.GetBaseControl().GetAppControlByParameter(parameter.Name);
                    if (appControl is null)
                        continue;
                    var smartControl = factory.GetFilterUIControl(LoggedInUser, appControl, null, null, parameter.IsMandatory);
                    if(smartControl != null)
                        dashboard.Filters.Add(smartControl);
                }

                return Ok(dashboard);
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
                var dataSouces = BSF.MicroAppContract.GetBaseDataSource().GetAllDataSources();
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
                var ds = BSF.MicroAppContract.GetBaseDataSource().GetAllDataSources().FirstOrDefault(d => d.Name == dataSource);
                if (ds == null)
                    throw new ValidationException($"No data source found with this name {dataSource}.");
                var controls = new List<AppControlVM>();
                //foreach (var form in ds.Forms)
                //{
                //    var formControls = BSF.AppFormControlService.GetFormControlsWithControl(form.Id, null, LoggedInUser.OrganizationId);
                //    controls.AddRange(formControls.Where(c => c.AppControl.DataType != ControlDataTypes.Guid).Select(c => new AppControlVM(c.AppControl)).ToList());
                //}
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

        /// <summary>
        /// This method will be used by dashboard render to get information needed to render a chart.
        /// </summary>
        /// <param name="chartId"></param>
        /// <param name="studyId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [HttpGet("GetDashboardChart/{chartId}/{studyId}")]
        public IActionResult GetDashboardChart(Guid chartId, string studyId)
        {
            try
            {
                var filter = new ControlValue(Guid.Parse("BD16339F-88DD-49E3-9CEB-09D65AAE38DA"), "Study",studyId);
                
                var model = new GetDashboardChartInputVM { ChartId = chartId, FilterValues = new List<ControlValue>() { filter } };
                var chart = BSF.ChartService.GetChart(model, LoggedInUser);
                if (chart != null && chart.SeriesData.Count != 0)
                    return Ok(chart);
                throw new ValidationException("No Chart with given id exits.");
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpPost("GetDashboardChart")]
        public IActionResult GetDashboardChart([FromBody] GetDashboardChartInputVM model)
        {
            try
            {
                var chart = BSF.ChartService.GetChart(model, LoggedInUser);
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
    public class GetDashboardChartInputVM
    {
        public Guid ChartId { get; set; }
        public string? GlobalFilterId { get; set; }
        public List<ControlValue> FilterValues { get; set; } = [];
    }
}