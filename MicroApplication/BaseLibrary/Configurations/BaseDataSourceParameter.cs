using BaseLibrary.Configurations.DataSources;

namespace BaseLibrary.Configurations
{
    public abstract class BaseDataSourceParameter
    {
        #region "Parameter Handling" - This will be delete later
        public static DataSourceParameter LoggedInUser = DataSourceParameter.CreateGuidParameter(Guid.Parse("D62AE27F-CEF0-40EA-B196-3DC302FD8EFB"), "@user");
        public static DataSourceParameter LoggedInUserOrganization = DataSourceParameter.CreateGuidParameter(Guid.Parse("BAC3533A-4DDE-45D9-AEFC-3A76E25BD3A2"), "@organization");

        private List<DataSourceParameter> _parameters = new List<DataSourceParameter>() { LoggedInUser, LoggedInUserOrganization };

        protected virtual DataSourceParameter? GetDataSourceParameter(ControlValue filter)
        {
            return _parameters.FirstOrDefault(f => f.ControlId == filter.ControlId);
        }

        public virtual object? GetParamValue(DataSourceParameter dataSourceParameter, IBaseLibraryServiceFactory serviceFactory, ControlValue filter)
        {
            if (filter.Values.Any() == false)
                return null;
            var filterValue = filter.GetFirstValue();
            switch (dataSourceParameter.DataType.Name)
            {
                case ControlDataTypes.Date:
                    if (filterValue == null)
                        return null;
                    var parts = filterValue.Split('-');
                    return new DateOnly(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
                case ControlDataTypes.Guid:
                    var val = GetImplicitParameters(dataSourceParameter, serviceFactory);
                    if (val != null)
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
        #endregion


        public BaseDataSourceParameter() { }
        public abstract List<MacroDataSourceParameter> GetApplicationMacroDataSourceParameters();
        private List<MacroDataSourceParameter> GetBaseMacroDataSourceParameters()
        {
            return new List<MacroDataSourceParameter>()
            {
                new MacroDataSourceParameter("D62AE27F-CEF0-40EA-B196-3DC302FD8EFB","UserId",null,"@userId"),
                new MacroDataSourceParameter("BAC3533A-4DDE-45D9-AEFC-3A76E25BD3A2","OrganizationId",null,"@organizationId"),
            };
        }
        public List<MacroDataSourceParameter> GetMacroDataSourceParameters()
        {
            var parameters = new List<MacroDataSourceParameter>();
            parameters.AddRange(GetBaseMacroDataSourceParameters());
            parameters.AddRange(GetApplicationMacroDataSourceParameters());
            return parameters;
        }
        public MacroDataSourceParameter GetMacroDataSourceParameterById(Guid id)
        {
            return GetMacroDataSourceParameters().FirstOrDefault(p => p.Id == id)!;
        }
        public MacroDataSourceParameter? GetMacroDataSourceParameterByControlId(Guid controlId)
        {
            return GetMacroDataSourceParameters().FirstOrDefault(p => p.AppControlId == controlId);
        }
        public MacroDataSourceParameter? GetMacroDataSourceParameterBySqlParameterName(string sqlParameterName)
        {
            return GetMacroDataSourceParameters().FirstOrDefault(p => p.SqlParameterName == sqlParameterName);
        }
        public MacroDataSourceParameter? GetMacroDataSourceParameterByName(string name)
        {
            return GetMacroDataSourceParameters().FirstOrDefault(p => p.Name == name);
        }
    }
}

