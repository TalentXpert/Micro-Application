
namespace MicroAppAPI.Configurations
{
    public class ApplicationPermission : BasePermission
    {
        protected override List<Permission> GetApplicationPermissions()
        {
            var permissions = new List<Permission>() { ManageCountry, ManageState, ManageCity, ManageAssetGroup, ManageAssetSubgroup, ManageAsset };
            return permissions;
        }

        public override List<Permission> GetOrganizationAdminPermissions()
        {
            return base.GetOrganizationAdminPermissions();
        }

        
        public static Permission ManageCountry = new Permission("70B18B69-D209-44B5-A263-ABE6964181A5", "Manage Country", "");
        public static Permission ManageState = new Permission("AFD69527-7D9C-4404-8C9A-5550D275C97E", "Manage State", "");
        public static Permission ManageCity = new Permission("380A26CC-932D-4BAB-9242-96672F894A65", "Manage City", "");

        public static Permission ManageAssetGroup = new Permission("7F98FFD7-4E4C-418B-8232-D50DBF576600", "Manage Asset", "");
        public static Permission ManageAssetSubgroup = new Permission("DF2A5F57-D024-4C17-AD42-43CC5281E6CC", "Manage Asset", "");
        public static Permission ManageAsset = new Permission("2A5BC8A0-F80F-490E-82C6-EF5A7252201F", "Manage Asset", "");
    }
}
