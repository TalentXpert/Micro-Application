
using BaseLibrary.UI.Controls;

namespace BaseLibrary.Services
{
    public interface IAppFormControlService
    {
        void DeleteOrganizationConfiguredControl(Guid id, Guid organizationId);
        List<AppFormControl> GetFormControls(AppFormControlRequestVM model, ApplicationUser loggedInUser);
        void SaveUpdateOrganizationFormControls(AppFormControlAddUpdateVM model, ApplicationUser loggedInUser);
        List<AppFormControl> GetFixedFormControls(Guid formId);
        List<AppFormControl> GetFormControlsWithControl(Guid formId, Guid? layoutControlId, Guid? organizationId);
        List<AppFormControl> GetGlobalFormControlsWithControl(Guid formId);
        AppFormControl? GetAnyFixedControl(Guid appControlId);
        AppFormControl GetAppFormControl(Guid id);
        void RemoveAllOrganizationConfiguredControls(AppFormResetRequestVM model, ApplicationUser loggedInUser);
        List<UIControl> GetGlobalControls(Guid? organizationId, AppForm appForm, List<ControlValue> GlobalControls, UIControlBaseFactory factory);
        List<UIControl> GetUIControls(Guid? organizationId, AppForm appForm, List<ControlValue> GlobalControls, UIControlBaseFactory factory);
        AppFormControl? GetLayoutControl(Guid formId);
    }

    public class AppFormControlService : ServiceLibraryBase, IAppFormControlService
    {
        public AppFormControlService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<AppFormControlService>())
        {
        }
        public List<AppFormControl> GetFormControls(AppFormControlRequestVM model, ApplicationUser loggedInUser)
        {
            return RF.AppFormControlRepository.GetFormControls(model.FormId, model.GlobalFormValue, loggedInUser.OrganizationId);
        }

        public void SaveUpdateOrganizationFormControls(AppFormControlAddUpdateVM model, ApplicationUser loggedInUser)
        {
            var appForm = RF.AppFormRepository.Get(model.FormId);
            var formControls = RF.AppFormControlRepository.GetFormControls(model.FormId, model.GlobalControlValue, loggedInUser.OrganizationId);
            int position = formControls.Count(fc => fc.IsFixedControl());
            //Save or Update existing controls
            foreach (var control in model.AppFormControls)
            {
                position += 1;
                var existingControl = formControls.FirstOrDefault(c => c.AppControlId == control.AppControlId);
                if (IsNull(existingControl))
                {
                    var appControl = RF.AppControlRepository.Get(control.AppControlId);
                    existingControl = AppFormControl.Create(loggedInUser.OrganizationId, appForm, appControl, position);
                    existingControl.SetLayoutControlOrDefault(model.GlobalControlValue);
                    RF.AppFormControlRepository.Add(existingControl);
                }
                existingControl?.Update(position, control);
            }

            //Removed removed controls
            foreach (var control in formControls)
            {
                var existingControl = model.AppFormControls.FirstOrDefault(c => c.AppControlId == control.AppControlId);
                if (existingControl != null) continue;
                if (control.IsFixedControl()) continue;
                RF.AppFormControlRepository.Remove(control);
            }
        }

        public void RemoveAllOrganizationConfiguredControls(AppFormResetRequestVM model, ApplicationUser loggedInUser)
        {
            if (loggedInUser == null)
                throw new ValidationException("Login is required for this operation.");
            if(loggedInUser.OrganizationId == null)
                throw new ValidationException("Organization can not be null for this operation.");
            var formControls = RF.AppFormControlRepository.GetOrganizationConfiguredFormControls(model.FormId, model.LayoutControlValue, loggedInUser.OrganizationId.Value);
            foreach (var control in formControls)
            {
                RF.AppFormControlRepository.Remove(control);
            }
        }

        public void DeleteOrganizationConfiguredControl(Guid id, Guid organizationId)
        {
            var appFormControl = RF.AppFormControlRepository.Get(id);
            if (appFormControl == null)
                return;
            if (appFormControl.IsFixedControl()) return;
            if(appFormControl.OrganisationId==organizationId)
                RF.AppFormControlRepository.Remove(appFormControl);
        }

        public List<AppFormControl> GetFixedFormControls(Guid formId)
        {
            var controls = RF.AppFormControlRepository.GetFiltered(f => f.AppFormId == formId && f.LayoutControlId == null && f.OrganisationId == null).ToList();
            return controls;
        }

        public List<AppFormControl> GetFormControlsWithControl(Guid formId, Guid? layoutControlId, Guid? organizationId)
        {
            var controls = RF.AppFormControlRepository.GetFormControls(formId, layoutControlId, organizationId);
            return controls.OrderBy(c => c.Position).ToList();
        }
        public List<AppFormControl> GetGlobalFormControlsWithControl(Guid formId)
        {
            var controls = RF.AppFormControlRepository.GetGlobalFormControlsWithControl(formId);
            return controls.OrderBy(c => c.Position).ToList();
        }

        public AppFormControl? GetAnyFixedControl(Guid appControlId)
        {
            return RF.AppFormControlRepository.GetAnyFixedControl(appControlId);
        }

        public AppFormControl GetAppFormControl(Guid id)
        {
            return RF.AppFormControlRepository.Get(id);
        }
        public List<UIControl> GetGlobalControls(Guid? organizationId, AppForm appForm, List<ControlValue> GlobalControlVaues, UIControlBaseFactory factory)
        {
            if (GlobalControlVaues == null)
                GlobalControlVaues = new List<ControlValue>();

            List<UIControl> controls = new List<UIControl>();
            var globalControls = GetGlobalFormControlsWithControl(appForm.Id);
            if (HasNoChild(globalControls))
                return controls;

            globalControls = ReorderParentChild(globalControls);
            Guid? parentId = null;

            UIControl? smartControl = null;
            foreach (var control in globalControls)
            {
                var appControl = control.AppControl;
                if (control.GetIsGlobalControl() == false)
                    continue;
                var controlValues = ControlReader.GetControlValues(appControl, GlobalControlVaues);
                smartControl = factory.GetUIControl(organizationId, appControl, control, controlValues, parentId, false);
                controls.Add(smartControl);
                parentId = null;
                if (Guid.TryParse(smartControl.Value, out Guid parentIdGuid)) { parentId = parentIdGuid; }
            }
            if (IsNotNull(smartControl))
                smartControl?.SetPageRefreshNeeded();
            return controls;
        }
        private List<AppFormControl> ReorderParentChild(List<AppFormControl> globalControls)
        {
            List<AppFormControl> result = new List<AppFormControl>();
            if (HasNoChild(globalControls))
                return result;

            AppFormControl? parent = globalControls.FirstOrDefault(c => string.IsNullOrWhiteSpace(c.AppControl.ParentControlIdentifier));
            if (IsNull(parent))
                return result;

            result.Add(parent);
            var child = globalControls.FirstOrDefault(c => c.AppControl.ParentControlIdentifier == parent.AppControl.ControlIdentifier);
            while (IsNotNull(child))
            {
                result.Add(child);
                parent = child;
                child = globalControls.FirstOrDefault(c => c.AppControl.ParentControlIdentifier == parent.AppControl.ControlIdentifier);
            }

            return result;
        }



        public AppFormControl? GetLayoutControl(Guid formId)
        {
            return RF.AppFormControlRepository.GetLayoutControl(formId);
        }

        public List<UIControl> GetUIControls(Guid? organizationId, AppForm appForm, List<ControlValue> GlobalControls, UIControlBaseFactory factory)
        {
            throw new NotImplementedException();
        }
    }
}
