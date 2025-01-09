using BaseLibrary.Configurations;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using SqlParameter = Microsoft.Data.SqlClient.SqlParameter;

namespace BaseLibrary.Repositories
{
    
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        ApplicationUser? GetByLoginId(string loginId);
        List<ApplicationUser> GetUsers(UserFilterVM model, ApplicationUser loggedInUser);
        ApplicationUser? GetUserByEmail(string email);
        List<List<GridCell>> GetRows(List<ControlFilter> filters, List<GridHeader> headers, ApplicationUser loggedInUser);
        List<ApplicationUser> GetUsers(Guid organizationId, GridRequestVM model);
    }

    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        IBaseDatabase unitOfWork;

        public UserRepository(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<UserRepository>())
        {
            this.unitOfWork = unitOfWork;
        }

        public ApplicationUser? GetByLoginId(string loginId)
        {
            return unitOfWork.ApplicationUsers.FirstOrDefault(p => p.LoginId == loginId);
        }
        
        public List<ApplicationUser> GetUsers(UserFilterVM model, ApplicationUser loggedInUser)
        {
            var query = unitOfWork.ApplicationUsers.Where(p => p.OrganizationId == loggedInUser.OrganizationId);

            if (string.IsNullOrWhiteSpace(model.Name) == false)
                query = query.Where(a => a.Name == model.Name);

            if (string.IsNullOrWhiteSpace(model.Email) == false)
                query = query.Where(a => a.Email == model.Email);

            if (string.IsNullOrWhiteSpace(model.ContactNumber) == false)
                query = query.Where(a => a.ContactNumber == model.ContactNumber);

            if (string.IsNullOrWhiteSpace(model.LoginId) == false)
                query = query.Where(a => a.LoginId == model.LoginId);

            return query.ToList();
        }

        public List<ApplicationUser> GetUsers(Guid organizationId, GridRequestVM model)
        {
            var query = unitOfWork.ApplicationUsers.Where(p => p.OrganizationId == organizationId);

            var filter = model.GetFilterControlValue(BaseControl.Name);
            if (IsNotNull(filter))
                query = query.Where(a => a.Name == filter.Value);

            filter = model.GetFilterControlValue(BaseControl.Email);
            if (IsNotNull(filter))
                query = query.Where(a => a.Email == filter.Value);

            filter = model.GetFilterControlValue(BaseControl.ContactNumber);
            if (IsNotNull(filter))
                query = query.Where(a => a.ContactNumber == filter.Value);

            filter = model.GetFilterControlValue(BaseControl.LoginId);
            if (IsNotNull(filter))
                query = query.Where(a => a.LoginId == filter.Value);

            return query.ToList();
            
        }
        public List<List<GridCell>> GetRows(List<ControlFilter> filters, List<GridHeader> headers, ApplicationUser loggedInUser)
        {
            var result = new List<List<GridCell>>();
            var dataTable = GetUsers(filters,loggedInUser);
            headers = headers.OrderBy(h => h.Position).ToList();
            foreach (DataRow dr in dataTable.Rows)
            {
                var cells = new List<GridCell>();
                foreach (var header in headers)
                {
                    cells.Add(new GridCell { T = ReadText(dr, header.HeaderIdentifier) });
                }
                result.Add(cells);
            }
            return result;
        }
        private DataTable GetUsers(List<ControlFilter> filters, ApplicationUser loggedInUser)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            var query = $"select * from ApplicationUser where OrganizationId='{loggedInUser.OrganizationId}'";
            var where = "";
            if (filters.Any())
            {
                where = PrepareWhereClause(filters, "Name", where, "Name", SqlDbType.NVarChar, sqlParameters);
                where = PrepareWhereClause(filters, "Email", where, "Email", SqlDbType.NVarChar, sqlParameters);
                where = PrepareWhereClause(filters, "ContactNumber", where, "ContactNumber", SqlDbType.NVarChar, sqlParameters);
                where = PrepareWhereClause(filters, "LoginId", where, "LoginId", SqlDbType.NVarChar, sqlParameters);
                if(!string.IsNullOrEmpty( where))
                {
                    query += " and " + where;
                }
                
            }
            var dt = new SqlCommandExecutor().GetDataTable(query,sqlParameters);
            return dt;
        }

        public ApplicationUser? GetUserByEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
                email = email.ToLower().Trim();
            return this.unitOfWork.ApplicationUsers.FirstOrDefault(j => j.Email.ToLower() == email);
        }
    }
}
