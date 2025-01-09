using System.Data;
using BaseLibrary.Repositories;

namespace BaseLibrary.Services
{
    public class ServiceLibraryBase : CleanCode
    {
        public IBaseLibraryServiceFactory SF { get; }
        public ILoggerFactory LoggerFactory { get; }
        public IBaseLibraryRepositoryFactory RF { get; }

        public ServiceLibraryBase(IBaseLibraryServiceFactory serviceFactory, ILoggerFactory loggerFactory)
        {
            SF = serviceFactory;
            RF = SF.RF;
            LoggerFactory = loggerFactory;
        }
        protected void CheckForNullForm(AppForm? form, Guid formId)
        {
            if (IsNull(form))
                throw new ValidationException($"Form with id '{formId}' not found.");
        }
        protected void CheckForNullControl(AppControl? control, Guid controlId)
        {
            if (IsNull(control))
                throw new ValidationException($"Control with id '{controlId}' not found.");
        }
        protected void CheckForNullLayoutControl(AppFormControl layoutControl,Guid id)
        {
            if (layoutControl == null)
                throw new ValidationException($"Layout control with id - '{id}' not found.");
        }
        protected void CheckForNullFormControl(AppFormControl? formControl, Guid formControlId)
        {
            if (IsNull(formControl))
                throw new ValidationException($"Form control with id '{formControlId}' not found.");
        }
        public DataTable GetDataTable(string query)
        {
            using(var db = new SqlCommandExecutor())
            {
                var dt = db.GetDataTable(query);
                return dt; 
            }
        }
    }
}
