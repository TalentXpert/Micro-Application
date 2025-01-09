namespace BaseLibrary.Repositories
{
    public interface IApplicationSettingRepository : IRepository<ApplicationSetting>
    {

    }
    public class ApplicationSettingRepository : Repository<ApplicationSetting>, IApplicationSettingRepository
    {
        IBaseDatabase unitOfWork;

        public ApplicationSettingRepository(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<UserRepository>())
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
