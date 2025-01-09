using System.Data;
using System.Text.RegularExpressions;

namespace BaseLibrary.Utilities.CSVLibrary
{

    public static class CSVHelper 
    {
        public static MemoryStream ToCSV(this DataTable dtDataTable)
        {
            var memoryStream = new MemoryStream();
            var memoryStream2 = new MemoryStream();

            StreamWriter sw = new StreamWriter(memoryStream);
            //headers    
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (C.IsNotDBNull(dr[i]))
                    {
                        string? value = dr[i].ToString();
                        if (C.IsNotNullOrEmpty(value) && value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }

            sw.Flush();
            memoryStream.Position = 0;
            memoryStream.CopyTo(memoryStream2);
            sw.Close();

            return memoryStream2;
        }

        public static CSVData GetCSVData(string csvString)
        {
            var data = new CSVData();
            string[] lines = csvString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            bool isHeaderRow = false;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (!isHeaderRow)
                {
                    string[] headers = line.Split(',');
                    PrepareHeader(headers, data);
                    isHeaderRow = true;
                }
                else
                {
                    List<string> rowdata = new List<string>();
                    Regex regx = new Regex("," + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");                    
                    string[] rows = regx.Split(line);
                    
                    foreach (var row in rows)
                        rowdata.Add(row.Replace("\"", string.Empty).Trim());
                                        
                    data.Rows.Add(rowdata.ToList());
                }
            }

            return data;
        }

        private static void PrepareHeader(string[] columns, CSVData data)
        {
            foreach (var column in columns)
            {
                if (C.IsNullOrEmpty(column)) continue;
                var columnWithOutSpace = X.Extension.String.RemoveEmptySpace(column);
                data.Headers.Add(columnWithOutSpace);
            }
        }
    }

    public class CSVData
    {
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        public CSVData()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }
    }
}



