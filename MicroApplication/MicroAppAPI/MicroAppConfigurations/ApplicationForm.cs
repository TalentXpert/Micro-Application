

namespace MicroAppAPI.Configurations
{
    public class ApplicationForm : BaseForm
    {
        public const string CountryFormId = "D8207B75-2FDA-C983-FB95-08D80C38C2F1";
        public const string StateFormId = "AC87D7D3-4F57-C0D6-D84B-08D906178713";
        public const string CityFormId = "71753C36-719C-C974-D8BE-08D90617A6F6";

        public static AppForm Country = AppForm.Create(CountryFormId, "Country", BaseMenu.ConfigurationMenuId, 1, true, ApplicationPermission.ManageCountry);
        public static AppForm State = AppForm.Create(StateFormId, "State", BaseMenu.ConfigurationMenuId, 2, true, ApplicationPermission.ManageState);
        public static AppForm City = AppForm.Create(CityFormId, "City", BaseMenu.ConfigurationMenuId, 3, true, ApplicationPermission.ManageCity);

        public const string AssetGroupFormId = "578288E3-7A59-CD2B-2955-08D90617DDB8";
        public const string AssetSubgroupFormId = "C256D987-5396-C3F2-B3E8-08D906562123";
        public const string AssetFormId = "4497D40C-4CDC-C02D-5F92-08D906187310";

        public const string LoginFormId = "2B947051-82F4-C4DB-EEF3-08D9086EFE72";

        public static AppForm AssetGroup = AppForm.Create(AssetGroupFormId, "Asset Group", ApplicationMenu.AssetConfigurationId, 1, true, ApplicationPermission.ManageAssetGroup);
        public static AppForm AssetSubgroup = AppForm.Create(AssetSubgroupFormId, "Asset Subgroup", ApplicationMenu.AssetConfigurationId, 1, true, ApplicationPermission.ManageAssetSubgroup);
        public static AppForm Asset = AppForm.Create(AssetFormId, "Asset", ApplicationMenu.AssetConfigurationId, 1, true, ApplicationPermission.ManageAsset);

        public static AppForm LoginForm = AppForm.Create(LoginFormId, "Login", ApplicationMenu.UserManagementMenuId, 1, false,ApplicationPermission.None);

       

        protected override List<AppForm> GetApplicationForms()
        {
            return new List<AppForm> { AssetGroup, AssetSubgroup, Asset };
        }

