namespace BaseLibrary.Domain.Audit
{
    public class AuditEvent
    {
        #region Properties
        protected AuditEvent(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        #endregion

        #region Sample
        //public static AuditEvent AddJobEvent => new AuditEvent(1, "Add Job");
        //public static AuditEvent UpdateJobEvent => new AuditEvent(2, "Update Job");
        #endregion


       


    }
}
