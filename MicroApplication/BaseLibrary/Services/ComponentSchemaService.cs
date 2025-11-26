
namespace BaseLibrary.Services
{
    public interface IComponentSchemaService
    {
        List<ComponentSchema> GetComponents(ComponentTypes dashboard, ApplicationUser loggedInUser);
        void SaveUpdateChartSchema(ChartSchema vm, Guid? organizationId, Guid? loggedInUserId);
        DashboardSchema SaveUpdateDashboardSchema(DashboardSchema vm, Guid? organizationId, Guid? loggedInUserId);
        List<MicroSqlQueryParameter> GetMicroSqlQueryParameters(DashboardSchema dashboard);
        DashboardSchema GetDashboardSchema(Guid id);
        ChartSchema GetChartSchema(Guid id);
    }

    public class ComponentSchemaService : ServiceLibraryBase, IComponentSchemaService
    {
        public ComponentSchemaService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<ComponentSchemaService>())
        {

        }
        public DashboardSchema GetDashboardSchema(Guid id)
        {
            var dashboards = SF.MicroAppContract.GetApplicationDashboards();
            var dashboard = dashboards.FirstOrDefault(d => d.Id == id);
            if (dashboard is null)
            {
                var ds = RF.ComponentSchemaRepository.Get(id);
                if (ds is not null)
                    dashboard = ds.GetDashboardSchema();
            }
            if (dashboard is not null)
                return dashboard;
            throw new ValidationException($"No Dashboard with id-{id} exits.");
        }

        public ChartSchema GetChartSchema(Guid id)
        {
            var chart = SF.MicroAppContract.GetBaseChart().GetCharts().FirstOrDefault(c => c.Id == id);
            if (chart is null)
            {
                var cs = RF.ComponentSchemaRepository.Get(id);
                if (cs is not null)
                    chart = cs.GetChartSchema();
            }
            if (chart is not null)
                return chart;
            throw new ValidationException($"No Chart with id-{id} exits.");
        }

        

        public List<ComponentSchema> GetComponents(ComponentTypes dashboard, ApplicationUser loggedInUser)
        {
            return RF.ComponentSchemaRepository.GetComponents(dashboard, loggedInUser);
        }
        public DashboardSchema SaveUpdateDashboardSchema(DashboardSchema vm, Guid? organizationId, Guid? loggedInUserId)
        {
            ComponentSchema? dc = null;
            if (vm.Id.HasValue)
                dc = RF.ComponentSchemaRepository.Get(vm.Id.Value);
            else
                vm.Id = IdentityGenerator.NewSequentialGuid();
            var data = NewtonsoftJsonAdapter.SerializeObject(vm);
            if (dc is null)
            {
                dc = ComponentSchema.Create(vm.Id, ComponentTypes.Dashboard, vm.Name, vm.Description, data, organizationId, loggedInUserId);
                RF.ComponentSchemaRepository.Add(dc);
            }
            else
            {
                dc.Update(vm.Name, vm.Description, data, loggedInUserId);
            }
            return dc.GetDashboardSchema();
        }

        public void SaveUpdateChartSchema(ChartSchema vm, Guid? organizationId, Guid? loggedInUserId)
        {
            ComponentSchema? dc = null;
            var data = NewtonsoftJsonAdapter.SerializeObject(vm);
            if (vm.Id.HasValue)
                dc = RF.ComponentSchemaRepository.Get(vm.Id.Value);
            if (dc is null)
            {

                dc = ComponentSchema.Create(vm.Id, ComponentTypes.Chart, vm.Name, vm.Description, data, organizationId, loggedInUserId);
                RF.ComponentSchemaRepository.Add(dc);
            }
            else
            {
                dc.Update(vm.Name, vm.Description, data, loggedInUserId);
            }
        }

        public List<MicroSqlQueryParameter> GetMicroSqlQueryParameters(DashboardSchema dashboard)
        {
            var result = new List<MicroSqlQueryParameter>();
            foreach (var row in dashboard.Rows)
            {
                foreach (var panel in row.Panels)
                {
                    var parameters = GetMicroSqlQueryParameters(panel);
                    foreach (var parameter in parameters)
                    {
                        if (!result.Any(p => p.Name == parameter.Name))
                            result.Add(parameter);
                    }
                }
            }
            return result;
        }

        private List<MicroSqlQueryParameter> GetMicroSqlQueryParameters(DashboardPanel panel)
        {
            Guid? dataSourceId = null;
            if (panel.ContentId.HasValue)
            {
                if (panel.ContentType == DashboardPanelContentType.Graph.ContentType)
                    dataSourceId = GetChartDataSource(panel.ContentId.Value);
                if (dataSourceId is not null)
                {
                    var dataSource = SF.SqlDataSourceService.GetDataSource(dataSourceId.Value);
                    var query = dataSource.GetSqlQuery();
                    if (query is not null)
                        return query.GetAllQueryParameters();
                }
            }
            return new List<MicroSqlQueryParameter>();
        }

        private Guid? GetChartDataSource(Guid chartId)
        {
            var chartSchema = SF.MicroAppContract.GetBaseChart().GetCharts().FirstOrDefault(c => c.Id == chartId);
            if (chartSchema == null)
                chartSchema = SF.RF.ComponentSchemaRepository.Get(chartId)?.GetChartSchema();
            return chartSchema?.DataSourceId;
        }
    }
}
