namespace BaseLibrary.Configurations.FormHandlers
{
    public class DefaultFormHandler : FormHandlerBase
    {
        public IBaseLibraryServiceFactory BSF { get; }
        public DefaultFormHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser? loggedInUser, AppForm form, AppControl? parentControl, AppControl? layoutControl, bool hasPrivateDataStore) : base(serviceFactory, loggedInUser, form, parentControl,layoutControl, hasPrivateDataStore)
        {
            BSF = serviceFactory;
        }
    }
}
