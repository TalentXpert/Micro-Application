using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Utilities.Formaters
{
    public class WordTagFormatter
    {
        public Table PrepareDocTable(DataTable dataTable)
        {
            // Initialize the table
            Table table = new Table();

            // Create table properties (optional: adds default borders)
            TableProperties tableProps = new TableProperties(
                new TableBorders(
                    new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }
                )
            );
            table.AppendChild(tableProps);

            TableRow headerRow = new TableRow();
            foreach (DataColumn column in dataTable.Columns)
            {
                TableCell cell = new TableCell();

                // Add header text with bold formatting
                Paragraph paragraph = new Paragraph();
                Run run = new Run();
                RunProperties runProps = new RunProperties(new Bold());
                run.AppendChild(runProps);
                run.AppendChild(new Text(column.ColumnName));
                paragraph.AppendChild(run);

                cell.AppendChild(paragraph);
                headerRow.AppendChild(cell);
            }
            table.AppendChild(headerRow);


            foreach (DataRow row in dataTable.Rows)
            {
                TableRow dataRow = new TableRow();
                foreach (var item in row.ItemArray)
                {
                    TableCell cell = new TableCell();

                    // Add text to the cell
                    Paragraph paragraph = new Paragraph(new Run(new Text(item?.ToString() ?? "")));
                    cell.AppendChild(paragraph);

                    dataRow.AppendChild(cell);
                }
                table.AppendChild(dataRow);
            }

            return table;
        }


        // Helper method to create a table cell containing text
        public static TableCell CreateTextCell(string text)
        {
            TableCell cell = new TableCell();
            Paragraph paragraph = new Paragraph();
            Run run = new(new Text(text));

            paragraph.Append(run);
            cell.Append(paragraph);

            return cell;
        }
    }
}
