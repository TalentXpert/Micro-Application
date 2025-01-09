
using BaseLibrary.Configurations;

namespace BaseLibrary.Controls.Grid
{
    public class GridHeader
    {
        public string HeaderIdentifier { get; protected set; }
        public int Position { get; protected set; }
        public string HeaderText { get; protected set; } // visible to  user and can be localized. do not write any logic on it. 
        public string Alignment { get; protected set; } 
        public string DataType { get; protected set; }
        public bool IsVisible { get; protected set; } //is header visible to user
        public int MinWidth { get; protected set; } // mini width required for this column.
        public bool IsFixed { get; protected set; }
        public bool IsSingleLine { get; protected set; }
        public List<SmartAction> Actions { get; protected set; } // options for menus on this column.
       
        public GridHeader(int position, string headerIdentifier,  string headerText, ControlDataType dataType, SmartControlAlignment alignment, bool isSingleLine) 
        {
            HeaderIdentifier = headerIdentifier;
            Position = position;
            HeaderText = headerText;
            Alignment = alignment.Name;
            DataType = dataType.Name;
            Actions = new List<SmartAction>();
            IsVisible = true;
            IsSingleLine = isSingleLine;
            IsFixed= false;
        }
        public GridHeader(int position, AppFormControl formControl,AppControl control, PageGridHeader pageGridHeader)
        {
            HeaderIdentifier = control.ControlIdentifier;
            Position = position;
            if (string.IsNullOrWhiteSpace(pageGridHeader.HeaderText))
                HeaderText = formControl.GetDisplayLabel();
            else
                HeaderText = pageGridHeader.HeaderText;
            Alignment = ControlDataType.GetAlignment(control.DataType).Name;
            DataType = ControlDataType.GetDataType(control.DataType).Name;
            Actions = pageGridHeader.Actions;
            IsVisible = true;
            IsSingleLine = true;
            IsFixed = false;
        }

        public GridHeader AddActions(List<SmartAction> actions)
        {
            if (actions != null && actions.Any())
                Actions.AddRange(actions);
            return this;
        }
        public GridHeader Hide()
        {
            IsVisible = false;
            return this;
        }
        /// <summary>
        /// User can not hide this column or Field 
        /// </summary>
        public GridHeader MakeFixed()
        {
            IsFixed = true;
            return this;
        }
        /// <summary>
        /// Render on single row in card mode
        /// </summary>
        public GridHeader RenderOnSingleLine()
        {
            IsSingleLine = true;
            return this;
        }
        public GridHeader SetMinimumWidth(int minWidth)
        {
            MinWidth = minWidth;
            return this;
        }
        public void SetPosition(int position)
        {
            Position= position;
        }
        public bool IsMandatory()
        {
            if (Actions.Any())
                return true;
            return false;
        }
        
    }
}
