
namespace BaseLibrary.Domain.Audit
{
    public class AuditLog : Entity
    {
        public Guid? OrganizationId { get; set; }
        public int EventId { get; protected set; }
        public string Event { get; protected set; }
        public string Description { get; set; }
        public string Detail { get; protected set; }
        public Guid UserId { get; protected set; }
        public Guid ReferenceObjectId { get; protected set; }
        public Guid? OriginalUserId { get; protected set; }
        protected AuditLog() { }
        public static AuditLog Create(AuditEvent auditEvent, Guid referenceObjectId, RecordFieldChanges? recordFieldChanges, Guid loggedInUserId, Guid? organizationId)
        {
            var auditLog = new AuditLog
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                EventId = auditEvent.Id,
                Event = auditEvent.Name,
                Detail = string.Empty,
                CreatedOn = DateTime.UtcNow,
                UserId = loggedInUserId,
                Description = string.Empty,
                ReferenceObjectId = referenceObjectId,
                OrganizationId = organizationId,
            };

            auditLog.SetCreatedOn();
            auditLog.SetUpdatedOn();

            if (recordFieldChanges != null)
                auditLog.Detail = recordFieldChanges.GetJSON();
            return auditLog;
        }

        public override IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            X.Validator.GuidValidator.CheckForEmpltyOrDefaulValue(Id, "Id", validationResults);
            X.Validator.GuidValidator.CheckForEmpltyOrDefaulValue(UserId, "UserId", validationResults);
            X.Validator.GuidValidator.CheckForEmpltyOrDefaulValue(ReferenceObjectId, "ReferenceObjectId", validationResults);
            X.Validator.IntegerValidator.CheckForEmpltyOrDefaulValue(EventId, "EventId", validationResults);
            X.Validator.StringValidator.CheckForNullOrEmpty(Event, "Event", validationResults);
            X.Validator.StringValidator.CheckForNullOrEmpty(Description, "Description", validationResults);
            X.Validator.StringValidator.CheckForMaxLength(Event, "Event", 64, validationResults);
            return validationResults;
        }

        public AuditLogVM ToAuditLogVM(string timeZone)
        {
            return new AuditLogVM
            {
                Event = this.Event,
                Title = this.Description,
                EventDate = DateTimeHelper.GetFormattedDateTime(this.CreatedOn, timeZone),
                Detail = this.Detail
            };
        }
    }

}
