using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;

namespace BaseLibrary.Repositories
{
    public interface IAppFormControlRepository : IRepository<AppFormControl>
    {
        List<AppFormControl> GetFormControls(Guid formId, Guid? layoutControlId, Guid? organizationId);
        List<AppFormControl> GetGlobalFormControlsWithControl(Guid formId);
        AppFormControl? GetAnyFixedControl(Guid appControlId);
        bool IsAppControlUsed(AppControl appControl);
        List<AppFormControl> GetFormFixedControls(Guid formId);
        AppFormControl? GetLayoutControl(Guid formId);
        List<AppFormControl> GetOrganizationConfiguredFormControls(Guid formId, Guid? layoutControlId, Guid organizationId);
    }

    public class AppFormControlRepository : Repository<AppFormControl>, IAppFormControlRepository
    {
        IBaseDatabase unitOfWork;

        public AppFormControlRepository(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<AppFormControlRepository>())
        {
            this.unitOfWork = unitOfWork;
        }

        private IQueryable<AppFormControl> OrganizationFormControls(Guid? organizationId)
        {
            if (organizationId.HasValue)
            {
                return unitOfWork.AppFormControls.Include(c => c.AppControl).Where(c => c.OrganisationId == null || c.OrganisationId == organizationId.Value);
            }
            return unitOfWork.AppFormControls.Include(c => c.AppControl).Where(c => c.OrganisationId == null);
        }
        public bool IsAppControlUsed(AppControl appControl)
        {
            return unitOfWork.AppFormControls.FirstOrDefault(c => c.AppControlId == appControl.Id) != null;
        }
        public List<AppFormControl> GetFormControls(Guid formId,Guid? layoutControlId, Guid? organizationId)
        {
            var query = OrganizationFormControls(organizationId);
            query = query.Where(c => c.AppFormId == formId);
            if (layoutControlId.HasValue)
                query = query.Where(c =>c.LayoutControlId ==null || c.LayoutControlId == layoutControlId.Value);
            var controls = query.ToList();
            return controls;
        }
        public List<AppFormControl> GetOrganizationConfiguredFormControls(Guid formId, Guid? layoutControlId, Guid organizationId)
        {
            var query =  unitOfWork.AppFormControls.Where(c => c.OrganisationId == organizationId && c.AppFormId == formId);
            if (layoutControlId.HasValue)
                query = query.Where(c => c.LayoutControlId == layoutControlId.Value);
            var controls = query.ToList();
            return controls;
        }
        public List<AppFormControl> GetGlobalFormControlsWithControl(Guid formId)
        {
            var query = OrganizationFormControls(null);
            query = query.Where(c => c.AppFormId == formId);
            var globalControls = query.OrderBy(c=>c.Position).ToList();
            return globalControls.Where(c=>c.GetIsGlobalControl()==true).ToList();
        }

        public AppFormControl? GetAnyFixedControl(Guid appControlId)
        {
            return unitOfWork.AppFormControls.FirstOrDefault(c => c.AppControlId == appControlId);
        }
        public AppFormControl? GetLayoutControl(Guid formId)
        {
            var query = OrganizationFormControls(null);
            query = query.Where(c => c.AppFormId == formId && c.AppControl.IsFormLayoutOwner == true);
            return query.FirstOrDefault();
        }
        public List<AppFormControl> GetFormFixedControls(Guid formId)
        {
            return unitOfWork.AppFormControls.Where(c => c.AppFormId == formId).ToList();
        }
    }
}
