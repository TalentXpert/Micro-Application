
namespace MicroAppAPI.Configurations
{

    public class ApplicationHandlerFactory : BaseHandlerFactory
    {
        public IServiceFactory SF { get; }

        public ApplicationHandlerFactory(IServiceFactory serviceFactory):base(serviceFactory)
        {
            SF = serviceFactory;
        }
        public override PageHandlerBase GetSmartPageHandler(Guid pageId, ApplicationUser loggedInUser)
        {
            switch (pageId.ToString().ToUpper())
            {
                case ApplicationPage.AssetGroupPageId: return new DefaultPageHandler(SF, loggedInUser, ApplicationPage.AssetGroupPage, ApplicationForm.AssetGroup, null);
                case ApplicationPage.AssetSubgroupPageId: return new DefaultPageHandler(SF, loggedInUser, ApplicationPage.AssetSubgroupPage, ApplicationForm.AssetSubgroup, ApplicationControl.AssetGroup);
                case ApplicationPage.AssetPageId: return new AssetPageHandler(SF, loggedInUser);
                case ApplicationPage.CountryPageId: return new DefaultPageHandler(BSF, loggedInUser, ApplicationPage.CountryPage, ApplicationForm.Country, null);
                case ApplicationPage.StatePageId: return new DefaultPageHandler(BSF, loggedInUser, ApplicationPage.StatePage, ApplicationForm.State, BaseControl.Country);
                case ApplicationPage.CityPageId: return new DefaultPageHandler(BSF, loggedInUser, ApplicationPage.CityPage, ApplicationForm.City, BaseControl.State);
            }
            return base.GetSmartPageHandler(pageId, loggedInUser);
        }

        public override FormHandlerBase GetFormHandler(Guid formId, ApplicationUser loggedInUser)
        {
            switch (formId.ToString().ToUpper())
            {
                case ApplicationForm.AssetGroupFormId: return new DefaultFormHandler(SF, loggedInUser, ApplicationForm.AssetGroup, null, null, false);
                case ApplicationForm.AssetSubgroupFormId: return new DefaultFormHandler(SF, loggedInUser, ApplicationForm.AssetSubgroup, ApplicationControl.AssetGroup, null, false);
                case ApplicationForm.AssetFormId: return new AssetFormHandler(SF, loggedInUser);
                case ApplicationForm.StateFormId: return new DefaultFormHandler(BSF, loggedInUser, ApplicationForm.State, BaseControl.Country, null, false);
                case ApplicationForm.CityFormId: return new DefaultFormHandler(BSF, loggedInUser, ApplicationForm.City, BaseControl.State, null, false);
                case ApplicationForm.CountryFormId: return new DefaultFormHandler(BSF, loggedInUser, ApplicationForm.Country, null, null, false);
            }
            return base.GetFormHandler(formId, loggedInUser);
        }
    }
}
