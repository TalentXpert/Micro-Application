

namespace MicroAppAPI.Services.Interfaces
{
    public interface IServiceFactory : IBaseLibraryServiceFactory
    {
        IRepositoryFactory RepositoryFactory { get; }


        new IUserManagementDatabase UnitOfWork { get; }
    }

}
