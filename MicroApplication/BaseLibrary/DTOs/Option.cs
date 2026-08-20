using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.DTOs
{
    public class NumberOption
    {
        public string Text { get; set; }
        public int Value { get; set; }
        protected NumberOption(string text, int value)
        {
            Text = text;
            Value = value;
        }
    }
    public class NumberItemModel
    {
        public string Text { get; set; } = "";
        public int Id { get; set; }
        public NumberItemModel() { }
        public NumberItemModel(string text, int id)
        {
            Text = text;
            Id = id;
        }
    }
    public class GuidItemModel
    {
        public string Text { get; set; } = "";
        public Guid Id { get; set; }
        public GuidItemModel() { }
        protected GuidItemModel(string text, Guid id)
        {
            Text = text;
            Id = id;
        }
    }
    public class TextItemModel
    {
        public string Text { get; set; } = "";
        public string Value { get; set; } = "";
        public TextItemModel() { }
        public TextItemModel(string text, string value)
        {
            Text = text;
            Value = value;
        }
    }
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
        public int GetIntValue()
        {
            if(int.TryParse(Value, out int result))
                return result;
            throw new ValidationException("Value can not be convert to integer.");
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
