using BaseLibrary.Configurations.FormHandlers;
using BaseLibrary.Configurations.PageHandlers;

namespace BaseLibrary.Configurations
{
    /// <summary>
    /// This class is used to get appropriate FormHandler or PageHandler based on formId or pageId. This class can be extended in application to add more handlers for custom forms or pages.
    /// </summary>
    public class BaseHandlerFactory
    {
        public IBaseLibraryServiceFactory BSF { get; }

        public BaseHandlerFactory(IBaseLibraryServiceFactory serviceFactory)
        {
            BSF = serviceFactory;
        }

        /// <summary>
        /// This method return form hadler for normal form
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="loggedInUser"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual FormHandlerBase GetFormHandler(Guid formId, ApplicationUser loggedInUser)
        {
            switch (formId.ToString().ToUpper())
            {
               
                case BaseForm.ApplicationControlFormId: return new ApplicationControlFormHandler(BSF, loggedInUser);
                
                case BaseForm.UserManagementFormId: return new UserManagementFormHandler(BSF, loggedInUser);
                case BaseForm.RolePermissionFormId: return new RolePermissionFormHandler(BSF, loggedInUser);
                case BaseForm.ManageOrganizationAdminFormId: return new OrganizationAdminFormHandler(BSF, loggedInUser);
                case BaseForm.ManageOrganizationFormId: return new OrganizationFormHandler(BSF, loggedInUser);
                //case BaseForm.AuditFormId:return new AuditPageHandler(BSF, loggedInUser);
                default:
                    var form = BSF.AppFormService.GetForm(formId);
                    if (form is null)
                        throw new ValidationException($"No application form found with {formId} id. Please use correct form id.");
                    var layoutControl = BSF.AppFormControlService.GetLayoutControl(formId);
                    return new DefaultFormHandler(BSF, loggedInUser, form, null, layoutControl?.AppControl, false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="loggedInUser"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual PageHandlerBase GetSmartPageHandler(Guid pageId, ApplicationUser loggedInUser)
        {
            switch (pageId.ToString().ToUpper())
            {
                case BasePage.UserManagementPageId: return new UserManagementPageHandler(BSF, loggedInUser);
               
                case BasePage.ApplicationControlPageId: return new ApplicationControlPageHandler(BSF, loggedInUser);
                case BasePage.ManageOrganizationAdminPageId: return new OrganizationAdminPageHandler(BSF, loggedInUser);
                case BasePage.ManageOrganizationPageId: return new OrganizationPageHandler(BSF, loggedInUser);
                case BaseForm.AuditFormId: return new AuditPageHandler(BSF, loggedInUser);
                default:
                    var appPage = BSF.AppPageService.GetPage(pageId);
                    if (appPage is null)
                        throw new ValidationException($"No application page found with {pageId} page id. Please use correct page id.");
                    var form = BSF.AppFormService.GetForm(pageId);
                    if (form is null)
                        throw new ValidationException($"No application page found with {pageId} page id. Please use correct page id.");
                    var layoutControl = BSF.AppFormControlService.GetLayoutControl(pageId);
                    return new DefaultPageHandler(BSF, loggedInUser, appPage, form, layoutControl?.AppControl);
            }
        }

        public virtual SelectFromListFormHandler GetSelectFromListFormHandler(Guid formId, ApplicationUser loggedInUser)
        {
            switch (formId.ToString().ToUpper())
            {
                case BaseForm.UserRoleFormId:
                    return new UserRoleSelectFromListFormHandler(BSF);
            }
            throw new ValidationException($"No form handler found for form id -{formId}");
        }

    }
}
