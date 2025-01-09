namespace BaseLibrary.Controls.Forms
{
    public static class ControlReader
    {
        public static ControlValue? GetControl(AppControl appControl, List<ControlValue> ControlValues)
        {
            if (C.IsNull(appControl))
                return null;

            if (C.HasNoChild(ControlValues))
                return null;

            return ControlValues.FirstOrDefault(c => c.IsMatching(appControl));
        }

        public static ControlValue? GetControl(string controlIdentifier, List<ControlValue> ControlValues)
        {
            if (C.IsNullOrEmpty(controlIdentifier))
                return null;

            if (C.HasNoChild(ControlValues))
                return null;

            return ControlValues.FirstOrDefault(c => c.IsMatching(controlIdentifier));
        }

        public static List<string> GetControlValues(AppControl appControl, List<ControlValue> ControlValues)
        {
            var control = GetControl(appControl, ControlValues);
            if (control != null)
                return control.Values;

            return new List<string>();
        }

        public static List<string> GetControlValues(string controlIdentifier, List<ControlValue> ControlValues)
        {
            var control = GetControl(controlIdentifier, ControlValues);
            if (control != null)
                return control.Values;

            return new List<string>();
        }

        public static string? GetControlFirstValue(AppControl appControl, List<ControlValue> ControlValues)
        {
            return GetControl(appControl, ControlValues)?.GetFirstValue();
        }
        public static string? GetControlFirstValue(string controlIdentifier, List<ControlValue> ControlValues)
        {
            return GetControl(controlIdentifier, ControlValues)?.GetFirstValue();
        }
        
        public static string? GetControlFirstValue(this List<ControlValue> controlValues, string controlIdentifier)
        {
            return GetControl(controlIdentifier, controlValues)?.GetFirstValue();
        }
        public static string? GetControlFirstValue(this List<ControlValue> controlValues, AppControl appControl)
        {
            return GetControl(appControl, controlValues)?.GetFirstValue();
        }

        public static Guid? GetControlFirstValueAsGuid(this List<ControlValue> controlValues, AppControl appControl)
        {
            var val = GetControlFirstValue(appControl, controlValues);
            if (C.IsNull(val))
                return null;
            return Convertor.ToGuid(val);
        }
        public static Guid? GetControlFirstValueAsGuid(this List<ControlValue> controlValues, string controlIdentifier)
        {
            var val = GetControlFirstValue(controlIdentifier, controlValues);
            if (C.IsNull(val))
                return null;
            return Convertor.ToGuid(val);
        }
        public static int GetControlFirstValueAsInt(AppControl appControl, List<ControlValue> ControlValues)
        {
            string value = GetControl(appControl, ControlValues)?.GetFirstValue();
            if (!C.IsNull(value))
                return Convert.ToInt16(value);
            return 0;
        }
        public static double GetControlFirstValueAsDouble(AppControl appControl, List<ControlValue> ControlValues)
        {
            string value = GetControl(appControl, ControlValues)?.GetFirstValue();
            if (!C.IsNull(value))
                return Convert.ToDouble(value);
            return 0;
        }
        public static DateTime GetControlFirstValueAsDate(AppControl appControl, List<ControlValue> ControlValues)
        {
            string value = GetControl(appControl, ControlValues)?.GetFirstValue();
            if (!C.IsNull(value))
                return Convert.ToDateTime(value);
            return new DateTime();
        }

    }

}
