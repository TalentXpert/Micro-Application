namespace BaseLibrary.Configurations
{
    public abstract class BaseForm
    {
        /// <summary>
        /// Implement this method to return your application forms
        /// </summary>
        protected abstract List<AppForm> GetApplicationForms();

        /// <summary>
        /// Override this method if you want to change base forms or their sequence. You can return empty list and return all forms from GetApplicationForms method.
        /// </summary>
        protected virtual List<AppForm> GetBaseForms()
        {
            return new List<AppForm>() { UserManagement, RolePermission, ApplicationControlForm };
        }

        public List<AppForm> GetForms()
        {
            var forms = new List<AppForm>();
            forms.AddRange(GetApplicationForms());
            forms.AddRange(GetBaseForms());
            return forms;
        }

        /// <summary>
        /// Implement this method to return your application forms with controls for seeding purpose
        /// </summary>
        protected abstract List<AppForm> GetApplicationFormsWithControls();
        /// <summary>
        /// Override this method if you want to change base forms or their sequence. You can return empty list and return all forms from GetApplicationFormsWithControls method.
        /// </summary>
        protected virtual List<AppForm> GetBaseFormWithControls()
        {
            var appForms = new List<AppForm>();



            var appForm = BaseForm.UserManagement;
            appForm.AddFormControl("37562631-134B-CE37-F519-08D909690ADE", BaseControl.Name, true, true, true, false);
            appForm.AddFormControl("453D188B-B7B6-CEE5-4C4D-08D90A01D43A", BaseControl.Email, true, true, true, true);
            appForm.AddFormControl("71035984-37CD-C5DC-51C8-08D90A04F665", BaseControl.ContactNumber, true, true, true, true);
            appForm.AddFormControl("6B610E04-EFD4-CE32-B69F-08D90A051284", BaseControl.LoginId, true, false, true, true);
            appForm.AddFormControl("92A2EAE2-01E6-CBD1-00A5-08D7CC999A61", BaseControl.Aadhar, true, true, true, true);
            appForms.Add(appForm);

            appForm = BaseForm.ApplicationControlForm;
            appForm.AddFormControl("6FFD0A53-566A-CE61-6A69-08D7CB0CFE31", BaseControl.ControlIdentifier, true);
            appForm.AddFormControl("62531E46-02C3-C3FC-DA0C-08D7CB0CFE31", BaseControl.ControlDataTypes, true);
            appForm.AddFormControl("D5C4A3EB-4817-C709-BB37-08D7CC956CCE", BaseControl.ControlType, true);
            appForm.AddFormControl("1A1D1056-7DF3-CDB3-7A30-08D7CC956CC9", BaseControl.DisplayLabel, true, true, false, false);
            appForm.AddFormControl("65766780-082E-C204-4DD8-08D7CC956CCE", BaseControl.Options, true, true, false, false);
            appForms.Add(appForm);

            appForm = BaseForm.RolePermission;
            appForm.AddFormControl("55EA36F9-8462-CE0A-18CC-08D7CC999A60", BaseControl.Role, true, true, true, false);
            appForm.AddFormControl("D998DC52-10F4-CEDE-8CDB-08D7CC999A60", BaseControl.PermissionSelection, true, true, true, false);
            appForms.Add(appForm);

           
            appForm = BaseForm.ManageOrganizationForm;
            appForm.AddFormControl("E02C30E6-3F4E-4CD5-AFC9-7B81037EC275", BaseControl.Name, true, true, true, false);
            appForm.AddFormControl("9B40DDF5-4752-44E9-91B6-829868354973", BaseControl.Website, true, true, false, false);
            appForms.Add(appForm);

            appForm = BaseForm.ManageOrganizationAdminForm;
            appForm.AddFormControl("CFDE4993-0FE4-45F0-9516-32289A18D08F", BaseControl.Name, true, true, true, false);
            appForm.AddFormControl("1EAA95D0-9833-4A59-AAFE-7D0DD8A20CD6", BaseControl.Email, true, true, true, true);
            appForm.AddFormControl("A36D17D8-B2FB-476E-AFF4-95E308AD8360", BaseControl.ContactNumber, true, true, true, true);
            appForm.AddFormControl("376E6FEB-15F4-461A-8160-5CAE48F4F92E", BaseControl.LoginId, true, false, true, true);
            appForm.AddFormControl("E5BBE240-4D7F-4527-8560-C75D53517AC8", BaseControl.Organization, true, true, true, true);
            appForms.Add(appForm);

            return appForms;
        }

        public List<AppForm> GetFormsWithControls()
        {
            var appForms = new List<AppForm>();
            appForms.AddRange(GetBaseFormWithControls());
            appForms.AddRange(GetApplicationFormsWithControls());
            return appForms;
        }



        public static AppForm UserManagement = AppForm.Create(UserManagementFormId, "User", BaseMenu.ConfigurationMenuId, 4, true, BasePermission.ManageUsers);
        public static AppForm RolePermission = AppForm.Create(RolePermissionFormId, "Role Permission", BaseMenu.ConfigurationMenuId, 5, true, BasePermission.ManageRoles);
        public static AppForm ApplicationControlForm = AppForm.Create(ApplicationControlFormId, "Application Control", BaseMenu.SystemConfigurationMenuId, 1, false, BasePermission.ManageFormControls);

        public static AppForm ManageOrganizationForm = AppForm.Create(ManageOrganizationFormId, "Organizations", BaseMenu.SystemConfigurationMenuId, 1, false, BasePermission.ManageSystem);
        public static AppForm ManageOrganizationAdminForm = AppForm.Create(ManageOrganizationAdminFormId, "Organization Admins", BaseMenu.SystemConfigurationMenuId, 1, false, BasePermission.ManageSystem);

        public const string UserRoleFormId = "442B1CD9-6331-49B9-AB50-6BB97F6A2BB5";
        public static AppForm UserRoleForm = AppForm.Create(UserRoleFormId, "User Role", BaseMenu.NoneMenuId, 1, false, BasePermission.None);

        public const string AuditFormId = "0CEA3092-0B53-4B40-9F44-8F37077819A3";
        public static AppForm AuditForm = AppForm.Create(AuditFormId, "Audit", BaseMenu.NoneMenuId, 1, false, BasePermission.None);

        public const string UserManagementFormId = "76BDCE12-97AA-C988-F075-08D906182053";
        public const string ApplicationControlFormId = "4E1851CE-CE89-CEAE-E37B-08D90619EC92";
        public const string RolePermissionFormId = "2DFC689E-ED79-C45D-4516-08D9086E7CDB";
        public const string ManageOrganizationFormId = "0122D161-C76E-45CD-A5FE-74CD83F58223";
        public const string ManageOrganizationAdminFormId = "A5443C1E-C8BF-4D57-B527-C1949C97716F";
        
    }
}

