using BaseLibrary.Repositories;

namespace MicroAppAPI.Repositories.Interfaces
{
    public interface IRepositoryFactory : IBaseLibraryRepositoryFactory
    {

        IAssetRepository AssetRepository { get; }
        IPollutionDataRepository PollutionDataRepository { get; }
    }
}
