using System;
using System.Collections.Generic;
using System.Globalization;

namespace VLL.Schedule.Extensions {


    /// <summary>
    /// Returns the Week of the Month starting on a Monday ( 0,1,2,3,4 or last) 
    /// ( 0 is any week not containing the Thursday of the week iso8601, or 1'st week is the week that contains the 4th)
    /// For example, 2004 begins on a Thursday, so the first week of ISO year 2004 begins on Monday, 29 Dec 2003 and ends on Sunday, 4 Jan 2004
    /// https://en.wikipedia.org/wiki/ISO_8601
    /// 
    /// </summary>
    public static class DateTimeExtensions {
        static Calendar _gc = CultureInfo.InvariantCulture.Calendar;

        static DateTimeExtensions() {

        }

        /// <summary>
        /// Gets the Week of the Month based on the date ( 0,1,2,3,4 or last) 
        /// See iso8601
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int WeekOfMonth(this DateTime date) {
            var weekOfDate = date.Iso8601WeekOfYear();
            if (weekOfDate <= 1)
                return weekOfDate;

            var newDelta = (date.AddDays(1 - date.Day)).Iso8601WeekOfYear();
            return weekOfDate - newDelta;

          
        }

        /// <summary>
        /// Gets the week of Year based on the gregorian calendar
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [Obsolete("Do not use this as it is not ISO8601 compatible")]
        private static int WeekOfYear(this DateTime date) {
            /// https://msdn.microsoft.com/en-us/library/system.globalization.calendar.getweekofyear(v=vs.110).aspx
            return _gc.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        /// <summary>
        /// https://blogs.msdn.microsoft.com/shawnste/2006/01/24/iso-8601-week-of-year-format-in-microsoft-net/
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int Iso8601WeekOfYear(this DateTime time) {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it’ll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = _gc.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day ( subtract 1 to match Python)
            return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// Date Range 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> DateRange(this DateTime startDate, DateTime endDate) {
            if (endDate < startDate) {
                throw new ArgumentException("Start must be Before End");
            }

            while (startDate <= endDate) {
                yield return startDate;
                if (startDate.Date == DateTime.MaxValue.Date)
                    break;
                startDate = startDate.AddDays(1);
            }
        }
        //def date
    }
}
