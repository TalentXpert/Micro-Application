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
            var controls = BSF.MicroAppContract.GetBaseControl().GetControls();
            var forms = BSF.MicroAppContract.GetBaseForm().GetFormsWithControls();
            var pages = BSF.MicroAppContract.GetBasePage().GetPages();
            BSF.SeederService.SeedApplicationForms(pages, controls, forms);

            var dataSources = BSF.MicroAppContract.GetBaseDataSource().GetSqlDataSources();
            BSF.SeederService.SeedDataSources(dataSources);

            var charts = BSF.MicroAppContract.GetBaseChart().GetCharts();
            BSF.SeederService.SeedCharts(charts);

            //var dashboards = BSF.MicroAppContract.GetBaseDashboard().GetDashboards();
            //BSF.SeederService.SeedDashboards(dashboards);
        }
        #endregion
    }
}
