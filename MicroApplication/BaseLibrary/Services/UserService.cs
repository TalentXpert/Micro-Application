
using DocumentFormat.OpenXml.Spreadsheet;

namespace BaseLibrary.Services
{
    public interface IUserService
    {
        void ChangePasswordToHash(ApplicationUser user, int salt);
        ApplicationUser? GetUser(Guid key);
        ApplicationUser? GetUserByLoginId(string LoginId);
        void ChangePassword(ApplicationUser user, string newPassword, int salt);
        void UpdateLastLogin(ApplicationUser user);
        void UpdatPasswordResetCode(string emailID, string passwordResetCode);
        List<List<GridCell>> GetRows(List<ControlFilter> filters, List<GridHeader> headers, ApplicationUser loggedInUser);
        ApplicationUser? SaveUpdate(SmartFormTemplateRequest model, ApplicationUser loggedInUser);
        ApplicationUser? GetUserAfterValidatingUserCredential(string email);
        ApplicationUser? Delete(Guid id);
        List<ApplicationUser> GetUsers(UserFilterVM model, ApplicationUser loggedInUser);
        List<ApplicationUser> GetUsers(Guid organizationId, GridRequestVM model);
        ApplicationUser SaveUpdateOrganizationAdmin(SmartFormTemplateRequest model, ApplicationUser loggedInUser);
        void UpdateUserDefaultStudy(Guid userId, Guid studyId);
        List<ApplicationUser> GetUsersByOrganization(Guid organizationId);
    }
    public class UserService : ServiceLibraryBase, IUserService
    {
        public UserService(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory)
        {
        }

        public ApplicationUser GetUser(Guid key)
        {
            return RF.UserRepository.Get(key);
        }

        public void UpdateLastLogin(ApplicationUser user)
        {
            using (var db = new SqlCommandExecutor())
            {
                var query = $"Update ApplicationUser SET SessionId='{user.SessionId}',LastLogin=GETUTCDATE() {GetWhereClause(user.Id)}";
                db.ExecuteQuery(query);
            }
        }
        public void UpdatPasswordResetCode(string emailID, string passwordResetCode)
        {
            using (var db = new SqlCommandExecutor())
            {
                var query = $"Update ApplicationUser SET PasswordResetCode='{passwordResetCode}',PasswordResetCodeTimeStamp=GETUTCDATE() where Email='{emailID}'";
                db.ExecuteQuery(query);
            }
        }

        public ApplicationUser? GetUserByLoginId(string LoginId)
        {
            return RF.UserRepository.GetByLoginId(LoginId);
        }

        public ApplicationUser? GetUserByEmail(string email)
        {
            return RF.UserRepository.GetUserByEmail(email);
        }
        public void ChangePasswordToHash(ApplicationUser user, int salt)
        {
            using (var db = new SqlCommandExecutor())
            {
                var hash = user.GetPasswordHash(salt);
                var query = $"Update ApplicationUser SET Password='{hash}',Salt={salt} {GetWhereClause(user.Id)}";
                db.ExecuteQuery(query);
            }
        }

        private string GetWhereClause(Guid key)
        {
            return $" where Id='{key}'";
        }
        public void ChangePassword(ApplicationUser user, string newPassword, int salt)
        {
            using (var db = new SqlCommandExecutor())
            {
                user.Password = newPassword;
                var hash = user.GetPasswordHash(salt);
                var query = $"Update ApplicationUser SET Password='{hash}',Salt={salt} {GetWhereClause(user.Id)}";

                db.ExecuteQuery(query);
            }
        }

        public List<List<GridCell>> GetRows(List<ControlFilter> filters, List<GridHeader> headers, ApplicationUser loggedInUser)
        {
            return RF.UserRepository.GetRows(filters, headers, loggedInUser);
        }

        public ApplicationUser? SaveUpdate(SmartFormTemplateRequest model, ApplicationUser loggedInUser)
        {

            ApplicationUser user;

            if (model.DataKey.IsNullOrEmpty())
            {
                user = new ApplicationUser(loggedInUser);
                RF.UserRepository.Add(user);
            }
            else
            {
                user = RF.UserRepository.Get(model.DataKey.Value);
            }
            user.Update(model);


            return user;

        }

        public ApplicationUser? GetUserAfterValidatingUserCredential(string email)
        {
            var matchingUsers = RF.UserRepository.GetUserByEmail(email);
            if (IsNotNull(matchingUsers))
                throw new ValidationException("User with given email id already exists");
            return matchingUsers;
        }

        public ApplicationUser? Delete(Guid id)
        {

            ApplicationUser? user = null;
            if (IsNotNullOrEmpty(id))
                user = RF.UserRepository.Get(id);

            if (IsNotNull(user))
                RF.UserRepository.Remove(user);


            return user;

        }

        public List<ApplicationUser> GetUsers(UserFilterVM model, ApplicationUser loggedInUser)
        {
            return RF.UserRepository.GetUsers(model, loggedInUser);
        }

        public List<ApplicationUser> GetUsers(Guid organizationId, GridRequestVM model)
        {
            return RF.UserRepository.GetUsers(organizationId, model);
        }

        public ApplicationUser SaveUpdateOrganizationAdmin(SmartFormTemplateRequest model, ApplicationUser loggedInUser)
        {
            ApplicationUser user;

            if (model.DataKey.HasValue is false)
            {
                user = new ApplicationUser(loggedInUser);
                RF.UserRepository.Add(user);
            }
            else
            {
                user = RF.UserRepository.Get(model.DataKey.Value);
            }
            user.Update(model);
            user.UpdateOrganization(model);
            if (user.OrganizationId.HasValue is false)
                throw new ValidationException("Organization can not be empty. Please provide organization.");
            var role = AddOrganizationAdminRole(user.OrganizationId.Value);
            SF.UserRoleService.SaveUpdate(user.Id, new List<Guid> { role.Id });
            return user;
        }

        private ApplicationRole AddOrganizationAdminRole(Guid organizationId)
        {
            var organizationAdminRole = RF.RoleRepository.GetOrganizationRoles(organizationId).FirstOrDefault(r => r.Name == ApplicationRoles.OrganizationAdminRole);
            if (organizationAdminRole is null)
            {
                organizationAdminRole = ApplicationRole.Create(ApplicationRoles.OrganizationAdminRole, organizationId);
                RF.RoleRepository.Add(organizationAdminRole);
            }
            return organizationAdminRole;
        }

        public void UpdateUserDefaultStudy(Guid userId, Guid studyId)
        {
            ApplicationUser applicationUser = RF.UserRepository.Get(userId);
            if (applicationUser != null)
            {
                applicationUser.DefaultStudyId= studyId;
                applicationUser.SetUpdatedOn();
            }
        }

        public List<ApplicationUser> GetUsersByOrganization(Guid organizationId)
        {
            return RF.UserRepository.GetUsersByOrganization(organizationId);
        }
    }
}
