namespace BaseLibrary.Controls.Charts
{
    public class ChartDataTypes
    {
        public const string Sum = "Sum";
        public const string Count = "Count";
        public const string Average = "Average";
    }

    public class ChartDataType
    {
        public string Type { get; private set; }

        private ChartDataType(string type)
        {
            Type = type;
        }
        public static ChartDataType Sum = new ChartDataType(ChartDataTypes.Sum);
        public static ChartDataType Count = new ChartDataType(ChartDataTypes.Count);
        public static ChartDataType Average = new ChartDataType(ChartDataTypes.Average);
    }
}
