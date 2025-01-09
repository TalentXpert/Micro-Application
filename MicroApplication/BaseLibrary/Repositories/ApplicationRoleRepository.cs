

namespace BaseLibrary.Repositories
{
    public interface IApplicationRoleRepository : IRepository<ApplicationRole>
    {
        List<ApplicationRole> GetOrganizationRoles(Guid organizationId);
    }
    public class ApplicationRoleRepository : Repository<ApplicationRole>, IApplicationRoleRepository
    {
        IBaseDatabase _currentUnitOfWork;

        public ApplicationRoleRepository(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<ApplicationRoleRepository>())
        {
            _currentUnitOfWork = unitOfWork;
        }

        public List<ApplicationRole> GetOrganizationRoles(Guid organizationId)
        {
            return _currentUnitOfWork.Role.Where(p => p.OrganizationId == organizationId).ToList();
        }

        

    }
}
