using BaseLibrary.Configurations.Models;

namespace BaseLibrary.Controllers
{
    /// <summary>
    /// This controller generate form template based on given input and also handle form post to collect data from form
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FormDataCollectionController : BaseLibraryController
    {
        public ILoggerFactory LoggerFactory { get; }

        public FormDataCollectionController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<AppFormControlController>())
        {
            LoggerFactory = loggerFactory;
        }


        #region SelectFromListForm
        [HttpGet("GetSelectFromListFormInput/{formId}/{entityId}")]
        public IActionResult GetSelectFromListFormInput(Guid formId, Guid entityId)
        {
            try
            {
                var formHandler = BSF.MicroAppContract.GetFormHandlerFactory().GetSelectFromListFormHandler(formId).GetInput(formId, entityId,LoggedInUser);
                if (formHandler == null)
                    throw new ValidationException($"Form input not found for formid {formId} and entity id {entityId}.");
                return Ok(formHandler);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpGet("GetSelectFromListFormInput/{formId}/{entityId}/{role}")]
        public IActionResult GetSelectFromListFormInput(Guid formId, Guid entityId,string role)
        {
            try
            {
                var formHandler = BSF.MicroAppContract.GetFormHandlerFactory().GetSelectFromListFormHandler(formId).GetInput(formId, entityId, LoggedInUser, role);
                if (formHandler == null)
                    throw new ValidationException($"Form input not found for formid {formId} and entity id {entityId}.");
                return Ok(formHandler);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpPost("SaveSelectFromListForm")]
        public IActionResult SaveSelectFromListForm([FromBody] SelectFromListFormData model)
        {
            try
            {
                BSF.MicroAppContract.GetFormHandlerFactory().GetSelectFromListFormHandler(model.FormId).SaveData(model, LoggedInUser);
                CommitTransaction();
                return Ok(true);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        #endregion

        #region DynamicForm

        #endregion
    }
}
