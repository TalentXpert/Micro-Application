using System.Globalization;

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

        private static DateTime GetDateBasedOnFormatWithCustomLogic(string date, string format)
        {
            date = date.Trim();
            format = format.Trim();
            return new DateFormatHanlder(format, date).GetDate();
        }
        public static DateTime GetDateBasedOnFormat(string dateString, string format="yyyy-MM-dd")
        {
            //string dateString = "10/12/2025";
            //string format = "dd/MM/yyyy";
            DateTime dateTime;
            if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }
            else
            {
               return GetDateBasedOnFormatWithCustomLogic(dateString, format);
            }
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
        public static string GetFormattedDateTime(DateTime? date, string timeZone)
        {
            if (date.HasValue)
            {
                date = ConvertToUserTimeZone(date.Value, timeZone);
                return date.Value.ToString("dd-MMM-yyyy HH:mm");
            }
            return string.Empty;
        }

        public static DateTime ConvertToUserTimeZone(DateTime utcDate, string timeZone)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            var userTimeZoneDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDate, timezone);
            return userTimeZoneDateTime;
        }
    }
    public class ConvertDateTime 
    {
        public DateTime ConvertToUserTimeZone(DateTime utcDate, string timeZone)
        {
            try
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                var userTimeZoneDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDate, timezone);
                return userTimeZoneDateTime;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DateTime GetConvertedTimeToUserTimeZone(DateTime utcDate, string timeZone)
        {
            try
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                var userTimeZoneDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDate, timezone);
                return userTimeZoneDateTime;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DateTime ConvertTimeToUtc(DateTime localTime, string timeZone)
        {
            try
            {
                var temp = DateTime.SpecifyKind(localTime, DateTimeKind.Unspecified);
                var timezone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                var userTimeZoneDateTime = TimeZoneInfo.ConvertTimeToUtc(temp, timezone);
                return userTimeZoneDateTime;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string MinutesToHours(int minutes)
        {
            int hours = minutes / 60;
            int mins = minutes % 60;
            var minutesInHours = string.Format("{0:00}:{1:00}", hours, mins);
            return minutesInHours;
        }

        public DateTime StringToDate(string date)
        {
            // return in the format of year, month, day)           
            var parts = date.Split('-');

            if (parts.Length == 1)
                parts = date.Split('/');

            try
            {
                return new DateTime(int.Parse(parts[2]), int.Parse(parts[0]), int.Parse(parts[1]));//return when date format is mm/dd//yyyy
            }
            catch (Exception)
            {
                return new DateTime(int.Parse(parts[2]), int.Parse(parts[1]), int.Parse(parts[0]));//return when date format is dd/mm//yyyy
            }

        }

        public DateTime GetDateTimeFromString(string datetime)
        {
            var parts = datetime.Split('-');
            return new DateTime(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[5]));
        }
    }
}
