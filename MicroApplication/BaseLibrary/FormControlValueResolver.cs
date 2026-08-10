
namespace BaseLibrary
{
    public class DashboardDataSource
    {
        public List<AppForm> Forms { get; private set; }
        public string Name { get; set; }
        public DashboardDataSource(string name)
        {
            Forms = [];
            Name = name;
        }
    }

    public class GraphBuilder : CleanCode
    {
        public GraphBuilder(List<IFormControlValueResolver> formControlValueResolvers, AppForm primaryForm, AppControl aggregateControl, AppControl dataControl, GraphDataType graphDataType)
        {
            FormControlValueResolvers = formControlValueResolvers;
            Form = primaryForm;
            AggregateControl = aggregateControl;
            DataControl = dataControl;
            GraphDataType = graphDataType;
        }

        private List<IFormControlValueResolver> FormControlValueResolvers { get; }
        private AppForm Form { get; }
        private AppControl AggregateControl { get; }
        private AppControl DataControl { get; }
        private GraphDataType GraphDataType { get; }

        public List<List<string>> GetDashboardChart()
        {
            var data = new List<List<string>>();
            var dataDictionary = new Dictionary<string, decimal>();
            foreach (var resolver in FormControlValueResolvers)
            {
                var value = resolver.GetFormControlValue(Form, DataControl);
                var aggregateValue = resolver.GetFormControlValue(Form, AggregateControl);
                if (aggregateValue == null) continue;
                if (dataDictionary.ContainsKey(aggregateValue) == false)
                    dataDictionary[aggregateValue] = 0.0m;
                if (IsNotNullOrEmpty(value) && decimal.TryParse(value, out decimal decimalValue))
                {
                    switch (GraphDataType.Type)
                    {
                        case GraphDataTypes.Average:
                        case GraphDataTypes.Sum:
                            dataDictionary[aggregateValue] += decimalValue;
                            break;
                        case GraphDataTypes.Count:
                            dataDictionary[aggregateValue] += 1;
                            break;
                    }
                }
            }
            if (GraphDataType.Type == GraphDataTypes.Average)
                foreach (var key in dataDictionary.Keys)
                    dataDictionary[key] = dataDictionary[key] / dataDictionary.Count;
            

            foreach (var key in dataDictionary.Keys)
            {
                var d = new List<string>();
                d.Add(key);
                d.Add(dataDictionary[key].ToString());
                data.Add(d);
            }

            return data;
        }

    }
}
