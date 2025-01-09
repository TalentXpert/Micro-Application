using BaseLibrary.Controls.Grid;
using ClosedXML.Excel;


namespace BaseLibrary.Utilities.Excels
{
    public class ExcelExporter : CleanCode
    {
        public byte[] GetExcel(GridModel model,string dateFormat,List<string> ignoreHeaders)
        {
            using (var workbook = new XLWorkbook())
            {
                if(ignoreHeaders==null) ignoreHeaders = new List<string>();
                var worksheet = workbook.Worksheets.Add("Export");
                var headers = model.Headers;
                int currentRow;
                int index;

                currentRow = 1;
                index = 1;

                // var columns = dataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName.ToLower());
                
                foreach (var col in headers)
                {
                    if (col.IsVisible == false)
                        continue;
                    if (ignoreHeaders.Any(c => AreEqualsIgnoreCase(c, col.HeaderIdentifier)))
                        continue;
                    worksheet.Cell(currentRow, index).Value = col.HeaderText;
                    index++;
                }

                var rows = model.Rows;
                DateTime d;
                foreach (var row in rows)
                {
                    currentRow++;
                    int pos = 0;
                    int column = 0;
                    foreach (var col in headers)
                    {
                        column += 1;
                        if (col.IsVisible == false)
                            continue;
                        if (ignoreHeaders.Any(c => AreEqualsIgnoreCase(c, col.HeaderIdentifier)))
                            continue;
                        pos += 1;
                        var value = row[column - 1]?.T;
                        var formattedValue = value;
                        if (value == null) continue;
                        switch (col.DataType)
                        {
                            case ControlDataTypes.Bool:
                            case ControlDataTypes.Datetime:
                                if (DateTime.TryParse(value, out d))
                                    formattedValue=DateTimeHelper.GetDateFormattedString(d, dateFormat, true);
                                break;
                            case ControlDataTypes.Date:
                                if (DateTime.TryParse(value, out d))
                                    formattedValue = DateTimeHelper.GetDateFormattedString(d, dateFormat, false);
                                break;
                            case ControlDataTypes.Integer:
                            case ControlDataTypes.Double:
                            case ControlDataTypes.String:
                            case ControlDataTypes.Guid:
                                break;
                        }
                        worksheet.Cell(currentRow, pos).Value = formattedValue;
                    }
                }

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();

                byte[] content=null;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    content = stream.ToArray();
                }
                return content;  //File(content, "application/vnd.openxmlformats-officedocument.spreadheetml.sheet", "data.xlsx");
            }
        }
    }
}
