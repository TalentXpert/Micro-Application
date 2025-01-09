namespace BaseLibrary.Controllers
{
    /// <summary>
    /// This controller allow organization admin to configure a form by adding more controls on top of common fixed controls
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AppFormControlController : BaseLibraryController
    {
        public ILoggerFactory LoggerFactory { get; }

        public AppFormControlController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory,loggerFactory.CreateLogger<AppFormControlController>())
        {
            LoggerFactory = loggerFactory;
        }

        /// <summary>
        /// This api return all costomizable forms 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllForms")]
        [HttpGet("GetAllCustomizableForms")]
        public IActionResult GetAllCustomizableForms()
        {
            try
            {
                var formList = BSF.FormConfigurationService.GetAllCustomizableForms();
                return Ok(formList);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API return all control of an organisation 
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        [HttpGet("GetControls")]
        public IActionResult GetControls()
        {
            try
            {
                List<AppControl> controls = BSF.AppControlService.GetOrganisationControls(LoggedInUser);
                controls = controls.Where(c => c.OrganisationId != null).ToList();
                return Ok(controls.Select(c => new AppControlVM(c)));
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API return all 
        /// </summary>
        [HttpGet("GetGlobalControls/{formId}")]
        public IActionResult GetGlobalControls(Guid formId)
        {
            try
            {
                var form = BSF.AppFormService.GetForm(formId);
                var controls = BSF.AppFormControlService.GetGlobalControls(LoggedInUser.OrganizationId, form, null, BaseLibraryServiceFactory.ApplicationControlBaseFactory);
                return Ok(controls);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API return all controls of a form 
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        [HttpPost("GetFormControls")]
        public IActionResult GetFormControls([FromBody] AppFormControlRequestVM model)
        {
            try
            {
                List<AppFormControl> controls = BSF.AppFormControlService.GetFormControls(model, LoggedInUser);
                return Ok(controls.Select(c => new AppFormControlListVM(c, c.AppControl)));
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpPost("Post")]
        public IActionResult Post([FromBody] AppFormControlAddUpdateVM model)
        {
            try
            {
                BSF.AppFormControlService.SaveUpdateOrganizationFormControls(model, LoggedInUser);
                CommitTransaction();
                return Ok(true);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpPut("RestForm")]
        public IActionResult RestForm([FromBody] AppFormResetRequestVM model)
        {
            try
            {
                BSF.AppFormControlService.RemoveAllOrganizationConfiguredControls(model, LoggedInUser);
                CommitTransaction();
                return Ok(true);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This api allow an organization admin to delete form control by the organisation
        /// </summary>
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                IsInOrganizationAdminRole("Delete Form Control");
                BSF.AppFormControlService.DeleteOrganizationConfiguredControl(id,LoggedInUser.GetOrganizationId());
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
