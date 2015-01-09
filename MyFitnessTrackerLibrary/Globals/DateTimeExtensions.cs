using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace MyFitnessTrackerLibrary.Globals
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek)
        {
            int diff = endOfWeek - dt.DayOfWeek;
            if (diff > 7)
            {
                diff -= 1;
            }

            return dt.AddDays(diff).Date;
        }

        public static DateTime StartOfMonth(this DateTime dt)
        {
            return(new DateTime(dt.Year, dt.Month, 1));
        }

        public static DateTime EndOfMonth(this DateTime dt)
        {
            return(new DateTime(dt.Year, dt.Month, 1).AddMonths(1).AddDays(-1));
        }

        public static DateTime StartOfMonth(this DateTime dt, int month)
        {
            return (new DateTime(dt.Year, month, 1));
        }

        public static DateTime EndOfMonth(this DateTime dt, int month)
        {
            return (new DateTime(dt.Year, month, 1).AddMonths(1).AddDays(-1));
        }

        // Iso8601
        public static int WeekOfDate(this DateTime dt)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(dt);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                dt = dt.AddDays(3);
            }
            return (CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday));
        }

        //Iso8601
        public static DateTime StartOfWeekNumber(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        public static DateTime EndOfWeekNumber(int year, int weekOfYear)
        {
            DateTime endOfWeek = new DateTime(year, 1, 1);
            return (DateTimeExtensions.StartOfWeekNumber(year, weekOfYear).AddDays(DayOfWeek.Sunday <= 0 ? 6 : DayOfWeek.Sunday - DayOfWeek.Monday));
        }
    }
}
