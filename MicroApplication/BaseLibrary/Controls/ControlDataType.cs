namespace BaseLibrary.Controls
{
    public class ControlDataTypes
    {
        public const string Bool = "bool";  //0 or 1
        public const string Datetime = "datetime"; //2023-10-08 18:48:18.633
        public const string Date = "date"; //2023-10-08 18:48:18.633
        public const string Integer = "int";
        public const string Double = "double";
        public const string String = "string";
        public const string Guid = "Guid";
    }

    public class ControlDataType
    {
        public string Name { get; set; }
        private ControlDataType(string dataType)
        {
            Name = dataType;
        }
        public static ControlDataType Bool = new ControlDataType(ControlDataTypes.Bool);
        public static ControlDataType Datetime = new ControlDataType(ControlDataTypes.Datetime);
        public static ControlDataType Date = new ControlDataType(ControlDataTypes.Date);
        public static ControlDataType Integer = new ControlDataType(ControlDataTypes.Integer);
        public static ControlDataType Double = new ControlDataType(ControlDataTypes.Double);
        public static ControlDataType String = new ControlDataType(ControlDataTypes.String);
        public static ControlDataType Guid = new ControlDataType(ControlDataTypes.Guid);
        public static SmartControlAlignment GetAlignment(string dataType)
        {
            if (dataType == String.Name || dataType == Bool.Name)
                return SmartControlAlignment.Left;
            return SmartControlAlignment.Right;
        }

        private static List<ControlDataType> _types = new List<ControlDataType> { Bool, Datetime, Date, Integer, Double, String, Guid };
        public static List<ControlDataType> GetControlDataTypes()
        {
            return _types;
        }
        public static ControlDataType GetDataType(string dataType)
        {
            var controlTypes = _types;
            return controlTypes.First(ct => ct.Name == dataType);
        }
        public static ControlDataType GetDataTypeFromPrefix(string prefix)
        {
            switch (prefix)
            {
                case DataTypePrefix.Bool:
                    return Bool;
                case DataTypePrefix.Datetime:
                    return Datetime;
                case DataTypePrefix.Date:
                    return Date;
                case DataTypePrefix.Integer:
                    return Integer;
                case DataTypePrefix.Double:
                    return Double;
                case DataTypePrefix.String:
                    return String;
                case DataTypePrefix.Guid:
                    return Guid;
            }
            throw new ValidationException($"No matching data type found for {prefix}.");
        }
    }

    public class DataTypePrefix
    {
        public const string Bool = "BL";
        public const string Datetime = "DT";
        public const string Date = "DA";
        public const string Integer = "IN";
        public const string Double = "DO";
        public const string String = "TX";
        public const string Guid = "GD";
    }
}
