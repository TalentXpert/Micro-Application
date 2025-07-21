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
        
    }

    public class ComponentSchemaService : ServiceLibraryBase, IComponentSchemaService
    {
        public ComponentSchemaService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<ComponentSchemaService>())
        {

        }

        public ComponentSchema? GetComponent(Guid id)
        {
            return RF.ComponentSchemaRepository.Get(id);
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
                vm.Id= IdentityGenerator.NewSequentialGuid();
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
    }
}
