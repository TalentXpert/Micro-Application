
namespace MicroAppAPI.Configurations
{
    
    public class ApplicationMenu : BaseMenu
    {
        #region 
        public static Guid UserManagementMenuId = new Guid("D387B73D-45AC-C116-A741-08D909447A8A");
        public static Guid AssetConfigurationId = new Guid("2CF53ADD-9AB4-C0BD-A327-08D908A67D22");
        public static Guid DashboardId = new Guid("1CA7B808-34D8-C0C5-DA76-08D90AD67EE2");
        #endregion

        //Fixed Top Parent menus
        public Menu UserManagement = new Menu(UserManagementMenuId, "User Management", "");
        public Menu AssetConfiguration = new Menu(AssetConfigurationId, "Asset Configuration", "");
        public Menu Dashboard = new Menu(DashboardId, "Dashboard", "");

        public override List<Menu> GetFixedTopMenus()
        {
            var topMenus = new List<Menu> { SystemConfiguration, Configuration, UserManagement, AssetConfiguration, Dashboard, Report };
            return topMenus;
        }

        protected override List<Menu> GetAllMenus(List<string> userPermissions)
        {
            var result = new List<Menu>();
            //System admin (Website admin)
            if (HasPermission(userPermissions, ApplicationPermission.ManageSystem))
            {
                result.Add(SystemConfiguration);
                SetLandingMenu(FormBuilder);
                SystemConfiguration.Children.Add(FormBuilder);
            }

            //organization admin 
            if (HasPermission(userPermissions, ApplicationPermission.ManageOrganization))
            {
                result.Add(Configuration);
                SetLandingMenu(FormConfiguration);
                Configuration.Children.Add(FormConfiguration);
                Configuration.Children.Add(ChartBuilder);
                Configuration.Children.Add(DashboardBuilder);

                result.Add(UserManagement);
                UserManagement.Children.Add(ManageRole);
                UserManagement.Children.Add(ManageRolePermission);
            }

            
            if (userPermissions.Any(p => AreEqualsIgnoreCase(p, ApplicationPermission.ManageAsset.Code)))
                result.Add(AssetConfiguration);

            if (userPermissions.Any(p => AreEqualsIgnoreCase(p, ApplicationPermission.ViewReports.Code)))
                result.Add(Report);

            if (userPermissions.Any(p => AreEqualsIgnoreCase(p, ApplicationPermission.ViewDashboards.Code)))
            {
                result.Add(Dashboard);
                Dashboard.Children.Add(new Menu(Guid.Parse("C0F55AAC-F354-4E53-AD2F-2CA1D9A05CF0"), "Asset Dashboard", $"appPage/assetdashboard"));
            }
            return result;
        }

        Menu? LandingMenu;
        private void SetLandingMenu(Menu landingMenu)
        {
            if (LandingMenu != null)
                return;
            LandingMenu = landingMenu;
            landingMenu.IsLanding = true;
        }
    }

