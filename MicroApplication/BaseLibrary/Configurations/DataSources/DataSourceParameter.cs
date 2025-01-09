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
}
