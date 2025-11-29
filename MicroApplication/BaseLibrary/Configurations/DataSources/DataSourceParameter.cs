using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Configurations.DataSources
{
    public class DataSourceParameter
    {
        public Guid ControlId { get; set; } = Guid.Empty;
        public string ControlIdentifier { get; set; } = string.Empty;
        public ControlDataType DataType { get; set; }

        protected DataSourceParameter(Guid controlId, string controlIdentifier, ControlDataType dataType)
        {
            ControlId = controlId;
            ControlIdentifier = controlIdentifier;
            DataType = dataType;
        }

        public static DataSourceParameter CreateDateParameter(Guid controlId, string controlIdentifier)
        {
            return new DataSourceParameter(controlId, controlIdentifier, ControlDataType.Date);
        }
        public static DataSourceParameter CreateDateTimeParameter(Guid controlId, string controlIdentifier)
        {
            return new DataSourceParameter(controlId, controlIdentifier, ControlDataType.Datetime);
        }
        public static DataSourceParameter CreateGuidParameter(Guid controlId, string controlIdentifier)
        {
            return new DataSourceParameter(controlId, controlIdentifier, ControlDataType.Guid);
        }
    }

    public class MacroDataSourceParameter
    {
        protected MacroDataSourceParameter() { }
        public MacroDataSourceParameter(string id, string name, Guid? appControlId, string? sqlParameterName)
        {
            Id = Guid.Parse(id);
            Name = name;
            AppControlId = appControlId;
            SqlParameterName = sqlParameterName;
        }
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// If this has value then this parameter is linked to an AppControl to get filter value. If null, then this is an implicit parameter like logged in user id or organization id or global filter value.
        /// </summary>
        public Guid? AppControlId { get; set; }
        /// <summary>
        /// This is the actual SQL parameter name used in the query text like @organizationId, @userId
        /// </summary>
        public string? SqlParameterName { get; set; } 
        
    }
}
