namespace BaseLibrary.Configurations
{
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

