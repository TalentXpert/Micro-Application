namespace BaseLibrary.Utilities
{
    public class DateVM
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public bool IsValidDate()
        {
            if (Year > 0 && Month > 0 && Day > 0) return true;
            return false;
        }

        public DateTime GetDate()
        {
            try
            {
                DateTime dateTime = new DateTime();
                if (this.Year == 0 || this.Month == 0 || this.Day == 0) return dateTime;
                return new DateTime(this.Year, this.Month, this.Day);
            }
            catch
            {
                return new DateTime(this.Year, this.Month, this.Day);
            }
        }
        public DateTime GetDateTime()
        {
            try
            {
                DateTime dateTime = new DateTime();
                if (this.Year == 0 || this.Month == 0 || this.Day == 0) return dateTime;
                return new DateTime(this.Year, this.Month, this.Day, 0, 0, 0, DateTimeKind.Utc);
            }
            catch
            {
                return new DateTime(this.Year, this.Month, this.Day - 1, 0, 0, 0, DateTimeKind.Utc);
            }
        }
        public DateTime GetDayStartDateTime()
        {
            try
            {
                DateTime dateTime = new DateTime();
                if (this.Year == 0 || this.Month == 0 || this.Day == 0) return dateTime;
                return new DateTime(this.Year, this.Month, this.Day, 0, 0, 0, DateTimeKind.Utc);
            }
            catch
            {
                return new DateTime(this.Year, this.Month, this.Day - 1, 0, 0, 0, DateTimeKind.Utc);
            }
        }

        public DateTime GetSqlDayStartDate(string userTimeZone)
        {
            var userTime = new DateTime(this.Year, this.Month, this.Day, 0, 0, 0);
            return GetDateTimeUserToUtcTime(userTime, userTimeZone);
        }
        public DateTime GetSqlDayEndDate(string userTimeZone)
        {
            var userTime = new DateTime(this.Year, this.Month, this.Day, 23, 59, 59, 999);
            return GetDateTimeUserToUtcTime(userTime, userTimeZone);
        }
        public DateTime GetDateTimeUserToUtcTime(DateTime userTime, string userTimeZone)
        {
            try
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById(userTimeZone);
                var utcTime = TimeZoneInfo.ConvertTimeToUtc(userTime, timezone);
                return utcTime;
            }
            catch
            {
                throw new ValidationException("Couldn't convert to datetime.");
            }
        }
        public DateTime GetDateTimeUserToUtcTime(string userTimeZone)
        {
            try
            {
                DateTime dateTime = new DateTime();
                if (this.Year == 0 || this.Month == 0 || this.Day == 0) return dateTime;
                var timezone = TimeZoneInfo.FindSystemTimeZoneById(userTimeZone);
                var userTime = new DateTime(this.Year, this.Month, this.Day, 0, 0, 0);
                var utcTime = TimeZoneInfo.ConvertTimeToUtc(userTime, timezone);
                return utcTime;
            }
            catch
            {
                throw new ValidationException("Couldn't convert to datetime.");
            }
        }
        public DateTime GetDayEndDateTime()
        {
            try
            {
                DateTime dateTime = new DateTime();
                if (this.Year == 0 || this.Month == 0 || this.Day == 0) return dateTime;
                return new DateTime(this.Year, this.Month, this.Day, 23, 59, 59, DateTimeKind.Utc);
            }
            catch
            {
                return new DateTime(this.Year, this.Month, this.Day - 1, 0, 0, 0, DateTimeKind.Utc);
            }
        }

        public DateTime GetDateWithTime()
        {
            DateTime dateTime = new DateTime();
            if (this.Year == 0 || this.Month == 0 || this.Day == 0) return dateTime;
            var utcNowDate = DateTime.UtcNow;
            return new DateTime(this.Year, this.Month, this.Day, utcNowDate.Hour, utcNowDate.Minute, utcNowDate.Second, utcNowDate.Millisecond, DateTimeKind.Utc);
        }
        public DateVM() { }

        public DateVM(DateTime dateTime)
        {
            Year = dateTime.Year;
            Month = dateTime.Month;
            Day = dateTime.Day;
        }

        public static bool TryDateVM(string yyyymmdd, out DateVM dateVM)
        {
            dateVM = null;
            if (yyyymmdd == null) return false;
            try
            {
                var parts = yyyymmdd.Split("-");
                dateVM = new DateVM
                {
                    Year = Convert.ToInt32(parts[0]),
                    Month = Convert.ToInt32(parts[1]),
                    Day = Convert.ToInt32(parts[2])
                };
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DateVM Get(DateTime? dateTime)
        {
            if (dateTime.HasValue) return new DateVM(dateTime.Value);
            return null;
        }


        public DateVM(int year, int month, int day)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
        }

        /// <summary>
        /// Date string should be in proper format
        /// </summary>
        /// <param name="dateString">yyyy-mm-dd</param>
        public static bool TryCreate(string dateString, out DateTime dateTime)
        {
            dateTime = new DateTime();
            try
            {
                if (!string.IsNullOrWhiteSpace(dateString))
                {
                    var dateParts = dateString.Split("-");
                    dateTime = new DateTime(Convert.ToInt32(dateParts[0]), Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[2]));
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            return this.Year + "-" + this.Month + "-" + this.Day;
        }
    }
    public class DateTimeVM
    {
        public DateVM DateVM { get; set; }
        public TimeVM TimeVM { get; set; }

        public DateTime GetDateTime()
        {
            return new DateTime(DateVM.Year, DateVM.Month, DateVM.Day, TimeVM.Hour, TimeVM.Minute, 0, DateTimeKind.Local);
        }
    }

    public class TimeVM
    {
        public int Hour { get; set; }
        public int Minute { get; set; }

        public bool IsValidTime()
        {
            if (Hour > 0 && Minute > 0) return true;
            return false;
        }
    }
}
