namespace BaseLibrary.Services
{
    public interface IAppPageService
    {
        AppPage GetPage(Guid pageId);
        AppPage SaveUpdatePageAndForm(AppPageSaveUpdateVM model);
    }
    public class AppPageService : ServiceLibraryBase, IAppPageService
    {
        public AppPageService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory)
        {
        }

        public AppPage GetPage(Guid pageId)
        {
            return RF.AppPageRepository.Get(pageId);
        }

        public AppPage SaveUpdatePageAndForm(AppPageSaveUpdateVM model)
        {
            AppPage? page = null;
            if (model.Id.IsNullOrEmpty() == false)
                page = RF.AppPageRepository.Get(model.Id.Value);

            if (page == null)
            {
                page = AppPage.Create(model);
                RF.AppPageRepository.Add(page);
            }
            else
            {
                page.Update(model);
            }

            var form = RF.AppFormRepository.Get(page.Id);
            if (form == null)
            {
                form = AppForm.Create(page.Id.ToString(), model,true);
                RF.AppFormRepository.Add(form);
            }
            else
                form.Update(model, true);


            return page;

        }
    }
}
