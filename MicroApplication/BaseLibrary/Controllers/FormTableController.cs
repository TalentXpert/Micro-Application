namespace BaseLibrary.Controllers
{
    /// <summary>
    /// This controller used to render a form that manages primary entity's  associated entities like user's roles,permissions etc.
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FormTableController : BaseLibraryController
    {
        public ILoggerFactory LoggerFactory { get; }

        public FormTableController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<AppFormControlController>())
        {
            LoggerFactory = loggerFactory;
        }

        [HttpGet("GetFromDetailTable/{formId}/{entityId}")]
        public IActionResult GetFromDetailTable(Guid formId, Guid entityId)
        {
            try
            {
                var formHandler = BSF.MicroAppContract.GetFormHandlerFactory().GetFormTableFormHandler(formId,LoggedInUser).GetTable(formId, entityId, LoggedInUser);
                if (formHandler == null)
                    throw new ValidationException($"Form input not found for formid {formId} and entity id {entityId}.");
                return Ok(formHandler);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }
    }
}
