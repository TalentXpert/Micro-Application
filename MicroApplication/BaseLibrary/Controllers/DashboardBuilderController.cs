using BaseLibrary.Domain.ComponentSchemas;
using System.Data;

namespace BaseLibrary.Controllers
{
    /// <summary>
    /// This class will be used by only dashboard builder
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DashboardBuilderController : BaseLibraryController
    {
        public ILoggerFactory LoggerFactory { get; }

        public DashboardBuilderController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<DashboardBuilderController>())
        {
            LoggerFactory = loggerFactory;
        }

        [HttpGet("GetContentTypes")]
        public IActionResult GetContentTypes()
        {
            try
            {
                var ds = DashboardPanelContentType.ContentTypes;
                return Ok(ds);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpGet("GetContents/{contentType}")]
        public IActionResult GetContents(string contentType)
        {
            try
            {
                List<ComponentSchema> components = new List<ComponentSchema>();
                if(DashboardPanelContentType.Graph.ContentType== contentType)
                    components = BSF.ComponentSchemaService.GetComponents(ComponentTypes.Chart, LoggedInUser);
                return Ok(components.Select(s => new { Id = s.Id, Name = s.Name }));
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpGet("GetTopMenu")]
        public IActionResult GetTopMenu()
        {
            return Ok(BSF.MicroAppContract.GetApplicationMenu().GetFixedTopMenus());
        }

        /// <summary>
        /// This API return all data sources
        /// </summary>
        /// <returns></returns>
        [HttpGet("Gets")]
        public IActionResult Gets()
        {
            try
            {
                var ds = BSF.ComponentSchemaService.GetComponents(ComponentTypes.Dashboard, LoggedInUser);
                return Ok(ds.Select(d=>d.GetDashboardSchema()).ToList());
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }
        /// <summary>
        /// This method will be used by dashboard builder to get information needed to build a dashboard.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var ds = BSF.ComponentSchemaService.GetDashboard(id);
                if (ds == null)
                    throw new ValidationException($"No dashboard found with given id {id}.");
                return Ok(ds);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API returns user saved filters and filter controls for given global control value if present 
        /// </summary>
        [HttpPost]
        public IActionResult Post([FromBody] DashboardSchema vm)
        {
            try
            {
                //IsInRole(ApplicationRole.OrganizationAdminRole, "Save Or Update Dashboard");
                var dc = BSF.ComponentSchemaService.SaveUpdateDashboardSchema(vm, LoggedInUser.OrganizationId,LoggedInUser.Id);
                CommitTransaction();
                return Ok(dc);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

    }

  
}