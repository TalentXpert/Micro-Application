using BaseLibrary.Controls.Charts;

namespace BaseLibrary.Configurations.DataSources.LinqDataSources
{
    public class GraphBuilder : CleanCode
    {
        public GraphBuilder(List<IFormControlValueResolver> formControlValueResolvers, AppForm primaryForm, AppControl aggregateControl, AppControl dataControl, ChartDataType graphDataType)
        {
            FormControlValueResolvers = formControlValueResolvers;
            Form = primaryForm;
            AggregateControl = aggregateControl;
            DataControl = dataControl;
            GraphDataType = graphDataType;
        }

        private List<IFormControlValueResolver> FormControlValueResolvers { get; }
        private AppForm Form { get; }
        //control on which data need to be aggregated like country, city or other grouping control. 
        private AppControl AggregateControl { get; }
        //control that will hold data to be aggregated like population, area or other value. 
        private AppControl DataControl { get; }
        private ChartDataType GraphDataType { get; }

        public List<List<string>> GetDashboardChart()
        {
            var data = new List<List<string>>();
            var dataDictionary = new Dictionary<string, decimal>();
            foreach (var resolver in FormControlValueResolvers)
            {
                //get value of both controls 
                var value = resolver.GetFormControlValue(Form, DataControl);
                var aggregateValue = resolver.GetFormControlValue(Form, AggregateControl);
                if (aggregateValue == null) continue;

                //do we have entry for aggregated control value 
                if (dataDictionary.ContainsKey(aggregateValue) == false)
                    dataDictionary[aggregateValue] = 0.0m;

                if (IsNotNullOrEmpty(value) && decimal.TryParse(value, out decimal decimalValue))
                {
                    switch (GraphDataType.Type)
                    {
                        case ChartDataTypes.Average:
                        case ChartDataTypes.Sum:
                            dataDictionary[aggregateValue] += decimalValue;
                            break;
                        case ChartDataTypes.Count:
                            dataDictionary[aggregateValue] += 1;
                            break;
                    }
                }
            }
            //if need average then divide total with number of records
            if (GraphDataType.Type == ChartDataTypes.Average)
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
