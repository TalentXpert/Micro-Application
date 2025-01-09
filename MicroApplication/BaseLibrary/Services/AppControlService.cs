
namespace BaseLibrary.Services
{
    public interface IAppControlService
    {
        List<AppControl> GetOrganisationControls(ApplicationUser loggedInUser);
        AppControl? GetAppControl(Guid id);
        AppControl SaveUpdateAppControl(Guid organizationId, AppControlVM vm);
        List<List<GridCell>> GetAppControlRows(List<ControlFilter> filters, List<GridHeader> headers, ApplicationUser loggedInUser);
        void DeleteAppControl(Guid id, ApplicationUser loggedInUser);
        List<AppControl> GetAllFixedControls();
    }
    public class AppControlService : ServiceLibraryBase, IAppControlService
    {
        public AppControlService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory)
        {

        }
        public List<AppControl> GetOrganisationControls(ApplicationUser loggedInUser)
        {
            return RF.AppControlRepository.GetOrganisationControls(loggedInUser.GetOrganizationId());
        }
        public List<AppControl> GetAllFixedControls()
        {
            return RF.AppControlRepository.GetAllFixedControls();
        }
        public AppControl? GetAppControl(Guid id)
        {
            return RF.AppControlRepository.Get(id);
        }

        public AppControl SaveUpdateAppControl(Guid organizationId, AppControlVM vm)
        {

            if (RF.AppControlRepository.IsDuplicateIdentifier(organizationId, vm.Id, vm.ControlIdentifier))
                throw new ValidationException("Duplicate control identifer. Please use unique identifier.");
            if (RF.AppControlRepository.IsDuplicateDisplayLabel(organizationId, vm.Id, vm.DisplayLabel))
                throw new ValidationException("Duplicate display label. Please use unique display label.");

            AppControl appControl;
            if (IsNullOrEmpty(vm.Id))
            {
                appControl = AppControl.Create(organizationId, vm);
                RF.AppControlRepository.Add(appControl);
            }
            else
            {
                appControl = GetAppControl(vm.Id);
            }
            appControl.Update(vm);

            return appControl;

        }

        public List<List<GridCell>> GetAppControlRows(List<ControlFilter> filters, List<GridHeader> headers, ApplicationUser loggedInUser)
        {
            return RF.AppControlRepository.GetRows(filters, headers, loggedInUser);
        }

        public void DeleteAppControl(Guid id, ApplicationUser loggedInUser)
        {
            var appControl = RF.AppControlRepository.Get(id);
            if (appControl == null)
                throw new ValidationException($"Application control with id-{id} not found.");
            if (appControl.OrganisationId != loggedInUser.OrganizationId)
                throw new ValidationException($"{appControl.DisplayLabel} is not associated with your organization. You can not delete a it.");
            RF.AppControlRepository.Remove(appControl);
        }
    }
}
