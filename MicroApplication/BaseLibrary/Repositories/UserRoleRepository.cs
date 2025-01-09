

using BaseLibrary.Configurations;

namespace UserManagement.Services.Repositories
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        List<Organization> GetOrganizations(ApplicationUser loggedInUser, GridRequestVM model);
    }
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        IBaseDatabase unitOfWork;

        public OrganizationRepository(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<ApplicationRolePermissionRepository>())
        {
            this.unitOfWork = unitOfWork;
        }

        public List<Organization> GetOrganizations(ApplicationUser loggedInUser, GridRequestVM model)
        {
            var query = unitOfWork.Organizations.AsQueryable();

            var filter = model.GetFilterControlValue(BaseControl.Name);
            if (IsNotNull(filter))
                query = query.Where(a => a.Name.StartsWith(filter.Value));

            filter = model.GetFilterControlValue(BaseControl.Website);
            if (IsNotNull(filter))
                query = query.Where(a => a.Website.StartsWith(filter.Value));

            return query.ToList();
        }
    }
        public interface IApplicationRolePermissionRepository : IRepository<ApplicationRolePermission>
    {
        List<ApplicationRolePermission> GetRolePermissions(Guid roleId);
        
    }
    public class ApplicationRolePermissionRepository : Repository<ApplicationRolePermission>, IApplicationRolePermissionRepository
    {
        IBaseDatabase _currentUnitOfWork;

        public ApplicationRolePermissionRepository(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<ApplicationRolePermissionRepository>())
        {
            _currentUnitOfWork = unitOfWork;
        }

       

        public List<ApplicationRolePermission> GetRolePermissions(Guid roleId)
        {
            return _currentUnitOfWork.RolePermission.Where(u => u.RoleId == roleId).ToList();
        }
    }
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        List<UserRole> GetUserRoles(Guid userId);
        bool HasPermission(ApplicationUser loggedInUser, Permission? permission);
        List<ApplicationRolePermission> GetUserPermissions(Guid userId);
    }

    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        IBaseDatabase _currentUnitOfWork;

        public UserRoleRepository(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<UserRoleRepository>())
        {
            _currentUnitOfWork = unitOfWork;
        }


        public List<UserRole> GetUserRoles(Guid userId)
        {
            return _currentUnitOfWork.UserRole.Where(u => u.UserId == userId).ToList();
        }

        public bool HasPermission(ApplicationUser loggedInUser, Permission? permission)
        {
            if (permission == null)
                return false;
            var query = from r in _currentUnitOfWork.UserRole
                        join p in _currentUnitOfWork.RolePermission on r.RoleId equals p.RoleId
                        where p.PermissionId == permission.Id
                        select p;
            return query.Any();
        }
        public List<ApplicationRolePermission> GetUserPermissions(Guid userId)
        {
            var query = from r in _currentUnitOfWork.UserRole
                        join p in _currentUnitOfWork.RolePermission on r.RoleId equals p.RoleId
                        where r.UserId == userId
                        select p;
            return query.ToList();
        }
    }
}
