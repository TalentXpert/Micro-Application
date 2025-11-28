using BaseLibrary.Domain.ComponentSchemas;
using BaseLibrary.Domain.DataSources;
using BaseLibrary.Repositories;

namespace BaseLibrary.Services
{
    public interface ISeederService
    {
        void SeedApplicationForms(List<AppPage> pages, List<AppControl> controls, List<AppForm> forms);
        void SeedCharts(List<ChartSchema> chartSchemas);
        void SeedDashboards(List<DashboardSchema> dashboards);
        void SeedDataSources(List<MacroSqlDataSource> sqlDataSource);
    }
    public class SeederService : ServiceLibraryBase, ISeederService
    {
        IMicroAppContract MicroAppContract { get; } 
        public SeederService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<SeederService>())
        {
            MicroAppContract = serviceFactory.MicroAppContract;
        }
        
        public void SeedApplicationForms(List<AppPage> pages, List<AppControl> controls, List<AppForm> forms)
        {

            var formControls = new List<AppFormControl>();
            foreach (var form in forms)
            {
                formControls.AddRange(form.AppFormControls);
                form.AppFormControls.Clear();
            }
            foreach (var formControl in formControls)
            {
                formControl.AppForm = null;
                formControl.AppControl = null;
            }

            var appPages = RF.AppPageRepository.GetAll();
            foreach (var page in pages)
            {
                var appPage = appPages.FirstOrDefault(p => p.Id == page.Id);
                if (appPage == null)
                {
                    appPage = page;
                    RF.AppPageRepository.Add(appPage);
                }
                appPage.Update(page);
            }

            var appControls = RF.AppControlRepository.GetFiltered(a => a.OrganisationId == null).ToList();
            var alreadyAdded = new List<AppControl>();
            foreach (var control in controls)
            {
                if (alreadyAdded.Any(p => p.Id == control.Id))
                    continue;
                var appControl = appControls.FirstOrDefault(f => f.Id == control.Id);
                if (appControl == null)
                {
                    appControl = control;
                    RF.AppControlRepository.Add(appControl);
                }
                appControl.Update(control);
                alreadyAdded.Add(appControl);
            }
           // RF.AppControlRepository.UnitOfWork.Commit();

            var appForms = RF.AppFormRepository.GetAll();
            foreach (var form in forms)
            {
                var appform = appForms.FirstOrDefault(f => f.Id == form.Id);
                if (appform == null)
                {
                    appform = form;
                    RF.AppFormRepository.Add(appform);
                }
                appform.Update(form);
            }

            var appFormControls = RF.AppFormControlRepository.GetFiltered(f => f.OrganisationId == null).ToList();
            foreach (var formControl in formControls)
            {
                var appFormControl = appFormControls.FirstOrDefault(c => c.Id == formControl.Id);
                if (appFormControl == null)
                {
                    if (appControls.Any(a => a.Id == formControl.AppControlId) || alreadyAdded.Any(a => a.Id == formControl.AppControlId))
                    {
                        appFormControl = formControl;
                        RF.AppFormControlRepository.Add(appFormControl);
                    }
                    else
                        throw new ValidationException($"{formControl.Id} using control {formControl.AppControlId} which is doesn't exist in database.");
                }
                appFormControl.Update(formControl);
            }
            
        }
        
        public void SeedCharts(List<ChartSchema> chartSchemas)
        {
            foreach(var chartSchema in chartSchemas)
            {
                SF.ComponentSchemaService.SaveUpdateChartSchema(chartSchema,null,null);
            }
        }

        public void SeedDashboards(List<DashboardSchema> dashboards)
        {
            foreach (var dashboard in dashboards)
            {
                SF.ComponentSchemaService.SaveUpdateDashboardSchema(dashboard,null,null);
            }
        }

        public void SeedDataSources(List<MacroSqlDataSource> sqlDataSources)
        {
            foreach(var datasource in sqlDataSources)
            {
                SF.SqlDataSourceService.SaveUpdateDataSources(datasource);
            }
        }

        public void SeedPages(List<AppPage> appPages)
        {

            var pages = RF.AppPageRepository.GetAll();
            foreach (var appPage in appPages)
            {
                var page = pages.FirstOrDefault(f => f.Id == appPage.Id);
                if (page == null)
                {
                    page = appPage;
                    RF.AppPageRepository.Add(page);
                }
                page.Update(appPage);
            }

        }
    }
}
