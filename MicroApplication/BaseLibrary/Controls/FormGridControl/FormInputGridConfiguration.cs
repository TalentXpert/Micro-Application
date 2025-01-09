using BaseLibrary.UI.Controls;

namespace BaseLibrary.Controls.FormGridControl
{
    public class FormInputGridConfiguration
    {
        public List<UIControl> Headers { get; set; } = new List<UIControl>();
        public List<FormInputGridRow> Rows { get; set; }= new List<FormInputGridRow>();
        public FormInputGridRow AddRow()
        {
            var row = new FormInputGridRow(Rows.Count + 1);
            Rows.Add(row);
            return row;
        }
    }

    public class FormInputGridRow
    {
        protected FormInputGridRow() { }
        public FormInputGridRow(int rowNumber) { RowNumber = rowNumber; }
        public int RowNumber { get; set; }
        public Guid? RowID { get; set; }  
        public bool ReadOnly { get; set; }
        public List<FormInputGridCell> Cells { get; set; }=new List<FormInputGridCell>();
        public void AddCell(string controlIdentifier, List<string> values)
        {
            Cells.Add(new FormInputGridCell(controlIdentifier, RowNumber, values));
        }
    }

    public class FormInputGridCell
    {
        protected FormInputGridCell() { }
        public FormInputGridCell(string controlIdentifier,int rowNumber, List<string> values)
        {
            ControlIdentifier = controlIdentifier+"_"+ rowNumber;
            if(values != null) 
                Values = values;
        }

        public string ControlIdentifier { get; set; }
        public string Style { get; set; }
        public List<string> Values { get; set; } = new List<string> ();
    }

    public class FormInputGridOutput
    {
        public List<FormInputGridRow> Rows { get; set; } = new List<FormInputGridRow>();
    }
}
