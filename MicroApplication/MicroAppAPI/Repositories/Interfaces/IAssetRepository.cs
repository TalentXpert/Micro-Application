
namespace MicroAppAPI.Repositories.Interfaces
{
    public interface IAssetRepository : IRepository<Asset>
    {
        List<Asset> GetAssets(Guid organizationId, Guid subgroupId, GridRequestVM model);
        List<UserMgmtFormControlValueDataSource> GetAssetDataSource(Guid organizationId);
    }
}
