using System;


namespace VLL.Schedule
{
    public class DailyOccurrence : ARecurrence {

        protected override string SpanUnit => "Day";

        public DailyOccurrence(DateTime? start, DateTime? end, int interval = 1) : base(start, end, interval) {

        }

        /// <summary>
        /// Method returns the number of days left 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        public int GetDaysLeft(DateTime fromDate) {
            var daysSinceStart = (fromDate.Subtract(this.Start)).Days;
            if (daysSinceStart == 0) {
                return 0;
            } else if (daysSinceStart <= Interval) {
                return Interval - daysSinceStart;
            } else if (daysSinceStart % Interval == 0) {
                return 0;
            } else {
                return Interval - (daysSinceStart % Interval);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="asDate">Strip Time Components </param>
        /// <returns></returns>
        public override DateTime? GetNext(DateTime from, bool asDate = false) {

            from = from.Date;

            if (from < this.Start || from > this.End)
                return null;
            var daysLeft = this.GetDaysLeft(from);

            if (daysLeft == 0 ) {
                return from;
            }
            
            var toTest = from.AddDays(daysLeft);

            if( toTest > End) {
                return null;
            } else {
                return toTest;
            }
        }

        /// <summary>
        /// Returns a IOccurrence that starts from a given date, and runs until the end with a 1 interval
        /// </summary>
        /// <param name="from">Start Date</param>
        /// <returns>DailyOccurrence</returns>
        public static IOccurrence From(DateTime from) {
            return new DailyOccurrence(from, DateTime.MaxValue);
        }

        /// <summary>
        /// Returns a IOccurrence that starts from a given date, and runs until the end with the specified interval
        /// </summary>
        /// <param name="from">Start Date</param>
        /// <param name="interval">Occurrs every [interval] days;</param>
        /// <returns>DailyOccurrence</returns>
        public static IOccurrence From(DateTime from, int interval) {
            return new DailyOccurrence(from, DateTime.MaxValue, interval);
        }

        /// <summary>
        /// Returns a IOccurrence that starts at the beggining of time and runs until the given date with a 1 interval
        /// </summary>
        /// <param name="to">End Date</param>
        /// <returns></returns>
        public static IOccurrence To(DateTime to) {
            return new DailyOccurrence(DateTime.MinValue, to);
        }

        /// <summary>
        /// Returns a IOccurrence that starts at the beggining of time and runs until the given date with the specified interval
        /// </summary>
        /// <param name="to">End Date</param>
        /// <param name="interval">Occurrs every [interval] days;</param>
        /// <returns>DailyOccurrence</returns>
        public static IOccurrence To(DateTime to, int interval) {
            return new DailyOccurrence(DateTime.MinValue, to, interval);
        }
    }
}
