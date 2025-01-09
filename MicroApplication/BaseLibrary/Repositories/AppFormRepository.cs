namespace BaseLibrary.Repositories
{
    public interface IAppFormRepository : IRepository<AppForm>
    {
        List<AppForm> GetAllCustomizableForms();
    }
    public class AppFormRepository : Repository<AppForm>, IAppFormRepository
    {
        IBaseDatabase unitOfWork;

        public AppFormRepository(IBaseDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<AppFormRepository>())
        {
            this.unitOfWork = unitOfWork;
        }

        public List<AppForm> GetAllCustomizableForms()
        {
            var forms = unitOfWork.AppForms.Where(f => f.IsCustomizable == true).ToList();
            return forms;
        }
    }
}
