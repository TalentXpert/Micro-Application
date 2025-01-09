namespace BaseLibrary.Repositories
{
    public interface IAppPageRepository : IRepository<AppPage>
    {

    }


    public class AppPageRepository : Repository<AppPage>, IAppPageRepository
    {
        IBaseDatabase unitOfWork;

        public AppPageRepository(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<AppPageRepository>())
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
