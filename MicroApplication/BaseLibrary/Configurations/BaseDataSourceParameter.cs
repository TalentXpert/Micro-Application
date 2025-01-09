using BaseLibrary.Configurations.DataSources;

namespace BaseLibrary.Configurations
{
    public class BaseDataSourceParameter
    {
        public static DataSourceParameter LoggedInUser = DataSourceParameter.CreateGuidParameter(Guid.Parse("D62AE27F-CEF0-40EA-B196-3DC302FD8EFB"), "@user");
        public static DataSourceParameter LoggedInUserOrganization = DataSourceParameter.CreateGuidParameter(Guid.Parse("BAC3533A-4DDE-45D9-AEFC-3A76E25BD3A2"), "@organization");

        private  List<DataSourceParameter> _parameters = new List<DataSourceParameter>() { LoggedInUser, LoggedInUserOrganization };

        protected virtual DataSourceParameter? GetDataSourceParameter(ControlValue filter)
        {
            return _parameters.FirstOrDefault(f => f.ControlId == filter.ControlId);
        }

        public virtual object? GetParamValue(DataSourceParameter dataSourceParameter, IBaseLibraryServiceFactory serviceFactory, ControlValue filter) 
        {
            if(filter.Values.Any()==false)
                return null;
            var filterValue = filter.GetFirstValue();
            switch (dataSourceParameter.DataType.Name)
            {
                case ControlDataTypes.Date:
                    if(filterValue==null)
                        return null;
                    var parts = filterValue.Split('-');
                    return new DateOnly(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
                case ControlDataTypes.Guid:
                    var val = GetImplicitParameters(dataSourceParameter, serviceFactory);
                    if (val!=null) 
                        return val;
                    return Convertor.ToGuid(filterValue);
            }
            return null;
        }

        protected virtual Guid? GetImplicitParameters(DataSourceParameter dataSourceParameter, IBaseLibraryServiceFactory serviceFactory)
        {
            switch (dataSourceParameter.ControlIdentifier)
            {
                case "@user": return serviceFactory.LoggedInUser?.Id;
                case "@organization": return serviceFactory.LoggedInUser?.OrganizationId;
            }
            return null;
        }
    }
}