        protected override List<AppForm> GetApplicationFormsWithControls()
        {
            var appForms = new List<AppForm>();
            
            var appForm = ApplicationForm.Country;
            appForm.AddFormControl("4E1851CE-CE89-CEAE-E37B-08D90619EC92", BaseControl.Name, true);
            appForm.AddFormControl("C256D987-5396-C3F2-B3E8-08D906562123", BaseControl.Description, true);
            appForms.Add(appForm);

            appForm = ApplicationForm.State;
            appForm.AddFormControl("2DFC689E-ED79-C45D-4516-08D9086E7CDB", BaseControl.Country, true).MakeGlobalControl();
            appForm.AddFormControl("2B947051-82F4-C4DB-EEF3-08D9086EFE72", BaseControl.Name, true);
            appForm.AddFormControl("92BEECFC-9590-C31D-2658-08D9086F9C8E", BaseControl.Description, true);
            appForms.Add(appForm);

            appForm = ApplicationForm.City;
            appForm.AddFormControl("781E6353-E45C-C3C5-2F06-08D9086FB9BB", BaseControl.Country, true).MakeGlobalControl();
            appForm.AddFormControl("284AFF89-6AA7-CC08-F79D-08D9087268F5", BaseControl.State, true).MakeGlobalControl();
            appForm.AddFormControl("17334509-D876-CACD-3C59-08D908777303", BaseControl.Name, true);
            appForm.AddFormControl("BBB86C8B-071E-C1E3-4029-08D908781D79", BaseControl.Description, true);
            appForms.Add(appForm);

            appForm = ApplicationForm.AssetGroup;
            appForm.AddFormControl("F6ED5D15-0278-C15D-BBA4-08D9087C59B2", BaseControl.Name, true);
            appForm.AddFormControl("10BCB097-A1AC-CF69-93D7-08D9087E9D24", BaseControl.Description, true);
            appForms.Add(appForm);

            appForm = ApplicationForm.AssetSubgroup;
            appForm.AddFormControl("2ABFE967-9B84-CC52-8CD6-08D7CC956CCF", ApplicationControl.AssetGroup, true).MakeGlobalControl();
            appForm.AddFormControl("0B255468-DF94-C802-F581-08D7CC956CCF", BaseControl.Name, true);
            appForm.AddFormControl("326154C2-9059-C36C-C905-08D90A075B6E", ApplicationControl.InventoryNumberPrefix, true);
            appForm.AddFormControl("322BD36A-C8AC-C82B-29B6-08D7CC999A5F", BaseControl.Description, true);
            appForms.Add(appForm);

            appForm = ApplicationForm.Asset;
            appForm.AddFormControl("EA1FB958-0040-C2B3-17DA-08D90945D76B", ApplicationControl.AssetGroup, true).MakeGlobalControl();
            appForm.AddFormControl("C2599E90-95D3-CAD9-A54B-08D7CC999A5F", ApplicationControl.AssetSubgroup, true).MakeGlobalControl();
            appForm.AddFormControl("671A5BED-0133-C7D6-CB5A-08D90950EBAC", BaseControl.Name, true);
            appForm.AddFormControl("03B14187-F8FB-C5AE-D80E-08D90A0771EB", ApplicationControl.InventoryNumber, false);
            appForm.AddFormControl("82ABCBA0-21F4-C5E1-EB11-08D90ACE9B29", BaseControl.Tag, true, false, false, false);
            appForm.AddFormControl("900061C6-1BDE-4A2D-8249-08200AB7C9BA", BaseControl.Quantity, false);
            appForm.AddFormControl("D4471783-776B-C02F-F0F6-08D7CB0CFE2A", ApplicationControl.AssetUnit, false);
            appForm.AddFormControl("62667145-1F58-CA23-198F-08D90952734E", BaseControl.Description, true);
            appForms.Add(appForm);

            //appForm = ApplicationForm.LoginForm;
            //appForm.AddFormControl("92BEECFC-9590-C31D-2658-08D9086F9C8E", BaseControl.Name, true);
            //appForm.AddFormControl("92BEECFC-9590-C31D-2658-08D9086F9C8E", BaseControl.Password, true);
            //appForms.Add(appForm);

            return appForms;
        }
    }


    /*






781E6353-E45C-C3C5-2F06-08D9086FB9BB
284AFF89-6AA7-CC08-F79D-08D9087268F5
17334509-D876-CACD-3C59-08D908777303
BBB86C8B-071E-C1E3-4029-08D908781D79
64919EAA-4F34-CDFE-9B38-08D908797B77
F6ED5D15-0278-C15D-BBA4-08D9087C59B2
10BCB097-A1AC-CF69-93D7-08D9087E9D24
EA1FB958-0040-C2B3-17DA-08D90945D76B
3A48F88D-531C-C950-E73E-08D909479BBF
876EC5F2-4D9E-C715-0180-08D9094C8F9D
671A5BED-0133-C7D6-CB5A-08D90950EBAC
62667145-1F58-CA23-198F-08D90952734E
37562631-134B-CE37-F519-08D909690ADE
453D188B-B7B6-CEE5-4C4D-08D90A01D43A
71035984-37CD-C5DC-51C8-08D90A04F665
6B610E04-EFD4-CE32-B69F-08D90A051284
326154C2-9059-C36C-C905-08D90A075B6E
03B14187-F8FB-C5AE-D80E-08D90A0771EB
82ABCBA0-21F4-C5E1-EB11-08D90ACE9B29
     */

}
