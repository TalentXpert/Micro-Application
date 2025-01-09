using BaseLibrary.Configurations;
using BaseLibrary.Controls.Menus;

namespace BaseLibrary.Controllers
{
    public class PageBuilderInfoVM
    {
        public List<AppFormList> Pages { get; set; }
        public List<AppControlVM> Controls { get; set; }
        public List<Menu> TopMenus { get; set; }
    }

    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PageBuilderController : BaseLibraryController
    {
        public ILoggerFactory LoggerFactory { get; }
        public IAppFormControlService AppFormControlService { get; }

        public PageBuilderController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<PageBuilderController>())
        {
            LoggerFactory = loggerFactory;
            AppFormControlService = BSF.AppFormControlService;
        }

        [HttpGet("GetPageInfo")]
        public IActionResult GetPageInfo()
        {
            try
            {
                var result = new PageBuilderInfoVM();
                result.Pages = BSF.FormConfigurationService.GetAllCustomizableForms();
                List<AppControl> controls = BSF.AppControlService.GetAllFixedControls();
                result.Controls = controls.Select(c => new AppControlVM(c)).ToList();
                result.TopMenus = BSF.MicroAppContract.GetApplicationMenu().GetFixedTopMenus();
                return Ok(result);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API return all fixed controls
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        [HttpGet("GetFormFixedControls/{formId}")]
        public IActionResult GetFormFixedControls(Guid formId)
        {
            try
            {
                //IsOperationAllowed(BasePermission.ManageRoles.Code, "GetFormFixedControls");
                List<AppFormControl> controls = AppFormControlService.GetFixedFormControls(formId);
                return Ok(controls.Select(c => new AppFormControlListVM(c, c.AppControl)));
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpPost("AddForm")]
        public IActionResult AddForm([FromBody] AppPageSaveUpdateVM model)
        {
            try
            {
                var page = BSF.AppPageService.SaveUpdatePageAndForm(model);
                var appFormList = new AppFormList(page);
                CommitTransaction();
                return Ok(appFormList);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpPost("Post")]
        public IActionResult Post([FromBody] AppFormFixedControlAddUpdateVM model)
        {
            try
            {
                BSF.FormBuilderService.SaveUpdateFixedFormControls(model);
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