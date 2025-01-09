using ClosedXML.Excel;
using System.Globalization;

namespace BaseLibrary.Utilities.Excels
{
    public class ExcelImportData 
    {
        XLWorkbook Workbook { get; }
        List<ImportExcelHeader> ImportExcelHeaders { get; }
        public ExcelImportData(byte[] importFile, List<ImportExcelHeader> headers)
        {
            Workbook = GetXLWorkbook(importFile);
            ImportExcelHeaders = headers;
        }
        private static XLWorkbook GetXLWorkbook(byte[] file)
        {
            using (var stream = new MemoryStream(file))
            {
                var ws = new XLWorkbook(stream);
                return ws;
            }
        }

        public List<Dictionary<string, string>> GetExcelData(bool addId)
        {
            var ws = Workbook.Worksheets.FirstOrDefault();
            if (ws == null)
                throw new ValidationException($"No worksheet found in given excel.");

            if (addId)
                ImportExcelHeaders.Insert(0, new ImportExcelHeader("Id", false));

            int rowIndex = 1;
            var excelHeaders = new List<string>();
            for (int i = 1; i <= ImportExcelHeaders.Count; i++)
            {
                var cell = ws.Cell(rowIndex, i);
                var value = GetCellValue(cell);
                excelHeaders.Add(value);
                if (ImportExcelHeaders.Any(h => h.Header == value))
                    continue;
                throw new ValidationException($"Attribute {value} is not a valid attribute.");
            }

            var data = new List<Dictionary<string, string>>();
            rowIndex += 1;
            var rowHasValue = true;
            var emptyRowsFound = 0;
            while (emptyRowsFound < 3)
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                rowHasValue = false;
                for (int i = 1; i <= excelHeaders.Count; i++)
                {
                    var cell = ws.Cell(rowIndex, i);
                    var value = GetCellValue(cell);
                    var key = excelHeaders[i - 1];
                    keyValues[key] = value;
                    if (rowHasValue == false && key.ToLower() != "id")
                        rowHasValue = !string.IsNullOrWhiteSpace(value);
                }
                if (rowHasValue)
                {
                    emptyRowsFound = 0;
                    data.Add(keyValues);
                }
                else
                    emptyRowsFound += 1;

                rowIndex += 1;
            }
            return data;
        }

        private static string GetCellValue(IXLCell cell)
        {
            var val = cell.Value;
            if (val.IsBlank)
                return "";
            return val.ToString(CultureInfo.InvariantCulture);
        }

    }
}
