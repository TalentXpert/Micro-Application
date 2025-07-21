
using BaseLibrary.Domain.Audit;

namespace BaseLibrary.Services
{
    public interface IAuditEventBaseService
    {
        List<AuditLog> GetAuditlogs(string referenceId);
    }
    public class AuditEventBaseService : ServiceLibraryBase, IAuditEventBaseService
    {
        public AuditEventBaseService(IBaseLibraryServiceFactory serviceFactory, ILogger logger) : base(serviceFactory, logger)
        {

        }

        #region Helpers
        protected RecordFieldChanges? RecordAuditEvent(AuditEvent auditEvent, Guid referenceObjectId, string description, RecordFieldChanges changes, Guid loggedInUserId, Guid? organizationId)
        {
            AuditLog? auditLog = null;
            if (changes == null)
                auditLog = AuditLog.Create(auditEvent, referenceObjectId, null, loggedInUserId, organizationId);
            else if (changes.HasChanges())
                auditLog = AuditLog.Create(auditEvent, referenceObjectId, changes, loggedInUserId, organizationId);

            if (auditLog is not null)
            {
                auditLog.Description = description;
                RF.AuditLogRepository.Add(auditLog);
            }
            return changes;
        }
        protected string FormatDate(DateTime? dateTime)
        {
            if (dateTime.HasValue) return DateTimeHelper.GetFormattedDate(dateTime.Value);
            return "";
        }

        protected string FormatNumber(int? number)
        {
            if (number is null)
                return "";
            if (number.HasValue)
                return number.Value.ToString();
            return "";
        }
        protected string FormatBoolean(bool? boolean)
        {
            if (boolean.HasValue)
                return boolean.Value ? "Yes" : "No";
            return "";
        }

        protected string FormatDate(DateVM date)
        {
            if (date == null) return "";
            return FormatDate(date.GetDateTime());
        }

        #endregion

