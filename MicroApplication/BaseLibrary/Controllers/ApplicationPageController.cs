using BaseLibrary.Configurations.PageHandlers;
using BaseLibrary.Utilities.Excels;
using System.Data;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace BaseLibrary.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]

    public class DataViewController : BaseLibraryController
    {
        public ILoggerFactory LoggerFactory { get; }

        public DataViewController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<ApplicationPageController>())
        {
            LoggerFactory = loggerFactory;
        }
    }
        [Route("api/[controller]")]
    [Produces("application/json")]

    public class ApplicationPageController : BaseLibraryController
    {
        public ILoggerFactory LoggerFactory { get; }
        private string? _studyDocumentsPath { get; set; }

        public ApplicationPageController(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<ApplicationPageController>())
        {
            LoggerFactory = loggerFactory;
            _studyDocumentsPath = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("FileUploadSettings")["StudyDocumentsPath"];
        }

        /// <summary>
        /// This returns page/dashboard handler for each.
        /// </summary>
        private PageHandlerBase GetSmartPageHandler(Guid pageId)
        {
            return BSF.MicroAppContract.GetHandlerFactory().GetSmartPageHandler(pageId, LoggedInUser);
        }

        /// <summary>
        /// This returns form handler for each form
        /// </summary>
        private FormHandlerBase GetFormHandler(Guid formId)
        {
            var loggedInUser = GetSafeCurrentUser();
            return BSF.MicroAppContract.GetHandlerFactory().GetFormHandler(formId, loggedInUser);
        }

        /// <summary>
        /// This API return page saved state so user should see last visited view of this page (CurrentGridFilterId,PageActions and GlobalControls)
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        [HttpGet("GetSmartPage/{pageId}")]
        public IActionResult GetSmartPage(Guid pageId)
        {
            try
            {
                var pageHandler = GetSmartPageHandler(pageId);
                var smartPage = pageHandler.GetSmartPage();
                return Ok(smartPage);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        #region Page Grid 

        /// <summary>
        /// This API returns user saved filters and filter controls for given global control value if present 
        /// </summary>
        [HttpPost("SmartGridConfiguration")]
        public IActionResult SmartGridConfiguration([FromBody] SmartGridConfigurationVM vm)
        {
            try
            {
                var pageHandler = GetSmartPageHandler(vm.PageId);
                var smartPage = pageHandler.GetSmartGridConfiguration(vm);
                return Ok(smartPage);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API return grid data base on request model 
        /// </summary>
        [HttpPost("ProcessPageDataRequest")]
        public IActionResult ProcessPageDataRequest([FromBody] GridRequestVM model)
        {
            try
            {
                var pageHandler = GetSmartPageHandler(model.PageId);
                BaseLibraryServiceFactory.UserSettingService.UpdateSmartPageState(model.PageId, LoggedInUser, null, model.PageSize);
                var gridModel = pageHandler.ProcessPageDataRequest(model);
                return Ok(gridModel);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API return grid data base on given filter id 
        /// </summary>
        [HttpGet("GetFilteredData/{pageId}/{filterId}/{pageSize}")]
        public IActionResult GetFilteredData(Guid pageId, Guid filterId, int pageSize)
        {
            try
            {
                var pageHandler = GetSmartPageHandler(pageId);
                BaseLibraryServiceFactory.UserSettingService.UpdateSmartPageState(pageId, LoggedInUser, filterId, pageSize);
                var gridModel = pageHandler.ProcessPageDataFilterRequest(filterId, pageSize);
                return Ok(gridModel);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API return grid data base on given row id 
        /// </summary>
        [HttpGet("ProcessRowDataRequest/{pageId}/{datakey}")]
        public IActionResult ProcessRowDataRequest(Guid pageId, Guid datakey)
        {
            try
            {
                var pageHandler = GetSmartPageHandler(pageId);                
                var gridModel = pageHandler.ProcessRowDataRequest(datakey);
                return Ok(gridModel);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API return grid data base on request model 
        /// </summary>
        [HttpPost("Export")]
        public IActionResult Export([FromBody] GridRequestVM model)
        {
            try
            {
                var pageHandler = GetSmartPageHandler(model.PageId);
                BaseLibraryServiceFactory.UserSettingService.UpdateSmartPageState(model.PageId, LoggedInUser, null, model.PageSize);
                var gridModel = pageHandler.ProcessPageDataRequest(model);
                var ignoreExcelHeaders = pageHandler.GetIgnoreExcelHeaders();
                var excel = new ExcelExporter().GetExcel(gridModel, "mm/dd/yyyy", ignoreExcelHeaders);
                var fileName = GetSafeDownloadFileName($"{pageHandler.AppPage.Name}.xlsx");
                Response.Headers.Add("X-FileName", fileName);
                Response.Headers.Add("Access-Control-Expose-Headers", "Access-Control-Expose-Headers:X-FileName");
                return File(excel, "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }
        private string GetSafeDownloadFileName(string input)
        {
            var safeFileName = string.Empty;
            foreach (var c in input)
            {
                if (char.IsLetter(c) || char.IsDigit(c) || c == '.')
                    safeFileName += c.ToString();
            }
            return safeFileName;
        }
        #endregion

        #region Filter and Header Configuration


        /// <summary>
        /// This API save user filter 
        /// </summary>
        [HttpPost("SaveFilter")]
        public IActionResult SaveFilter([FromBody] UserGridFilter model)
        {
            try
            {
                var filter = BaseLibraryServiceFactory.UserSettingService.SaveFilter(model, LoggedInUser);
                CommitTransaction();
                return Ok(filter);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API delete user filter
        /// </summary>
        [HttpDelete("DeleteFilter/{filterId}")]
        public IActionResult DeleteFilter(Guid filterId)
        {
            try
            {
                BaseLibraryServiceFactory.UserSettingService.DeleteFilter(filterId);
                CommitTransaction();
                return Ok(true);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API return user configured headers
        /// </summary>
        [HttpPost("GetUserHeaders")]
        public IActionResult GetUserHeaders([FromBody] SmartGridConfigurationVM vm)
        {
            try
            {
                var pageHandler = GetSmartPageHandler(vm.PageId);
                var userSavedHeaders = BaseLibraryServiceFactory.UserSettingService.GetUserConfiguredGridHeaders(LoggedInUser.Id, vm.PageId).OrderBy(h => h.Position);
                var allHeaders = pageHandler.GetGridHeaders(vm, true);

                var result = new UserGridHeadersVM { PageId = vm.PageId };
                int position = 2; // first position is fixed for Id column which is always hidden

                //first add all user selected headers
                foreach (var savedHeader in userSavedHeaders)
                {
                    if (savedHeader != null && string.IsNullOrWhiteSpace(savedHeader.HeaderIdentifier) == false)
                    {
                        var header = allHeaders.FirstOrDefault(h => h.HeaderIdentifier == savedHeader.HeaderIdentifier);
                        if (header != null)
                        {
                            result.Headers.Add(new UserGridHeaderVM(savedHeader.HeaderIdentifier, position, header.HeaderText, true, header.IsMandatory()));
                            position++;
                            allHeaders.Remove(header);
                        }
                    }
                }

                //add remaining headers 
                foreach (var header in allHeaders)
                {
                    if (header.IsVisible == false)
                        continue;
                    if (result.Headers.Any(h => h.HeaderIdentifier == header.HeaderIdentifier))
                        continue;
                    result.Headers.Add(new UserGridHeaderVM(header.HeaderIdentifier, position, header.HeaderText, false, false));
                    position++;
                }

                return Ok(result);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API save user selection of headers
        /// </summary>
        [HttpPost("SaveUserHeaders")]
        public IActionResult SaveUserHeaders([FromBody] UserGridHeadersVM model)
        {
            try
            {
                var pageHandler = GetSmartPageHandler(model.PageId);
                var headers = model.Headers.Where(h => h.IsVisible == true).Select(h => new UserGridHeader { HeaderIdentifier = h.HeaderIdentifier, Position = h.Position }).ToList();
                BaseLibraryServiceFactory.UserSettingService.SaveGridHeaders(model.PageId, headers, LoggedInUser);
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

        #region Control Selection Data
        /// <summary>
        /// This API return search option for typeahead control 
        /// </summary>
        [HttpGet("GetSearchOptions/{controlId}/{parentId}/{searchTerm}")]
        public IActionResult GetSearchOptions(Guid controlId, Guid parentId, string searchTerm)
        {
            try
            {
                Guid? parentControlValue = null;
                if (C.IsNotNullOrEmpty(parentId))
                    parentControlValue = parentId;
                var appControl = BSF.AppControlService.GetAppControl(controlId);
                if (appControl == null)
                    throw new ValidationException($"Control with {controlId} is not found.");
                if (appControl.OptionFormId == null)
                    throw new ValidationException("OptionFormId cannot be null for this control.");
                var options = BSF.MicroAppContract.GetHandlerFactory().GetFormHandler(appControl.OptionFormId.Value, LoggedInUser).GetControlOptions(GetSafeCurrentUser()?.OrganizationId, parentId, searchTerm); 
                return Ok(options);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API return record of a table based on parentId for dropdown kind of control 
        /// </summary>
        [HttpGet("GetChildren/{controlId}/{parentId}")]
        public IActionResult GetChildren(Guid controlId, Guid parentId)
        {
            try
            {
                var factory = BSF.ApplicationControlBaseFactory;
                var appControl = BSF.AppControlService.GetAppControl(controlId);
                var appFormControl = BSF.AppFormControlService.GetAnyFixedControl(controlId);
                var emptyEntry = true;
                if (appControl.IsGlobalControl || appFormControl.IsMandatory)
                    emptyEntry = false;
                var control = factory.GetUIControl(LoggedInUser.OrganizationId, appControl, appFormControl, null, parentId, emptyEntry);
                List<SmartControlOption> options = new List<SmartControlOption>();
                if (control != null)
                    options = control.Options;
                return Ok(options);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        #endregion

        #region Page Form 
        /// <summary>
        /// This API return page template with all control need to render a page for data display or add or edit 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ProcessFormGenerateRequest")]
        public IActionResult ProcessFormGenerateRequest([FromBody] SmartFormGenerateRequest model)
        {
            try
            {
                var factory = BSF.ApplicationControlBaseFactory;
                var pageHandler = GetFormHandler(model.FormId);
                var form = pageHandler.ProcessFormGenerateRequest(model, factory);
                return Ok(form);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API save page data into PageDataStore or derived storage. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ProcessFormSaveRequest")]
        public IActionResult ProcessFormSaveRequest([FromBody] SmartFormTemplateRequest model)
        {
            try
            {
                var pageHandler = GetFormHandler(model.FormId);
                pageHandler.ProcessFormSaveRequest(model);
                CommitTransaction();
                return Ok(true);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API save page data into PageDataStore or derived storage. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ProcessFormConsentRequest")]
        public IActionResult ProcessFormConsentRequest([FromBody] SmartFormConsentTemplateRequest model)
        {
            try
            {
                var pageHandler = GetFormHandler(model.FormId);
                pageHandler.ProcessFormConsentRequest(model);                
                CommitTransaction();
                return Ok(true);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        /// <summary>
        /// This API delete user filter
        /// </summary>
        [HttpDelete("Delete/{formId}/{id}")]
        public IActionResult Delete(Guid formId, Guid id)
        {
            try
            {
                var pageHandler = GetFormHandler(formId);
                pageHandler.DeleteData(id);
                CommitTransaction();
                return Ok(true);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }


        [HttpPost("UploadFile")]
        public ActionResult<bool> UploadFile([FromForm] FileUploadViewModel fileUploadViewModel)
        {
            try
            {
                if (fileUploadViewModel.UploadFiles != null && fileUploadViewModel.UploadFiles.Any())
                {
                    if (fileUploadViewModel.ControlId == null ||
                        fileUploadViewModel.ControlId.Count != fileUploadViewModel.UploadFiles.Count)
                    {
                        return BadRequest("Mismatch between the number of uploaded files and control IDs.");
                    }
                    for (int i = 0; i < fileUploadViewModel.UploadFiles.Count; i++)
                    {
                        var file = fileUploadViewModel.UploadFiles[i];
                        var controlId = fileUploadViewModel.ControlId[i];
                        string uniqueFileName = $"{controlId}_{file.FileName}";
                        string fileNameWithPath = Path.Combine(_studyDocumentsPath, uniqueFileName);
                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }

                return Ok(true);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpDelete("DeleteFile/{formId}/{controlId}/{fileName}")]
        public ActionResult<bool> DeleteFile(Guid formId, Guid controlId,string fileName)
        {
            try
            {
                var filePath = string.Concat(Path.Combine(_studyDocumentsPath, fileName));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                return Ok(true);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        #endregion

        #region Page View 
        /// <summary>
        /// This API return page saved state so user should see last visited view of this page (CurrentGridFilterId,PageActions and GlobalControls)
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        [HttpGet("GetViewPageContents/{formId}/{id}")]
        public IActionResult GetViewPageContents(Guid formId, Guid id)
        {
            try
            {
                var pageHandler = GetFormHandler(formId);
                var smartPage = pageHandler.GetPageContentView(new SmartFormGenerateRequest { DataKey = id, FormMode = SmartActionFormMode.View.Name, FormId = formId, GlobalControls = new List<ControlValue>() });
                return Ok(smartPage);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }
        #endregion

        #region Menus

        [HttpGet("GetMenus")]
        public IActionResult GetMenus()
        {
            try
            {
                var pages = BSF.RF.AppPageRepository.GetAll().ToList();
                var permissions = BSF.UserRoleService.GetUserAllPermissions(LoggedInUser);
                var result = BSF.MicroAppContract.GetApplicationMenu().GetMenus(pages, permissions);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        #endregion

        #region Excel Import

        [HttpPost("GetImportTemplateRequest")]
        public IActionResult GetImportTemplateRequest([FromBody] ExcelImportTmplateRequest model)
        {
            try
            {
                var factory = BSF.ApplicationControlBaseFactory;
                var pageHandler = GetFormHandler(model.FormId);
                var excel = pageHandler.GetImportTemplateRequest(model, factory);
                var fileName = GetSafeDownloadFileName($"{pageHandler.Form.Name}.xlsx");
                Response.Headers.Add("X-FileName", fileName);
                Response.Headers.Add("Access-Control-Expose-Headers", "Access-Control-Expose-Headers:X-FileName");
                return File(excel, "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpPost("ImportDataFromExcel")]
        public IActionResult ImportDataFromExcel(ExcelImportRequest model)
        {
            try
            {
                var factory = BSF.ApplicationControlBaseFactory;
                var pageHandler = GetFormHandler(model.FormId);
               // var form = pageHandler.ImportDataFromExcel(model, factory, LoggedInUser);
                return Ok(true);
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        #endregion

    }
}

public class FileUploadViewModel
{
    public List<IFormFile> UploadFiles { get; set; }
    public List<string> ControlId { get; set; }
}