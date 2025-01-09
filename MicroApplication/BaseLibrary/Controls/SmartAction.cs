namespace BaseLibrary.Controls.Grid
{

    public class SmartActionFormMode
    {
        public string Name { get; set; }
        private SmartActionFormMode(string formMode)
        {
            Name = formMode;
        }

        public static SmartActionFormMode Add = new SmartActionFormMode("Add");
        public static SmartActionFormMode Edit = new SmartActionFormMode("Edit");
        public static SmartActionFormMode View = new SmartActionFormMode("View");
        public static SmartActionFormMode Delete = new SmartActionFormMode("Delete"); 
    }

    public class FormTypes 
    {
        public static string DynamicForm = "DynamicForm";
        public static string SelectFromListForm = "SelectFromListForm";
    }

    public class SmartAction
    {
        public Guid FormId { get; set; }
        public string Text { get; set; }
        public string Param { get; set; }
        public string ActionIdentifier { get; set; }
        public string FormMode { get; set; }
        public string FormType { get; set; }
        public SmartAction(AppForm appForm,string formType, string text, string param, string actionIdentifier, SmartActionFormMode mode)
        {
            FormId = appForm.Id;
            Text = text;
            Param = param;
            ActionIdentifier = actionIdentifier;
            FormMode = mode.Name;
            FormType = formType;
        }
    }
}
