namespace BaseLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class WebsiteAdminController : BaseLibraryController
    {
        public WebsiteAdminController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<UserRoleController>())
        {

        }

        #region UpdateApplicationConfiguration
        [HttpPatch("UpdateApplicationConfiguration")]
        public IActionResult UpdateApplicationConfiguration()
        {
            try
            {
                //IsInWebsiteAdminRole("UpdateApplicationConfiguration");
                SeedApplicaitonData();
                CommitTransaction();
                return Ok(true);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }
        private void SeedApplicaitonData()
        {
            var controls = BSF.MicroAppContract.GetApplicationControls();
            var forms = BSF.MicroAppContract.GetApplicationFormsWithControls();
            var pages = BSF.MicroAppContract.GetApplicationPages();
            BSF.SeederService.SeedApplicationForms(pages, controls, forms);

            var dataSources = BSF.MicroAppContract.GetSqlDataSources();
            BSF.SeederService.SeedDataSources(dataSources);

            var charts = BSF.MicroAppContract.GetApplicationCharts();
            BSF.SeederService.SeedCharts(charts);

            var dashboards = BSF.MicroAppContract.GetApplicationDashboards();
            BSF.SeederService.SeedDashboards(dashboards);
        }
        #endregion
    }
}
