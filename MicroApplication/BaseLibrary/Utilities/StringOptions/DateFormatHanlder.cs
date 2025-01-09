namespace BaseLibrary.Utilities
{
    public class DateFormatHanlder
    {
        public DateFormatHanlder(string format, string date)
        {
            Format = format;
            Date = date;
            Separator = GetSeparator(Format);
        }

        public static char GetSeparator(string format)
        {
            foreach (var c in format)
            {
                if (char.IsLetter(c)) continue;
                return c;
            }
            return '.';
        }

        public char Separator { get; }
        public string Format { get; }
        public string Date { get; }

        public DateTime GetDate()
        {
            var dateParts = Date.Split(Separator);
            var dateFormatParts = Format.Split(Separator);
            int day = DateTime.Now.Day;
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month; ;
            for (int i = 0; i < dateParts.Length; i++)
            {
                if (dateFormatParts[i].Contains("m", StringComparison.CurrentCultureIgnoreCase))
                    month = Convert.ToInt32(dateParts[i]);
                if (dateFormatParts[i].Contains("d", StringComparison.CurrentCultureIgnoreCase))
                    day = Convert.ToInt32(dateParts[i]);
                if (dateFormatParts[i].Contains("y", StringComparison.CurrentCultureIgnoreCase))
                    year = Convert.ToInt32(dateParts[i]);
            }
            return new DateTime(year, month, day);
        }

        public bool CanConvert()
        {
            if (string.IsNullOrWhiteSpace(Format)) return false;
            if (string.IsNullOrWhiteSpace(Date)) return false;
            var sep = GetSeparator(Format);
            var dSep = GetSeparator(Date);
            if (sep != dSep)
                return false;
            var dateParts = Date.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            if (dateParts.Length != 3)
                return false;
            var dateFormatParts = Format.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            if (dateFormatParts.Length != 3)
                return false;
            return true;
        }
    }
}
