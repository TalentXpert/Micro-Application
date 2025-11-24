using BaseLibrary.Domain;
using BaseLibrary.Domain.ComponentSchemas;
using System.Collections.Generic;
using System.Data;

namespace BaseLibrary.Services
{
    public interface IComponentSchemaService
    {
        ComponentSchema? GetComponent(Guid id);
        List<ComponentSchema> GetComponents(ComponentTypes dashboard, ApplicationUser loggedInUser);
        void SaveUpdateChartSchema(ChartSchema vm, Guid? organizationId, Guid? loggedInUserId);
        DashboardSchema SaveUpdateDashboardSchema(DashboardSchema vm, Guid? organizationId, Guid? loggedInUserId);
        List<MicroSqlQueryParameter> GetMicroSqlQueryParameters(DashboardSchema dashboard);
    }

    public class ComponentSchemaService : ServiceLibraryBase, IComponentSchemaService
    {
        public ComponentSchemaService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<ComponentSchemaService>())
        {

        }

        public ComponentSchema? GetComponent(Guid id)
        {
            var component = GetFromComponentInMemoryList(id);
            if (component is not null)
                return component;
            return RF.ComponentSchemaRepository.Get(id);
        }
        private ComponentSchema? GetFromComponentInMemoryList(Guid id)
        {
            var chartSchema = SF.MicroAppContract.GetBaseChart().GetCharts().FirstOrDefault(c => c.Id == id);
            if (chartSchema is not null)
            {
                var data = NewtonsoftJsonAdapter.SerializeObject(chartSchema);
                var componentSchema = ComponentSchema.Create(chartSchema.Id, ComponentTypes.Chart, chartSchema.Name, chartSchema.Description, data, null, null);
                return componentSchema;
            }
            return null;
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
            if (dc == null)
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
            if (dc == null)
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
            foreach (var row in dashboard.Rows)
            {
                foreach (var panel in row.Panels)
                {
                    return GetMicroSqlQueryParameters(panel);
                }
            }
            return new List<MicroSqlQueryParameter>();
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
                        return query.Parameters;
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
