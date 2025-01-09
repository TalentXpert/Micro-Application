namespace BaseLibrary.Controls.Charts
{
    public class ChartType
    {
        public string Type { get; private set; }

        private ChartType(string type)
        {
            Type = type;
        }
        public static ChartType VerticalBar = new ChartType("VerticalBar");
        public static ChartType HorizontalBar = new ChartType("HorizontalBar");
        public static ChartType VerticalLine = new ChartType("VerticalLine");
        public static ChartType HorizontalLine = new ChartType("HorizontalLine");
        public static ChartType Pie = new ChartType("PieChart");
        public static ChartType Bar = new ChartType("Bar");

        public static List<ChartType> GetGraphTypes()
        {
            return new List<ChartType>
            {
                VerticalBar, HorizontalBar, VerticalLine, HorizontalLine, Pie, Bar
            };
        }
    }






}
