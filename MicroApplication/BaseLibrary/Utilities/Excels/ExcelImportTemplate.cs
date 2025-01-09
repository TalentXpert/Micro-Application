using BaseLibrary.Domain;
using ClosedXML.Excel;

namespace BaseLibrary.Utilities.Excels
{

    public class ImportExcelHeader
    {
        public string Header { get; set; }
        public bool IsMandatory { get; set; }
        public List<string> Lookups { get; set; }
        public string DefaultData { get; set; }
        public ImportExcelHeader(string header, bool isMandatory) { Header = header; IsMandatory = isMandatory; Lookups = new List<string>(); }
        //public ImportExcelHeader AddBoolLookups(string language)
        //{
        //    Lookups.AddRange(new List<string> { "", WordTranslator.Translate("Yes", language), WordTranslator.Translate("No", language) });
        //    return this;
        //}
        public ImportExcelHeader AddBoolLookups(string language)
        {
            Lookups.AddRange(new List<string> { "", "Yes", "No" });
            return this;
        }
        public ImportExcelHeader AddLookups(List<string> lookups)
        {
            Lookups.AddRange(lookups);
            return this;
        }
        public void AddDefaultData(string data)
        {
            Lookups.Clear();
            DefaultData = data;
        }
    }

    public class ExcelImportTemplate
    {
        XLWorkbook Workbook { get; }
        IXLWorksheet ImportWorkSheet { get; }
        IXLWorksheet LookupWorkSheet { get; }
        List<ImportExcelHeader> ImportExcelHeaders { get; }
        public bool AddLookupDropdown { get; }

        public ExcelImportTemplate(string worksheetName, List<ImportExcelHeader> headers, bool showLookup = false, bool addLookupDropdown = true)
        {
            Workbook = new XLWorkbook();
            ImportWorkSheet = Workbook.Worksheets.Add(worksheetName);
            LookupWorkSheet = Workbook.Worksheets.Add("Lookup");
            if (showLookup == false)
                LookupWorkSheet.Hide();
            ImportExcelHeaders = headers;
            AddLookupDropdown = addLookupDropdown;
        }

        public byte[] CreateImportTemplate(int rows)
        {
            ImportExcelHeaders.Insert(0, new ImportExcelHeader("Id", false));
            AddHeaderRow();

            int columIndex = 1;
            foreach (var header in ImportExcelHeaders)
            {
                for (int i = 2; i <= rows + 1; i++)
                {
                    if (header.Header == "Id")
                    {
                        ImportWorkSheet.Cell(i, columIndex).Value = IdentityGenerator.NewSequentialGuid().ToString();
                    }
                    if (string.IsNullOrWhiteSpace(header.DefaultData)==false)
                    {
                        ImportWorkSheet.Cell(i, columIndex).Value = header.DefaultData;
                    }
                }
                columIndex += 1;
            }

            ImportWorkSheet.Columns().AdjustToContents(15.0, 100.0);
            ImportWorkSheet.Rows().AdjustToContents();

            LookupWorkSheet.Columns().AdjustToContents();
            LookupWorkSheet.Rows().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                Workbook.SaveAs(stream);
                var content = stream.ToArray();
                return content;
            }
        }

        private int AddHeaderRow()
        {
            int columIndex = 1;
            foreach (var header in ImportExcelHeaders)
            {
                FormatHeaderCell(ImportWorkSheet.Cell(1, columIndex), header);
                if (header.Lookups.Any())
                {
                    var rowMaxRange = AddLookup(columIndex, header.Lookups, header.Header);
                    var columnExcelHeading = ExcelHeaderMapping.GetColumnExcelHeading(columIndex);
                    if (columnExcelHeading != null && AddLookupDropdown)
                    {
                        var rangeAddress = $"{columnExcelHeading}1:{columnExcelHeading}{rowMaxRange}";
                        ImportWorkSheet.Column(columIndex).CreateDataValidation().List(LookupWorkSheet.Range(rangeAddress), true);
                    }
                }
                columIndex += 1;
            }

            return columIndex;
        }

        public byte[] GenerateExcel(List<Dictionary<string, string>> errorRows)
        {
            int rowIndex = 1;

            ImportExcelHeaders.Add(new ImportExcelHeader("Error", false));
            AddHeaderRow();

            foreach (var row in errorRows)
            {
                int columIndex = 1;
                rowIndex += 1;
                foreach (var h in ImportExcelHeaders)
                {
                    ImportWorkSheet.Cell(rowIndex, columIndex).Value = row[h.Header];
                    columIndex += 1;
                }
            }

            LookupWorkSheet.Columns().AdjustToContents(15.0, 100.0);
            LookupWorkSheet.Rows().AdjustToContents();

            LookupWorkSheet.Columns().AdjustToContents();
            LookupWorkSheet.Rows().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                Workbook.SaveAs(stream);
                var content = stream.ToArray();
                return content;
            }
        }
        private int AddLookup(int columIndex, List<string> lookups, string header)
        {
            if (lookups != null && lookups.Any())
            {
                int rowIndex = 1;
                if (AddLookupDropdown == false)
                {
                    LookupWorkSheet.Cell(rowIndex, columIndex).Value = header;
                    rowIndex += 1;
                }
                foreach (var lookupValue in lookups)
                {
                    if (string.IsNullOrWhiteSpace(lookupValue))
                        continue;
                    LookupWorkSheet.Cell(rowIndex, columIndex).Value = lookupValue;
                    rowIndex += 1;
                }
                return rowIndex - 1;
            }
            return 0;
        }

        private static void FormatHeaderCell(IXLCell cell, ImportExcelHeader header)
        {
            cell.Value = header.Header;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontColor = XLColor.Black;
            if (header.IsMandatory)
                cell.Style.Fill.BackgroundColor = XLColor.DarkGray;
            else
                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
        }
    }
}
