
using BaseLibrary.Configurations.DataSources.LinqDataSources;

namespace MicroAppAPI.Configurations
{

    public class DataSources
    {
        public static List<LinqDataSource> GetDataSources()
        {
            return new List<LinqDataSource>
            {
                new CityDashboardDataSource()
            };
        }
    }
    public class CityDashboardDataSource : LinqDataSource
    {
        public CityDashboardDataSource() : base("City", ApplicationForm.City)
        {
            Forms.Add(ApplicationForm.State);
            Forms.Add(ApplicationForm.Country);
        }
    }
    
    public class UserMgmtFormControlValueResolver : FormControlValueResolver, IFormControlValueResolver
    {
        UserMgmtFormControlValueDataSource dataSource;
        public UserMgmtFormControlValueResolver(UserMgmtFormControlValueDataSource formControlValueDataSource) : base(formControlValueDataSource)
        {
            dataSource = formControlValueDataSource;
        }

        public override string? GetFormControlValue(AppForm form, AppControl control)
        {
            string? value = null;
            if (value == null && form.Id == ApplicationForm.City.Id)
                value = dataSource.City?.GetValue(control.ControlIdentifier);
            if (value == null && form.Id == ApplicationForm.State.Id)
                value = dataSource.State?.GetValue(control.ControlIdentifier);
            if (value == null && form.Id == ApplicationForm.Country.Id)
                value = dataSource.Country?.GetValue(control.ControlIdentifier);
            if (value == null && form.Id == ApplicationForm.Asset.Id)
                value = dataSource.Asset?.GetValue(control.ControlIdentifier);
            if (value == null && form.Id == ApplicationForm.AssetSubgroup.Id)
                value = dataSource.AssetSubcategory?.GetValue(control.ControlIdentifier);
            if (value == null && form.Id == ApplicationForm.AssetGroup.Id)
                value = dataSource.AssetCategory?.GetValue(control.ControlIdentifier);
            if (value == null)
                value = base.GetFormControlValue(form, control);

            if (control.DataType != ControlDataTypes.Guid)
                return value;
            return GetText(control);
        }
        private string? GetText(AppControl control)
        {
            switch (control.ControlIdentifier)
            {
                case ApplicationControls.City: return dataSource.City?.Name;
                case ApplicationControls.State: return dataSource.State?.Name;
                case ApplicationControls.Country: return dataSource.Country?.Name;
                case ApplicationControls.Asset: return dataSource.Asset?.Name;
                case ApplicationControls.AssetGroup: return dataSource.AssetCategory?.Name;
                case ApplicationControls.AssetSubgroup: return dataSource.AssetSubcategory?.Name;
            }
            return null;
        }
    }

}
