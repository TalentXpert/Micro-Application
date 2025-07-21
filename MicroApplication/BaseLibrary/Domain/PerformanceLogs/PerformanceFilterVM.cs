namespace BaseLibrary.Domain.PerformanceLogs
{
    public class PerformanceFilterVM
    {
        public string ControllerName { get; set; }
        public string MethodName { get; set; }
        public string Duration { get; set; }
    }
}
