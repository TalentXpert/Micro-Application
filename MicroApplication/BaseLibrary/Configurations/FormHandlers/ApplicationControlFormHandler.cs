namespace BaseLibrary.Configurations.FormHandlers
{
    public class ApplicationControlFormHandler : FormHandlerBase
    {
        public ApplicationControlFormHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser loggedInUser)
            : base(serviceFactory, loggedInUser, BaseForm.ApplicationControlForm, null, null, false)
        {

        }

        public override void ProcessFormSaveRequest(SmartFormTemplateRequest model)
        {
            AppControlVM vm = new AppControlVM();
            if (IsNotNullOrEmpty(model.DataKey))
                vm.Id = model.DataKey.Value;

            vm.ControlIdentifier = model.ControlValues.GetControlFirstValue(BaseControl.ControlIdentifier);
            vm.DataType = model.ControlValues.GetControlFirstValue(BaseControl.ControlDataTypes);
            vm.ControlType = model.ControlValues.GetControlFirstValue(BaseControl.ControlType);
            vm.DisplayLabel = model.ControlValues.GetControlFirstValue(BaseControl.DisplayLabel);
            vm.Options = model.ControlValues.GetControlFirstValue(BaseControl.Options);
            BaseLibraryServiceFactory.AppControlService.SaveUpdateAppControl(GetLoggedInUserOrganization(LoggedInUser), vm);
        }

        protected override List<ControlValue> GetFormControlValues(SmartFormGenerateRequest model)
        {
            var id = model.DataKey;
            AppControl? appControl = null;
            if (IsNotNullOrEmpty(id))
                appControl = BaseLibraryServiceFactory.AppControlService.GetAppControl(id.Value);

            var controlValues = new List<ControlValue>();
            if (IsNotNull(appControl))
            {
                controlValues.Add(new ControlValue(BaseControl.ControlIdentifier, appControl.ControlIdentifier));
                controlValues.Add(new ControlValue(BaseControl.ControlDataTypes, appControl.DataType));
                controlValues.Add(new ControlValue(BaseControl.ControlType, appControl.ControlType));
                controlValues.Add(new ControlValue(BaseControl.DisplayLabel, appControl.DisplayLabel));
                controlValues.Add(new ControlValue(BaseControl.Options, appControl.Options));
            }
            return controlValues;
        }

        public override void DeleteData(Guid id)
        {
            BaseLibraryServiceFactory.AppControlService.DeleteAppControl(id, GetLoggedInUser(LoggedInUser));
        }
    }
}
