using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.DTOs
{
    public class Option
    {
        public string Text { get; set; }
        public string Value { get; set; }
        protected Option(string text, string value)
        {
            Text = text;
            Value = value;
        }
        protected Option(string value):this(value,value)
        {
        }
    }

    public class DatabaseOption : Option
    {
        protected DatabaseOption(string value) : base(value, value)
        {
        }
        public static DatabaseOption MicroApplicationDatabase = new DatabaseOption("MicroApplicationDatabase");
    }
}
