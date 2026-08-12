using BaseLibrary.Configurations.Models;
using BaseLibrary.Domain.Audit;

namespace BaseLibrary.Configurations.FormHandlers
{
    public class AuditFormFormHandler : FormTableFormHandler
    {
        public AuditFormFormHandler(IBaseLibraryServiceFactory serviceFactory, ApplicationUser loggedInUser)
        {
            ServiceFactory = serviceFactory;
        }

        public IBaseLibraryServiceFactory ServiceFactory { get; }

        public override FormViewTable GetTable(Guid formId, Guid formDataEntityId, ApplicationUser loggedInUser, string parentControlValue = "")
        {
            var formViewTable = new FormViewTable();
            var auditLog = ServiceFactory.RF.AuditLogRepository.Get(formDataEntityId);
            if (auditLog is not null)
            {
                var details = RecordFieldChanges.GetRecordFieldChanges(auditLog.Detail);
                if (details is not null)
                {
                    formViewTable.Columns.AddRange(new List<string> { "#", "Field", "Old Value", "New Value" });
                    int i = 1;
                    foreach (var eventChange in details.EventChanges)
                    {
                        var row = new List<string>
                {
                    i.ToString(),
                    eventChange.FieldName,
                    eventChange.OldValue,
                    eventChange.NewValue
                };
                        formViewTable.Rows.Add(row);
                        i++;
                    }
                }
            }
            return formViewTable;
        }
    }
}
