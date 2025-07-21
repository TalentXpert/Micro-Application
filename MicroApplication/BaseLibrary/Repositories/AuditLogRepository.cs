using BaseLibrary.Domain.Audit;
using BaseLibrary.Domain.PerformanceLogs;

namespace BaseLibrary.Repositories
{
    
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        List<AuditLog> GetAuditLogs(Guid referenceObjectId);
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

    }
}
