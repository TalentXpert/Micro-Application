
namespace BaseLibrary.Services
{
    public interface IFormBuilderService
    {
        void SaveUpdateFixedFormControls(AppFormFixedControlAddUpdateVM model);
    }

    /// <summary>
    /// This service will create or update form for everyone with common controls visible to everyone. Organization can configure these form later on form configuration 
    /// </summary>
    public class FormBuilderService : ServiceLibraryBase, IFormBuilderService
    {
        public FormBuilderService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory)
        {

        }
        public void SaveUpdateFixedFormControls(AppFormFixedControlAddUpdateVM model)
        {

            var appForm = RF.AppFormRepository.Get(model.FormId);
            var formControls = RF.AppFormControlRepository.GetFormFixedControls(model.FormId);
            int position = 0;

            //Save or Update existing controls
            foreach (var control in model.AppFormControls)
            {
                position += 1;
                var existingControl = formControls.FirstOrDefault(c => c.AppControlId == control.AppControlId);
                if (IsNull(existingControl))
                {
                    var appControl = RF.AppControlRepository.Get(control.AppControlId);
                    existingControl = AppFormControl.Create(null, appForm, appControl, position);
                    RF.AppFormControlRepository.Add(existingControl);
                }
                existingControl?.UpdateFixedControl(position, control);
            }

            //Removed controls
            foreach (var control in formControls)
            {
                var existingControl = model.AppFormControls.FirstOrDefault(c => c.AppControlId == control.AppControlId);
                if (existingControl != null) continue;
                RF.AppFormControlRepository.Remove(control);
            }
        }
    }

    
 }
