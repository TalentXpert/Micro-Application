using System.Web;

namespace BaseLibrary.Utilities.Formaters
{
    public class HtmlTagFormatter
    {
        public static string ConvertDataTableToHtml(DataTable dt)
        {
            if (dt == null) return string.Empty;

            StringBuilder sb = new StringBuilder();

            // 1. Start the table tag (add classes or inline styles if desired)
            sb.AppendLine("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse; font-family: Arial, sans-serif;'>");

            // 2. Build the Header Row (<thead> and <th>)
            sb.AppendLine("  <thead>");
            sb.AppendLine("    <tr>");
            foreach (DataColumn column in dt.Columns)
            {
                // HttpUtility.HtmlEncode prevents broken layouts and XSS if names contain special characters
                sb.AppendLine($"      <th>{HttpUtility.HtmlEncode(column.ColumnName)}</th>");
            }
            sb.AppendLine("    </tr>");
            sb.AppendLine("  </thead>");

            // 3. Build the Data Rows (<tbody> and <td>)
            sb.AppendLine("  <tbody>");
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendLine("    <tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    string? cellValue = string.Empty;
                    if (row.IsNull(column) is false)
                        cellValue = row[column].ToString();
                    sb.AppendLine($"      <td>{HttpUtility.HtmlEncode(cellValue)}</td>");
                }
                sb.AppendLine("    </tr>");
            }
            sb.AppendLine("  </tbody>");

            // 4. End the table tag
            sb.AppendLine("</table>");

            return sb.ToString();
        }

    }
}
