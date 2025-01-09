using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BaseLibrary.Utilities
{
    public class Extension
    {
        DataTableExtension _dataTable;
        public DataTableExtension DataTable { get { return _dataTable ?? (_dataTable = new DataTableExtension()); } }

        StringExtension _string;
        public StringExtension String { get { return _string ?? (_string = new StringExtension()); } }

        DecimalExtension _decimal;
        public DecimalExtension Decimal { get { return _decimal ?? (_decimal = new DecimalExtension()); } }


        ContactNumberExtension _contactNumber;
        public ContactNumberExtension ContactNumber { get { return _contactNumber ?? (_contactNumber = new ContactNumberExtension()); } }


       
    }

   

    public class DataTableExtension
    {
        public List<string> GetColumns(DataTable datatable)
        {
            var columnNames = new List<string>();
            foreach (DataColumn column in datatable.Columns)
            {
                columnNames.Add(column.ColumnName);
            }
            return columnNames;
        }

        public bool IsEmptyOrNullDataRow(DataRow tempRow, DataTable dataTable)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                if (!tempRow.IsNull(column.ColumnName)) return false;
            }
            return true;
        }

        public bool IsColumnNotNull(DataRow row, string columnName, List<string> columns)
        {
            if (columns.Any(c => c.Equals(columnName, StringComparison.OrdinalIgnoreCase)))
            {
                if (row.IsNull(columnName)) return false;
                return true;
            }
            return false;
        }

        public bool HasColumn(string columnName, List<string> columns)
        {
            return columns.Any(c => c.Equals(columnName, StringComparison.OrdinalIgnoreCase));
        }

        public int? GetIntValue(DataRow row, string columnName, List<string> columns)
        {
            if (IsColumnNotNull(row, columnName, columns))
                return (int)row[columnName];
            return null;
        }

        public Guid? GetGuidValue(DataRow row, string columnName)
        {
            if (!row.IsNull(columnName))
                return (Guid)row[columnName];
            return null;
        }

        public List<string> GetColumnValues(DataTable dt, string column)
        {
            var result = new List<string>();
            foreach(DataRow dr in dt.Rows)
            {
                result.Add(dr[column].ToString());
            }
            return result;
        }
    }

    public class DecimalExtension
    {
        public decimal? GetDecimal(string input)
        {
            if (decimal.TryParse(input, out decimal result))
                return result;
            return null;
        }

        public decimal? ExtractDecimal(string input)
        {
            string experience = string.Empty;
            foreach (var c in input)
            {
                if (char.IsDigit(c) || c == '.')
                {
                    experience += c.ToString();
                }
            }
            return GetDecimal(experience);
        }

        public decimal? ConvertToDecimal(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;
            string experience = string.Empty;
            foreach (var c in input)
            {
                if (char.IsDigit(c) || c == '.')
                {
                    experience += c.ToString();
                }
            }

            if (decimal.TryParse(experience, out decimal num))
                return num;
            return null;
        }
    }

    public class ContactNumberExtension
    {
        public string RemoveUnwantedNumber(string contactNumber, string country)
        {
            if (country.ToUpper() == "INDIA")
            {
                if (contactNumber.Length > 10 && contactNumber.StartsWith("91"))
                {
                    var temp = contactNumber.Substring(2);
                    if (temp.Length >= 10) return temp;
                    return contactNumber;
                }
                return contactNumber;
            }
            return contactNumber;
        }
    }
}
