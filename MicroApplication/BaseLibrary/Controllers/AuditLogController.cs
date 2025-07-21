namespace BaseLibrary.Controllers
{
    [Route("api/[controller]")]
    public class AuditLogController : BaseLibraryController
    {
        IAuditEventBaseService _auditEventService;

        public ILoggerFactory LoggerFactory { get; }

        public AuditLogController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<AppFormControlController>())
        {
            LoggerFactory = loggerFactory;
            _auditEventService = serviceFactory.AuditEventBaseService;
        }


        [HttpGet("get/{referenceObjectId}")]
        public IActionResult Get(string referenceObjectId)
        {
            try
            {
                GaurdForNullEmptyString(referenceObjectId, "Reference Object Id");

                var auditLogs = _auditEventService.GetAuditlogs(referenceObjectId);
                return Ok(auditLogs.Select(a => a.ToAuditLogVM(LoggedInUser.GetTimeZone())));
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo(), new { ReferenceObjectId = referenceObjectId });
            }
        }
    }
}
