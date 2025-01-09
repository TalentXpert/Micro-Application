using Microsoft.Data.SqlClient;
using System.Data;

namespace BaseLibrary.Repositories
{
    public interface IAppControlRepository : IRepository<AppControl>
    {
        List<AppControl> GetOrganisationControls(Guid organizationId);
        List<List<GridCell>> GetRows(List<ControlFilter> filters, List<GridHeader> headers, ApplicationUser loggedInUser);
        bool IsDuplicateIdentifier(Guid organisationId, Guid? id, string controlIdentifier);
        bool IsDuplicateDisplayLabel(Guid organisationId, Guid? id, string displayLabel);
        List<AppControl> GetAllFixedControls();
    }

    public class AppControlRepository : Repository<AppControl>, IAppControlRepository
    {
        IBaseDatabase unitOfWork;

        public AppControlRepository(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<AppControlRepository>())
        {
            this.unitOfWork = unitOfWork;
        }

        public List<AppControl> GetOrganisationControls(Guid organizationId)
        {
            var query = unitOfWork.AppControls.Where(c => c.OrganisationId == organizationId);
            return query.ToList();
        }
        private IQueryable<AppControl> GetOrganizationAndFixedControls(Guid organisationId)
        {
            var query = unitOfWork.AppControls.Where(c => c.OrganisationId == organisationId || c.OrganisationId == null);
            return query;
        }
        public List<List<GridCell>> GetRows(List<ControlFilter> filters, List<GridHeader> headers, ApplicationUser loggedInUser)
        {
            var result = new List<List<GridCell>>();
            var dataTable = GetAppControls(filters, loggedInUser);
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

        private DataTable GetAppControls(List<ControlFilter> filters, ApplicationUser loggedInUser)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            var query = $"select * from AppControl where (OrganisationId='{loggedInUser.OrganizationId}')";
            var where = "";
            if (filters.Any())
            {
                where = PrepareWhereClause(filters, "ControlIdentifier", where, "ControlIdentifier", SqlDbType.NVarChar, sqlParameters);
                where = PrepareWhereClause(filters, "DisplayLabel", where, "DisplayLabel", SqlDbType.NVarChar, sqlParameters);
                where = PrepareWhereClause(filters, "ControlType", where, "ControlType", SqlDbType.NVarChar, sqlParameters);
                where = PrepareWhereClause(filters, "DataType", where, "DataType", SqlDbType.NVarChar, sqlParameters);
                where = PrepareWhereClause(filters, "Options", where, "Options", SqlDbType.NVarChar, sqlParameters);
                if (!string.IsNullOrEmpty(where))
                {
                    query += " and " + where;
                }

            }
            var dt = new SqlCommandExecutor().GetDataTable(query, sqlParameters);
            return dt;
        }

        public bool IsDuplicateIdentifier(Guid organisationId, Guid? id, string controlIdentifier)
        {
            var query = GetOrganizationAndFixedControls(organisationId);
            query = query.Where(c => c.ControlIdentifier == controlIdentifier);// || c.DisplayLabel == displayLabel);
            if (IsNotNullOrEmpty(id))
                query = query.Where(c => c.Id != id.Value);
            var appControl = query.FirstOrDefault();
            if (IsNull(appControl))
                return false;
            return true;
        }

        public bool IsDuplicateDisplayLabel(Guid organisationId, Guid? id, string displayLabel)
        {
            var query = GetOrganizationAndFixedControls(organisationId);
            query = query.Where(c => c.DisplayLabel == displayLabel);
            if (IsNotNullOrEmpty(id))
                query = query.Where(c => c.Id != id.Value);
            var appControl = query.FirstOrDefault();
            if (IsNull(appControl))
                return false;
            return true;
        }

        public List<AppControl> GetAllFixedControls()
        {
            var query = unitOfWork.AppControls.Where(c => c.OrganisationId == null);
            return query.ToList();
        }
    }
}
