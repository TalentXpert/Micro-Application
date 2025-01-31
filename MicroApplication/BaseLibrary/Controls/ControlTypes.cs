using BaseLibrary.UI.Controls;

namespace BaseLibrary.Controls
{
    public class ControlTypes
    {
        public string Name { get; set; }
        private ControlTypes(string name)
        {
            Name = name;
        }
        public static ControlTypes TextBox = new ControlTypes("TextBox");
        public static ControlTypes TextArea = new ControlTypes("TextArea");
        public static ControlTypes DatePicker = new ControlTypes("DatePicker");
        public static ControlTypes Dropdown = new ControlTypes("Dropdown");
        public static ControlTypes Typeahead = new ControlTypes("Typeahead");
        public static ControlTypes Label = new ControlTypes("Label");
        public static ControlTypes MultipleSelection = new ControlTypes("MultipleSelection");
        public static ControlTypes MultipleSelectGrid = new ControlTypes("MultiSelectGrid");
        public static ControlTypes Password = new ControlTypes("Password");
        public static ControlTypes InputGrid = new ControlTypes("InputGrid");
        public static ControlTypes RadioButton = new ControlTypes("RadioButton");
        public static ControlTypes UploadFile = new ControlTypes("UploadFile");
        public static ControlTypes GetControlType(string controlType)
        {
            var controlTypes = GetControlTypes();
            return controlTypes.First(c=> c.Name == controlType);
        }
        public static List<ControlTypes> GetControlTypes()
        {
            var controlTypes = new List<ControlTypes> { TextArea, TextBox, DatePicker, Dropdown, Typeahead, Label, MultipleSelection, MultipleSelectGrid , Password, InputGrid, RadioButton, UploadFile };
            return controlTypes;
        }
        public static bool IsDropdownControl(string controlType)
        {
            return controlType == Dropdown.Name || controlType == RadioButton.Name;
        }

        public static bool IsOptionNeeded(MicroControl control)
        {
            return ControlTypes.IsDropdownControl(control.ControlType);
        }
        public static bool IsComplexControl(MicroControl control)
        {
            return IsComplexControl(control.ControlType);
        }
        public static bool IsComplexControl(string controlType)
        {
            if (controlType == InputGrid.Name)
                return true;
            return false;
        }
    }
}
