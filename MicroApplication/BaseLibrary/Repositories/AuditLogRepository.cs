using BaseLibrary.Domain.Audit;
using BaseLibrary.Domain.PerformanceLogs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BaseLibrary.Repositories
{
    
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        List<AuditLog> GetAuditLogs(Guid referenceObjectId);
        List<List<GridCell>> GetRows(List<ControlFilter> filters, List<GridHeader> headers, ApplicationUser loggedInUser);
    }
    public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
    {
        IBaseDatabase unitOfWork;
        public AuditLogRepository(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<AuditLogRepository>())
        {
            this.unitOfWork = unitOfWork;
        }

        public List<AuditLog> GetAuditLogs(Guid referenceObjectId)
        {
            return unitOfWork.AuditLogs.Where(a => a.ReferenceObjectId == referenceObjectId).OrderByDescending(a => a.CreatedOn).ToList();
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
            var query = $"select * from auditlog where (organizationId='{loggedInUser.OrganizationId}')";
          // var where = "";
            //if (filters.Any())
            //{
            //    where = PrepareWhereClause(filters, "ControlIdentifier", where, "ControlIdentifier", SqlDbType.NVarChar, sqlParameters);
            //    where = PrepareWhereClause(filters, "DisplayLabel", where, "DisplayLabel", SqlDbType.NVarChar, sqlParameters);
            //    where = PrepareWhereClause(filters, "ControlType", where, "ControlType", SqlDbType.NVarChar, sqlParameters);
            //    where = PrepareWhereClause(filters, "DataType", where, "DataType", SqlDbType.NVarChar, sqlParameters);
            //    where = PrepareWhereClause(filters, "Options", where, "Options", SqlDbType.NVarChar, sqlParameters);
            //    if (!string.IsNullOrEmpty(where))
            //    {
            //        query += " and " + where;
            //    }

            //}
            var dt = new SqlCommandExecutor().GetDataTable(query, sqlParameters);
            return dt;
        }

    }
}