        #region Sample 
        /*
        public void RecordÀddJobCandidateEvent(Job job, JobCandidateVM jobCandidateVM, string loggedInUserName, Guid loggedInUserId, Guid? organizationId)
        {
            var changes = new RecordFieldChanges();
            changes.Add("Name", jobCandidateVM.Name);
            //changes.Add("ContactNumber", jobCandidateVM.ContactNumber);
            //changes.Add("Email", jobCandidateVM.Email);
            changes.Add("Designation", GetLookupText(LookupType.Designation, jobCandidateVM.DesignationId, jobCandidateVM.Designation));
            changes.Add("NoticePeriod", FormatNumber(jobCandidateVM.NoticePeriod));
            changes.Add("LastWorkingDay", FormatDate(jobCandidateVM.LastWorkingDay));
            changes.Add("ExperienceInYears", jobCandidateVM.ExperienceInYears.ToString());
            changes.Add("CTC", jobCandidateVM.CTC.ToString());
            changes.Add("ECTC", FormatNumber(jobCandidateVM.ECTC));
            changes.Add("Status", jobCandidateVM.Status);
            changes.Add("Relocation Required", FormatBoolean(jobCandidateVM.IsRelocationRequired));
            changes.Add("Country", GetLookupText(LookupType.Country, jobCandidateVM.CountryId, null));
            changes.Add("City", GetLookupText(ChildLookupType.City, jobCandidateVM.CityId, jobCandidateVM.City));
            changes.Add("Location", GetLocalityText(jobCandidateVM.CityId, jobCandidateVM.LocationId, jobCandidateVM.Location));
            changes.Add("ZipCode", jobCandidateVM.ZipCode);
            changes.Add("AssignedTo", GetJobProviderName(jobCandidateVM.AssignedTo));
            changes.Add("CV", jobCandidateVM.CVName);
            changes.Add("OnHold", FormatBoolean(jobCandidateVM.IsOnHold));
            changes.Add("PreferredModeOfCommunication", jobCandidateVM.PreferredModeOfCommunication);
            changes.Add("Skills", ConcatJobCandidateSkill(jobCandidateVM.Skills));
            changes.Add("SalaryOffered", FormatNumber(jobCandidateVM.SalaryOffered));
            changes.Add("IsPermanentAddress", FormatBoolean(jobCandidateVM.IsPermanentAddress));
            changes.Add("ReadyToRelocate", FormatBoolean(jobCandidateVM.IsReadyToRelocateToJobLocation));
            changes.Add("Gender", jobCandidateVM.Gender);
            changes.Add("DOB", FormatDate(jobCandidateVM.DOB));
            changes.Add("IsHideCandidateContact", FormatBoolean(jobCandidateVM.IsHideCandidateContact));
            var description = $"{loggedInUserName} added '{jobCandidateVM.Name}' to '{job.Title}' job.";
            RecordAuditEvent(AuditEvent.AddJobCandidateEvent, jobCandidateVM.Id, description, changes, loggedInUserId, organizationId);
        }
        public void RecordUpdateJobCandidateEvent(Job jobVM, JobCandidateVM oldJobCandidateVM, JobCandidateVM newJobCandidateVM, string loggedInUserName, Guid loggedInUserId, Guid? teamId)
        {
            var changes = new RecordFieldChanges();
            changes.Add("Name", newJobCandidateVM.Name, oldJobCandidateVM.Name);
            // changes.Add("ContactNumber", newJobCandidateVM.ContactNumber, oldJobCandidateVM.ContactNumber);
            // changes.Add("Email", newJobCandidateVM.Email, oldJobCandidateVM.Email);
            changes.Add("Designation", GetLookupText(LookupType.Designation, newJobCandidateVM.DesignationId, newJobCandidateVM.Designation), GetLookupText(LookupType.Designation, oldJobCandidateVM.DesignationId, oldJobCandidateVM.Designation));
            changes.Add("NoticePeriod", newJobCandidateVM.NoticePeriod.ToString(), oldJobCandidateVM.NoticePeriod.ToString());
            changes.Add("LastWorkingDay", FormatDate(newJobCandidateVM.LastWorkingDay), FormatDate(oldJobCandidateVM.LastWorkingDay));
            changes.Add("ExperienceInYears", newJobCandidateVM.ExperienceInYears.ToString(), oldJobCandidateVM.ExperienceInYears.ToString());
            changes.Add("CTC", newJobCandidateVM.CTC.ToString(), oldJobCandidateVM.CTC.ToString());
            changes.Add("ECTC", FormatNumber(newJobCandidateVM.ECTC), FormatNumber(oldJobCandidateVM.ECTC));
            changes.Add("Relocation Required", FormatBoolean(newJobCandidateVM.IsRelocationRequired), FormatBoolean(oldJobCandidateVM.IsRelocationRequired));
            changes.Add("Country", GetLookupText(LookupType.Country, newJobCandidateVM.CountryId, null), GetLookupText(LookupType.Country, oldJobCandidateVM.CountryId, null));
            changes.Add("City", GetLookupText(ChildLookupType.City, newJobCandidateVM.CityId, newJobCandidateVM.City), GetLookupText(ChildLookupType.City, oldJobCandidateVM.CityId, oldJobCandidateVM.City));
            changes.Add("Location", GetLocalityText(newJobCandidateVM.CityId, newJobCandidateVM.LocationId, newJobCandidateVM.Location), GetLocalityText(oldJobCandidateVM.CityId, oldJobCandidateVM.LocationId, oldJobCandidateVM.Location));
            changes.Add("ZipCode", newJobCandidateVM.ZipCode, oldJobCandidateVM.ZipCode);
            changes.Add("AssignedTo", GetJobProviderName(newJobCandidateVM.AssignedTo), GetJobProviderName(oldJobCandidateVM.AssignedTo));
            changes.Add("CV", newJobCandidateVM.CVName, oldJobCandidateVM.CVName);
            changes.Add("OnHold", FormatBoolean(newJobCandidateVM.IsOnHold), FormatBoolean(oldJobCandidateVM.IsOnHold));
            changes.Add("PreferredModeOfCommunication", newJobCandidateVM.PreferredModeOfCommunication, oldJobCandidateVM.PreferredModeOfCommunication);
            changes.Add("Skills", ConcatJobCandidateSkill(newJobCandidateVM.Skills), ConcatJobCandidateSkill(oldJobCandidateVM.Skills));
            changes.Add("SalaryOffered", FormatNumber(newJobCandidateVM.SalaryOffered), FormatNumber(oldJobCandidateVM.SalaryOffered));
            changes.Add("IsPermanentAddress", FormatBoolean(newJobCandidateVM.IsPermanentAddress), FormatBoolean(oldJobCandidateVM.IsPermanentAddress));
            changes.Add("ReadyToRelocate", FormatBoolean(newJobCandidateVM.IsReadyToRelocateToJobLocation), FormatBoolean(oldJobCandidateVM.IsReadyToRelocateToJobLocation));
            changes.Add("IsHideCandidateContact", FormatBoolean(newJobCandidateVM.IsHideCandidateContact), FormatBoolean(oldJobCandidateVM.IsHideCandidateContact));
            changes.Add("Gender", newJobCandidateVM.Gender, oldJobCandidateVM.Gender);
            changes.Add("DOB", FormatDate(newJobCandidateVM.DOB), FormatDate(oldJobCandidateVM.DOB));
            changes.Add("IsLookingForJob", FormatBoolean(newJobCandidateVM.IsLookingForJob), FormatBoolean(oldJobCandidateVM.IsLookingForJob));
            var description = $"{loggedInUserName} updated '{oldJobCandidateVM.Name}' for '{jobVM.Title}' job.";
            RecordAuditEvent(AuditEvent.UpdateJobCandidateEvent, oldJobCandidateVM.Id, description, changes, loggedInUserId, teamId);
        }
        public void RecordChangeJobCandidateStatus(Job job, string oldStatus, JobCandidate jobCandidate, ApplicationUser loggedInUser, Guid? teamId)
        {
            var description = $"{loggedInUser.Name} changed status from {oldStatus} to {jobCandidate.Status} of candidate '{jobCandidate.Name}' for job '{job.Title}'.";
            RecordAuditEvent(AuditEvent.ChangeJobCandidateStatus, jobCandidate.Id, description, null, loggedInUser.Id, teamId);
        }

        public void RecordRejectedJobCandidateStatus(Job job, string reason, string reasonDetail, JobCandidate jobCandidate, ApplicationUser loggedInUser, Guid? teamId)
        {
            var changes = new RecordFieldChanges();
            changes.Add("IsRejected", FormatBoolean(true));
            changes.Add("RejectionReason", reason);
            changes.Add("RejectionDetail", reasonDetail);
            var description = $"{loggedInUser.Name} rejected candidate '{jobCandidate.Name}' for job '{job.Title}'.";
            RecordAuditEvent(AuditEvent.RejectJobCandidateStatus, jobCandidate.Id, description, changes, loggedInUser.Id, teamId);
        }
        */
        #endregion

        public List<AuditLog> GetAuditlogs(string referenceId)
        {
            Guid.TryParse(referenceId, out Guid referenceObjectId);
            return RF.AuditLogRepository.GetAuditLogs(referenceObjectId);
        }

    }
}
