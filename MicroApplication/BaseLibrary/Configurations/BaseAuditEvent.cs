

using BaseLibrary.Domain.Audit;
using System.Collections.Generic;

namespace BaseLibrary.Configurations
{
    public abstract class BaseAuditEvent
    {
        protected abstract List<AuditEvent> GetApplicationAuditEvents();
        public List<AuditEvent> GetAuditEvents()
        {
            var events = new List<AuditEvent>();
            events.AddRange(GetBaseAuditEvents());
            events.AddRange(GetApplicationAuditEvents());
            return events;
        }
        protected virtual List<AuditEvent> GetBaseAuditEvents()
        {
            return new List<AuditEvent>();
        }
    }

}
