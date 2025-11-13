namespace BaseLibrary.Configurations
{
    /// <summary>
    /// This list all micro application activities and uses number upto 1000 so child application should use number above 1000.
    /// </summary>
    public class ActivityTypeBase 
    {
        public const int FormGenerate = 1;
        public const int ProcessForm = 2;
    }
    public abstract class BaseSearch
    {
        IBaseLibraryServiceFactory BSF { get; }
        public BaseSearch(IBaseLibraryServiceFactory baseLibraryServiceFactory)
        {
            BSF=baseLibraryServiceFactory;
        }
        public virtual List<SmartControlOption> GetSearchOptions(Guid? organizationId, AppControl appControl, Guid? parentId, string searchTerm)
        {
            switch (appControl.ControlIdentifier)
            {
                case BaseControls.Organization:
                    return BSF.PageDataStoreService.GetSearchResult(null, BaseForm.ManageOrganizationForm.Id, null, searchTerm);
            }
            return new List<SmartControlOption>();
        }
    }
}

