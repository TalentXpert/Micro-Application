namespace BaseLibrary.Utilities
{
    public class DateTimeHelper
    {
        public static string GetFormattedDate(DateTime? date)
        {
            string result = string.Empty;
            if (date.HasValue)
            {
                result = date.Value.ToString("dd-MMM-yyyy");
            }
            return result;
        }

        public static string GetFormattedDateTime(DateTime? date)
        {
            string result = string.Empty;
            if (date.HasValue)
            {
                result = date.Value.ToString("dd-MMM-yyyy HH:mm:ss");
            }
            return result;
        }

        public static string GetDateFormattedString(DateTime date, string format, bool includeTime)
        {
            if (includeTime)
                return date.ToString($"{format} HH:mm:ss");
            return date.ToString(format);
        }

        public static DateTime GetDateBasedOnFormat(string date, string format)
        {
            date = date.Trim();
            format = format.Trim();
            return new DateFormatHanlder(format, date).GetDate();
        }

        public static string GetMonthName(int month, bool isShortName = true)
        {
            DateTime date = new DateTime(2002, month, 01);
            string month_name;
            if (isShortName)
                month_name = date.ToString("MMM");
            else
                month_name = date.ToString("MMMM");
            return month_name;
        }

        public static int GetMonth(int months)
        {
            DateTime datetime = DateTime.Now.AddMonths(months);
            return datetime.Month;
        }
        public static DateTime GetMonthStartDate(int months)
        {
            DateTime datetime = DateTime.Now.AddMonths(months);
            var monthStart = new DateTime(datetime.Year, datetime.Month, 1);
            return monthStart;
        }
        public static DateTime GetMonthEndDate(int months)
        {
            DateTime datetime = DateTime.Now.AddMonths(months);
            var monthStart = new DateTime(datetime.Year, datetime.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);
            return monthEnd;
        }
        public static string DateInSqlQueryFormat(DateTime d, bool isStart)
        {
            var result = $"{d.Year}-{d.Month}-{d.Day}";
            if (isStart)
                return $"{result} 00:00:00.000";
            return $"{result} 23:59:59.999";
        }
        public static string DateInSqlQueryFormat(DateTime d)
        {
            var result = $"{d.Year}-{d.Month}-{d.Day} {d.Hour}:{d.Minute}:{d.Second}.{d.Millisecond}";
            return result;
        }

        //public static DateTime? GetDate(string inputDate, string format = null)
        //{
        //    if (string.IsNullOrWhiteSpace(inputDate))
        //        return null;
        //    if (format == null)
        //        format = GetDateTimeFormat(inputDate);
        //    if (DateTime.TryParseExact(inputDate, format, null, System.Globalization.DateTimeStyles.None, out DateTime outDate))
        //        return outDate;
        //    return null;
        //}

        //public static string GetDateTimeFormat(string inputDate)
        //{
        //    if (string.IsNullOrWhiteSpace(inputDate)) return null;
        //    string[] datePatterns = { "dd.MM.yyyy", "d.MM.yyyy", "dd.M.yyyy", "d.MM.yyyy", "dd-MM-yyyy", "dd-MM-yyyy", "dd/MM/yyyy", "yyyy-MM-dd" };
        //    foreach (string datePattern in datePatterns)
        //    {
        //        if (DateTime.TryParseExact(inputDate, datePattern, null, System.Globalization.DateTimeStyles.None, out DateTime outDate))
        //            return datePattern;
        //    }
        //    return null;
        //}

        //public static DateTime? GetDateTime(string inputDate)
        //{
        //    if (string.IsNullOrWhiteSpace(inputDate)) return null;


        //    string[] datePatterns = { "dd.MM.yyyy", "d.MM.yyyy", "dd.M.yyyy", "d.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy", "yyyy-MM-dd" };
        //    foreach (string datePattern in datePatterns)
        //    {
        //        if (DateTime.TryParseExact(inputDate, datePattern, null, System.Globalization.DateTimeStyles.None, out DateTime outDate))
        //            return outDate;
        //    }
        //    return null;
        //}

        public static DateTime GetDayStartDateTime(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }
        public static DateTime GetDayEndDateTime(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        public static bool IsFirstDateSmallerThanSecond(DateTime scheduleAt, DateTime startDatum)
        {
            return GetDayStartDateTime(scheduleAt) < GetDayStartDateTime(startDatum);
        }

    }
}
