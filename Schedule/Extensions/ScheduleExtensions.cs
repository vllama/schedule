using System;
using System.Linq;
using VLL.Schedule;

namespace VLL.Schedule.Extensions {
    public static class ScheduleExtensions {

        /// <summary>
        /// Return which Day of the month the date represents. For example :
        ///         July 17'th 2016 is the 3'rd Sunday of July 2016, therefore we would return WeekOfMonth.Third (0 based index)
        /// </summary>
        /// <param name="date">The day of the month</param>
        /// <returns>Returns 0 first, 1, 2,3,4 or 5 (last) occurrence of the day of the week in the month</returns>
        public static WeekOfMonth GetOrdinalInMonth(this DateTime date) {

            // create a range iterator of all the days in the month of the date
            var range = AOccurrence.DateRange(new DateTime(date.Year, date.Month, 1),
                                              new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month))
                                              );
            // get oonly the days in the range where the days of the week of the date are the same...
            //TODO: one can also do weird +7,-7 math until the bounds of the month are reached.. would that be that much faster???
            var nextRange = range.Where(x => x.DayOfWeek == date.DayOfWeek);

            // get the index of the day in the month where the date is the passed date
            int ordinal = nextRange.Select((time, index) => new { time, index }).First(x => date == x.time).index;
            if (ordinal < 0 || ordinal > 5)
                throw new ApplicationException("Something went wrong with determining the Ordinal in the Month");
            return (WeekOfMonth)ordinal;
        }
    }
}