    /*

14A2E661-6960-CCB6-555D-08D90BC55171
59A2F51E-C6F5-C379-55AA-08D90BC55171
7B9771F4-4390-C213-5654-08D90BC55171
7CA68A0B-E419-CF52-569B-08D90BC55171
0F512CFD-D200-C498-2F84-08D90BC551BA
BACF4D5C-E2E5-CB86-3038-08D90BC551BA
AD05B038-5505-C326-314F-08D90BC551BA
D67D669D-C955-CE9D-31D3-08D90BC551BA
D8019229-970E-C1D3-32CF-08D90BC551BA
34C9C8F3-B6C4-C828-3338-08D90BC551BA
AB23D5B4-9D88-CFCB-339C-08D90BC551BA
9359D3F7-FAC6-C7CB-341C-08D90BC551BA
6DA1354E-9C3D-C69F-3495-08D90BC551BA
8E52848D-6E98-CD35-82D2-08D90BC55215
0588F88E-CC33-CF5C-836B-08D90BC55215
2759965B-C000-C4A2-83CC-08D90BC55215
08D897AE-93F9-C45B-844C-08D90BC55215
1CD53726-6A2A-C2A8-84AD-08D90BC55215
F794FD12-0774-C162-853F-08D90BC55215
BF5CD90B-80E5-C3D6-8686-08D90BC55215
15D0589D-73C3-C3BC-8773-08D90BC55215
C7690018-C256-C292-884C-08D90BC55215
2F6B232C-9981-C94E-CBBE-08D90BC55252
DF2509B2-EBA3-C506-CC46-08D90BC55252
D9F1EEFB-DFD8-C939-CCC1-08D90BC55252
9CD2B64D-9528-CDF2-CD1C-08D90BC55252
B4931FC9-4989-C2A8-CE18-08D90BC55252
62FB95D1-CD9B-CAFA-CE72-08D90BC55252
66C03E26-6AB6-CE27-CEB7-08D90BC55252
F65AEB6C-828A-CBB9-CEFB-08D90BC55252
F95EA00B-117A-CE6D-CF51-08D90BC55252
3D4BEDDB-266C-CEE3-10FF-08D90BC55295
E49E23A5-D488-C8CD-1180-08D90BC55295
6DB8705A-CA9B-C68D-11C7-08D90BC55295
3A66BCB9-9BE3-C7BF-120D-08D90BC55295
C9147A19-F1AA-C88F-1260-08D90BC55295
EA7C034E-E4A3-CD68-12A6-08D90BC55295
C1BE16F8-DF59-C188-131E-08D90BC55295
A51FFA96-E458-C037-1378-08D90BC55295
CBC06810-845F-CCF5-13E9-08D90BC55295
8154FAEC-1829-C4CE-A09C-08D90DEC3773
5B424565-9429-C304-D849-08D90E0C595C
13276984-14EE-C3CB-F44D-08D90E0C595C
CC3EA81A-0064-C15E-F5E3-08D90E0C595C
C73BB3EC-A93A-C5DC-F6BE-08D90E0C595C
D12E36C7-84B3-CE22-F78B-08D90E0C595C
2C036672-C39E-CDA8-F874-08D90E0C595C
159F3393-625B-C0C0-F8F9-08D90E0C595C
8AB8689A-625A-C7E3-F994-08D90E0C595C
904F7047-D1B5-C93C-FABA-08D90E0C595C
DFDF8308-622A-C206-FBF8-08D90E0C595C
6DB89A0F-CBB2-C31B-7DBA-08D90E0C6405
ECFF5E0B-F9C1-C21F-7EE4-08D90E0C6405
E633E1F2-C4BE-CA6E-7F9E-08D90E0C6405
19FCCAA4-92EB-C22F-805F-08D90E0C6405
F7F9DE9A-633F-C573-8118-08D90E0C6405
C65827D0-2C0A-CC9F-81C1-08D90E0C6405
F6CB1D9A-C44B-CBEC-826C-08D90E0C6405
E9B6E7BB-AD0F-C834-830E-08D90E0C6405
DD276A65-5A5B-CA5B-83AF-08D90E0C6405
0AC36772-3F3D-CCF2-8464-08D90E0C6405
CD294035-DE4E-C526-8A89-08D90E0CA1BE
0972A8A5-FD99-C7D2-8B8A-08D90E0CA1BE
A434B5E1-2666-C775-8BF0-08D90E0CA1BE
9BD5F545-6D4C-CB7E-8C4E-08D90E0CA1BE
47011383-4B89-CDC2-8CB4-08D90E0CA1BE
368D9176-EBE0-C6D1-8D1A-08D90E0CA1BE
4348AD50-A78F-C532-8D77-08D90E0CA1BE
4FF2A972-80A5-C7DD-8DD3-08D90E0CA1BE
BCD62A92-F162-C6DF-8E3E-08D90E0CA1BE
EEE56ABA-4701-CB9B-E4D5-08D90E0E080D
F27417FE-3E87-C5EA-E7AD-08D90E0E080D
91ED6626-BF3A-C14B-E83E-08D90E0E080D
5CCB5EEC-DA99-CE3F-E8BA-08D90E0E080D
C57F4A7C-4452-C2BB-E951-08D90E0E080D
241E70BE-AB5F-C2B9-E9E6-08D90E0E080D
    */
}
