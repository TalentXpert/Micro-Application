using BaseLibrary.UserInterface;

namespace BaseLibrary.UserInterface
{
    
    public class GraphType
    {
        public string Type { get; private set; }

        private GraphType(string type)
        {
            Type = type;
        }
        public static GraphType VerticalBar = new GraphType("VerticalBar");
        public static GraphType HorizontalBar = new GraphType("HorizontalBar");
        public static GraphType VerticalLine = new GraphType("VerticalLine");
        public static GraphType HorizontalLine = new GraphType("HorizontalLine");
        public static GraphType Pie = new GraphType("PieChart");
        public static GraphType Bar = new GraphType("Bar");

        public static List<GraphType> GetGraphTypes()
        {
            return new List<GraphType>
            {
                VerticalBar, HorizontalBar, VerticalLine, HorizontalLine, Pie, Bar
            };
        }
    }

    public class GraphDataTypes
    {
        public const string Sum = "Sum";
        public const string Count = "Count";
        public const string Average = "Average";
    }

    public class GraphDataType
    {
        public string Type { get; private set; }

        private GraphDataType(string type)
        {
            Type = type;
        }
        public static GraphDataType Sum = new GraphDataType(GraphDataTypes.Sum);
        public static GraphDataType Count = new GraphDataType(GraphDataTypes.Count);
        public static GraphDataType Average = new GraphDataType(GraphDataTypes.Average);
    }



   

    
}
