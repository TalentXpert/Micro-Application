namespace BaseLibrary.Domain.PerformanceLogs
{
    public class DeletePerformanceVM
    {
        public List<Guid> Ids { get; set; }

        public DeletePerformanceVM()
        {
            Ids = new List<Guid>();
        }
    }
}
