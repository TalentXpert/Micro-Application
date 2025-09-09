namespace BaseLibrary.Configurations.FormHandlers
{
    public class ApplicationControlFormHandler : FormHandlerBase
    {
        public ApplicationControlFormHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser? loggedInUser)
            : base(serviceFactory, loggedInUser, BaseForm.ApplicationControlForm, null, null, false)
        {

        }

        public override void ProcessFormSaveRequest(SmartFormTemplateRequest model)
        {
            AppControlVM vm = new AppControlVM();
            if (IsNotNullOrEmpty(model.DataKey) && model.DataKey.HasValue)
                vm.Id = model.DataKey.Value;

            vm.ControlIdentifier = model.ControlValues.GetControlFirstValue(BaseControl.ControlIdentifier) ?? string.Empty;
            vm.DataType = model.ControlValues.GetControlFirstValue(BaseControl.ControlDataTypes) ?? string.Empty;
            vm.ControlType = model.ControlValues.GetControlFirstValue(BaseControl.ControlType) ?? string.Empty;
            vm.DisplayLabel = model.ControlValues.GetControlFirstValue(BaseControl.DisplayLabel) ?? string.Empty;
            vm.Options = model.ControlValues.GetControlFirstValue(BaseControl.Options);

            BaseLibraryServiceFactory.AppControlService.SaveUpdateAppControl(GetLoggedInUserOrganization(LoggedInUser), vm);
        }

        protected override List<ControlValue> GetFormControlValues(SmartFormGenerateRequest model)
        {
            var id = model.DataKey;
            AppControl? appControl = null;
            if (IsNotNullOrEmpty(id) && id.HasValue)
                appControl = BaseLibraryServiceFactory.AppControlService.GetAppControl(id.Value);

            var controlValues = new List<ControlValue>();
            if (IsNotNull(appControl))
            {
                // Fix CS8602: check for null before dereferencing appControl properties
                controlValues.Add(new ControlValue(BaseControl.ControlIdentifier, appControl?.ControlIdentifier ?? string.Empty));
                controlValues.Add(new ControlValue(BaseControl.ControlDataTypes, appControl?.DataType ?? string.Empty));
                controlValues.Add(new ControlValue(BaseControl.ControlType, appControl?.ControlType ?? string.Empty));
                controlValues.Add(new ControlValue(BaseControl.DisplayLabel, appControl?.DisplayLabel ?? string.Empty));
                controlValues.Add(new ControlValue(BaseControl.Options, appControl?.Options ?? string.Empty));
            }
            return controlValues;
        }

        public override void DeleteData(Guid id)
        {
            BaseLibraryServiceFactory.AppControlService.DeleteAppControl(id, GetLoggedInUser(LoggedInUser));
        }
    }
}
