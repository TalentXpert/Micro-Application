namespace MicroAppAPI.Configurations
{

    public class AssetFormHandler : DefaultFormHandler
    {
        IServiceFactory SF { get; set; }
        public AssetFormHandler(IServiceFactory serviceFactory, ApplicationUser loggedInUser)
            : base(serviceFactory, loggedInUser, ApplicationForm.Asset, ApplicationControl.AssetSubgroup, ApplicationControl.AssetSubgroup, true)
        {
            SF=serviceFactory;
        }

        public override void ProcessFormSaveRequest(SmartFormTemplateRequest model)
        {
            Asset asset;
            var subgroupIdText = model.ControlValues.GetControlFirstValue(ApplicationControl.AssetSubgroup);
            if (subgroupIdText != null && Guid.TryParse(subgroupIdText, out Guid subgroupId))
            {
                if (model.DataKey.IsNullOrEmpty())
                {
                    asset = new Asset(LoggedInUser);
                    SF.RepositoryFactory.AssetRepository.Add(asset);
                }
                else
                {
                    asset = SF.RepositoryFactory.AssetRepository.Get(model.DataKey.Value);
                }
                asset.Update(model);
            }
        }

        public override FormStoreBase? GetStoreObject(Guid id)
        {
            return SF.RepositoryFactory.AssetRepository.Get(id);
        }

        public override void DeleteData(Guid id)
        {
            var asset = SF.RepositoryFactory.AssetRepository.Get(id);
            SF.RepositoryFactory.AssetRepository.Remove(asset);
        }
    }
}
