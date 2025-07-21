using BaseLibrary.Domain.PerformanceLogs;

namespace BaseLibrary.Repositories
{
    public interface IPerformanceLogRepository : IRepository<PerformanceLog>
    {
        List<PerformanceLog> GetPerformanceLogs(PerformanceFilterVM performanceFilterVM);
    }

    public class PerformanceLogRepository : Repository<PerformanceLog>, IPerformanceLogRepository
    {
        IBaseDatabase unitOfWork;
        public PerformanceLogRepository(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<AuditLogRepository>())
        {
            this.unitOfWork = unitOfWork;
        }



        public List<PerformanceLog> GetPerformanceLogs(PerformanceFilterVM performanceFilterVM)
        {
            int duration = 1000;
            int.TryParse(performanceFilterVM.Duration, out duration);

            var query = unitOfWork.PerformanceLogs.Where(p => p.Duration >= duration);

            if (!string.IsNullOrWhiteSpace(performanceFilterVM.ControllerName))
                query = query.Where(p => p.Controller == performanceFilterVM.ControllerName.Trim());

            if (!string.IsNullOrWhiteSpace(performanceFilterVM.MethodName))
                query = query.Where(p => p.Method == performanceFilterVM.MethodName.Trim());

            return query.OrderByDescending(p => p.CreatedOn).Take(100).ToList();
        }
    }
}
