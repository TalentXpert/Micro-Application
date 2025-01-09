


using BaseLibrary.Configurations.DataSources.LinqDataSources;

namespace MicroAppAPI.Repositories
{
    public class PollutionDataRepository : Repository<PollutionData>, IPollutionDataRepository
    {
        IUserManagementDatabase _currentUnitOfWork;

        public PollutionDataRepository(IUserManagementDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<PollutionDataRepository>())
        {
            _currentUnitOfWork = unitOfWork;
        }
    }

    public class AssetRepository : Repository<Asset>, IAssetRepository
    {
        IUserManagementDatabase _currentUnitOfWork;

        public AssetRepository(IUserManagementDatabase unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory.CreateLogger<AssetRepository>())
        {
            _currentUnitOfWork = unitOfWork;
        }
        private IQueryable<Asset> OrganizationAssets(Guid organizationId)
        {
            return _currentUnitOfWork.Assets.Where(a => a.OrganizationId == organizationId);
        }
        public List<Asset> GetAssets(Guid organizationId, Guid subgroupId, GridRequestVM model)
        {
            var query = OrganizationAssets(organizationId).Where(a => a.SubgroupId == subgroupId);
            var filter = model.GetFilterControlValue(ApplicationControls.InventoryNumber);
            if (IsNotNull(filter))
                query = query.Where(a => a.InventoryNumber == filter.Value);
            filter = model.GetFilterControlValue(BaseControls.Tag);
            if (IsNotNull(filter))
                query = query.Where(a => a.Tag == filter.Value);
            filter = model.GetFilterControlValue(BaseControls.Quantity);
            if (IsNotNull(filter) && decimal.TryParse(filter.Value, out decimal quantity))
                query = query.Where(a => a.Quantity >= quantity);
            return query.ToList();
        }

        public List<UserMgmtFormControlValueDataSource> GetAssetDataSource(Guid organizationId)
        {
            var query = from a in _currentUnitOfWork.Assets
                        join c in _currentUnitOfWork.PageDataStore on a.GroupId equals c.Id
                        join s in _currentUnitOfWork.PageDataStore on a.SubgroupId equals s.Id
                        where a.OrganizationId == organizationId
                        select new UserMgmtFormControlValueDataSource { Asset = a, AssetCategory = c, AssetSubcategory = s };
            return query.ToList();
        }

    }
    public class UserMgmtFormControlValueDataSource : BaseLinqFormDataSource
    {
        public FormDataStore? AssetCategory { get; set; }
        public FormDataStore? AssetSubcategory { get; set; }
        public Asset? Asset { get; set; }

    }
}
