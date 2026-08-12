
using BaseLibrary.Configurations.Models;

namespace BaseLibrary.Configurations.FormHandlers
{
    public abstract class SelectFromListFormHandler 
    {
        public abstract SelectFromListFormInput GetInput(Guid formId, Guid formDataEntityId, ApplicationUser loggedInUser, string parentControlValue = "");        
        public abstract void SaveData(SelectFromListFormData data, ApplicationUser loggedInUser);
    }

    public abstract class FormTableFormHandler
    {
        public abstract FormViewTable GetTable(Guid formId, Guid formDataEntityId, ApplicationUser loggedInUser, string parentControlValue = "");
    }
}
