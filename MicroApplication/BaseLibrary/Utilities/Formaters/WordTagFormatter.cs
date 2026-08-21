using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
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

        //https://learn.microsoft.com/en-us/office/open-xml/word/how-to-insert-a-table-into-a-word-processing-document?tabs=cs-0%2Ccs-1%2Ccs-2%2Ccs-3%2Ccs-4%2Ccs
        public static void Main(Body body)
        {
            string filePath = @"C:\path\to\your\document.docx";

            // Open the document for editing
            //using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, true))
            {
                

                // 1. Create a Page Break to force a new page
                Paragraph pageBreakParagraph = new Paragraph(
                    new Run(
                        new Break() { Type = BreakValues.Page }
                    )
                );
                body.AppendChild(pageBreakParagraph);

                // 2. Create the Table object
                Table table = new Table();

                // 3. Define Table Properties (Borders & Styling)
                TableProperties tblProps = new TableProperties(
                    new TableStyle() { Val = "TableGrid" }, // Uses default Word grid style
                    new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct } // 100% width
                );
                table.AppendChild(tblProps);

                // 4. Populate rows and cells
                string[,] tableData = new string[,]
                {
                { "Header 1", "Header 2", "Header 3" },
                { "Row 1, Cell 1", "Row 1, Cell 2", "Row 1, Cell 3" }
                };

                for (int i = 0; i < tableData.GetLength(0); i++)
                {
                    TableRow row = new TableRow();

                    for (int j = 0; j < tableData.GetLength(1); j++)
                    {
                        // Create cell text wrapper
                        Text textNode = new Text(tableData[i, j]);
                        Run runNode = new Run(textNode);
                        Paragraph paragraphNode = new Paragraph(runNode);
                        TableCell cell = new TableCell(paragraphNode);

                        // Add cell to the current row
                        row.AppendChild(cell);
                    }

                    // Add row to the table
                    table.AppendChild(row);
                }

                // 5. Append the table onto the new page
                body.AppendChild(table);

                // Save modifications automatically via using block disposal
                //Console.WriteLine("Successfully added a new page with a table!");
            }
        }
    }

    #region 

    /// <summary>
    /// how to add a table in docx file in c# that replaces a tag or text
    /// </summary>
    public class Program
    {
        static void Main()
        {
            string filePath = @"C:\Temp\template.docx";
            string tagToReplace = "{{MyTableTag}}";

            ReplaceTagWithTable(filePath, tagToReplace);
        }

        public static void ReplaceTagWithTable(string docPath, string tag)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(docPath, true))
            {
                var body = wordDoc.MainDocumentPart.Document.Body;

                // Find the specific text node containing the target placeholder tag
                Text targetTextNode = body.Descendants<Text>().FirstOrDefault(t => t.Text.Contains(tag));

                if (targetTextNode != null)
                {
                    // Navigate up to find the parent Paragraph containing this text node
                    Paragraph parentParagraph = targetTextNode.Ancestors<Paragraph>().FirstOrDefault();

                    if (parentParagraph != null)
                    {
                        // 1. Generate the table structure
                        Table table = CreateSampleTable();

                        // 2. Insert the table directly after the placeholder paragraph
                        parentParagraph.InsertAfterSelf(table);

                        // 3. Remove or clear the tag text
                        targetTextNode.Text = targetTextNode.Text.Replace(tag, "");

                        // If the paragraph is now completely empty, remove it to prevent a blank line
                        if (string.IsNullOrWhiteSpace(parentParagraph.InnerText))
                        {
                            parentParagraph.Remove();
                        }

                        // Save modifications back to the document container
                        wordDoc.MainDocumentPart.Document.Save();
                    }
                }
            }
        }

        private static Table CreateSampleTable()
        {
            Table table = new Table();

            // Define explicit borders for the table layout
            TableProperties tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }
                )
            );
            table.AppendChild(tblProp);

            // Populate Table Header Row
            TableRow headerRow = new TableRow();
            headerRow.Append(CreateCell("ID", isHeader: true), CreateCell("Product Name", isHeader: true));
            table.Append(headerRow);

            // Populate Data Rows
            TableRow row1 = new TableRow();
            row1.Append(CreateCell("101"), CreateCell("Cloud Storage Subscription"));
            table.Append(row1);

            TableRow row2 = new TableRow();
            row2.Append(CreateCell("102"), CreateCell("AI Api Access Token"));
            table.Append(row2);

            return table;
        }

        private static TableCell CreateCell(string cellText, bool isHeader = false)
        {
            TableCell cell = new TableCell();
            Paragraph paragraph = new Paragraph();
            Run run = new Run();

            // Apply bolding stylization if specified as a header item
            if (isHeader)
            {
                RunProperties runProps = new RunProperties(new Bold());
                run.Append(runProps);
            }

            run.Append(new Text(cellText));
            paragraph.Append(run);
            cell.Append(paragraph);

            return cell;
        }
    }

    #endregion 
}

