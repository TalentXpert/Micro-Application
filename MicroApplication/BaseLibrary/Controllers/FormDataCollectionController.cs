using BaseLibrary.Configurations.Models;

namespace BaseLibrary.Controllers
{
    /// <summary>
    /// This controller used to render a form that manages primary entity's  associated entities like user's roles,permissions etc.
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

        /// <summary>
        /// This API returns data to render form with list of items to select. 
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="entityId"></param>
        /// <returns>SelectFromListFormInput</returns>
        /// <exception cref="ValidationException"></exception>
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

        /// <summary>
        /// This API is used to save form data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
