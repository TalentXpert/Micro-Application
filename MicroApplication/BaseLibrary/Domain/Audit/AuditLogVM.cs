namespace BaseLibrary.Domain.Audit
{
    public class AuditLogVM
    {
        public string Event { get; set; }
        public string Title { get; set; }
        public string EventDate { get; set; }
        public string Detail { get; set; }

        public void TrimAllStrings()
        {
            Event = TrimString.Trim(Event);
            Title = TrimString.Trim(Title);
            EventDate = TrimString.Trim(EventDate);
            Detail = TrimString.Trim(Detail);
        }
    }

}
