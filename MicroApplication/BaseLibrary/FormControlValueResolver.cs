using BaseLibrary.Configurations;
using System.Data;

namespace BaseLibrary
{
    public class DashboardDataSource
    {
        public List<AppForm> Forms { get; private set; }
        public string Name { get; set; }
        public DashboardDataSource(string name)
        {
            Forms = new List<AppForm>();
            Name = name;
        }
    }

    //public class FormControlValueDataSource
    //{
    //    public PageDataStore? City { get; set; }
    //    public PageDataStore? State { get; set; }
    //    public PageDataStore? Country { get; set; }
    //    public AppControl? Control { get; set; }
    //    public ApplicationUser? User { get; set; }
    //    public PageDataStore? Form { get; set; }
    //}

    //public interface IFormControlValueResolver
    //{
    //    string? GetFormControlValue(AppForm form, AppControl control);
    //}
    //public class FormControlValueResolver: IFormControlValueResolver
    //{
    //    public BaseLinqFormDataSource FormControlValueDataSource { get; }
    //    public FormControlValueResolver(BaseLinqFormDataSource formControlValueDataSource)
    //    {
    //        FormControlValueDataSource = formControlValueDataSource;
    //    }
    //    public virtual string? GetFormControlValue(AppForm form, AppControl control)
    //    {
    //        //if (control.ControlIdentifier == BaseControl.Name.ControlIdentifier)
    //        //    return FormControlValueDataSource.Form?.Name;
    //        //if (control.ControlIdentifier == BaseControl.Description.ControlIdentifier)
    //        //    return FormControlValueDataSource.Form?.Description;
    //        //return FormControlValueDataSource.Form?.GetValue(control.ControlIdentifier);
    //        return null;
    //    }
    //}

    

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

        public new List<List<string>> GetDashboardChart()
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
