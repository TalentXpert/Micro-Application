namespace BaseLibrary.Domain.PerformanceLogs
{
    public class PerformanceLogVM
    {
        public Guid Id { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public int Duration { get; set; }
        public DateTime ExecutedOn { get; set; }
        public string User { get; set; }
        public string Detail { get; set; }

    }
}
