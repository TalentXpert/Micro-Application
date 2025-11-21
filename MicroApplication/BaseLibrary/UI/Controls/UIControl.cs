namespace BaseLibrary.UI.Controls
{

    /// <summary>
    /// This class represent a user interface form control that will be render on a form
    /// </summary>
    public class UIControl : MicroControl
    {
        public bool IsEditable { get; protected set; } // whether user can edit data or not once a data is entered 
        public bool IsMandatory { get; protected set; }
        public bool IsUnique { get; protected set; } //if this value should be unique for an organization.
        public long? Minimum { get; protected set; } //character length of a input field. 
        public long? Maximum { get; protected set; }//character length of a input field. 
        public bool IsGlobalControl { get; protected set; } // true means this will display on top of grid control and read only on data entry form, this can be changed while adding this control to a form in form configuration
        public bool IsFormLayoutOwner { get; protected set; } //this enable this control to change form layout
        public bool IsPageRefreshNeeded { get; protected set; } // this will decide if we need full page refresh.
        public bool IsSingleLine { get; protected set; } //should be redered in single line on card or list view. 
        public int ListViewLine { get; protected set; }
        public int ListViewLinePosition { get; protected set; }
        public bool IsComplexControl { get; protected set; }=false;
        //complexcontrol 
        public string ControlConfigurationJson { get; set; } // this json string will help to render a comlex control

        public UIControl(AppControl appControl, List<string>? values) : base(appControl, values)
        {
            SetEditable(true);

            IsMandatory = false;
            IsUnique = false;
            Minimum = null;
            Maximum = null;

            IsGlobalControl = false;
            IsFormLayoutOwner = appControl.IsFormLayoutOwner;
            IsPageRefreshNeeded = false;
            IsSingleLine = true;
        }

        public UIControl(AppControl appControl, AppFormControl appFormControl, List<string>? values) : base(appControl, values)
        {

            SetEditable(appFormControl.IsEditable);

            IsMandatory = appFormControl.IsMandatory;
            IsUnique = appFormControl.IsUnique;
            Minimum = appFormControl.Minimum;
            Maximum = appFormControl.Maximum;

            IsGlobalControl = appFormControl.GetIsGlobalControl();
            IsFormLayoutOwner = appControl.IsFormLayoutOwner;
            IsPageRefreshNeeded = false;
            
            IsSingleLine = appFormControl.IsSingleLine;
            Tooltip = appFormControl.Tooltip;
        }
        
        private void SetEditable(bool isEditable)
        {
            if (string.IsNullOrWhiteSpace(Value)) //if control is empty then we can not 
                IsEditable = true;
            else
                IsEditable = isEditable;
        }
        
        public UIControl SetPageRefreshNeeded()
        {
            IsPageRefreshNeeded = true;
            return this;
        }
    }
}
