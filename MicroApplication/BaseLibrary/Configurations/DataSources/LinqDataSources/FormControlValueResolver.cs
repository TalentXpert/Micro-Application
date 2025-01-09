namespace BaseLibrary.Configurations.DataSources.LinqDataSources
{
    public interface IFormControlValueResolver
    {
        string? GetFormControlValue(AppForm form, AppControl control);
    }

    /// <summary>
    /// This call will find a control value from provided formControlValueDataSource object.
    /// </summary>
    public class FormControlValueResolver : IFormControlValueResolver
    {
        public BaseLinqFormDataSource FormControlValueDataSource { get; }
        public FormControlValueResolver(BaseLinqFormDataSource formControlValueDataSource)
        {
            FormControlValueDataSource = formControlValueDataSource;
        }
        public virtual string? GetFormControlValue(AppForm form, AppControl control)
        {
            switch (form.Id.ToString())
            {
                case BaseForm.ManageOrganizationAdminFormId:
                case BaseForm.UserManagementFormId:
                    return FormControlValueDataSource.User?.GetValue(control.ControlIdentifier);
                case BaseForm.RolePermissionFormId:
                    return FormControlValueDataSource.ApplicationRolePermission?.GetValue(control.ControlIdentifier);
                case BaseForm.ApplicationControlFormId:
                    return FormControlValueDataSource.Control?.GetValue(control.ControlIdentifier);
                case BaseForm.UserRoleFormId:
                    return FormControlValueDataSource.UserRole?.GetValue(control.ControlIdentifier);
                default:
                    if (control.ControlIdentifier == BaseControl.Name.ControlIdentifier)
                        return FormControlValueDataSource.DataStore?.Name;
                    if (control.ControlIdentifier == BaseControl.Description.ControlIdentifier)
                        return FormControlValueDataSource.DataStore?.Description;
                    return FormControlValueDataSource.DataStore?.GetValue(control.ControlIdentifier);
            }
        }
    }
}

