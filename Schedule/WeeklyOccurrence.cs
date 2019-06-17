using System;
using System.Collections.Generic;
using System.Linq;

namespace VLL.Schedule {
    /// <summary>
    /// Weekly Ocurrences 
    /// </summary>
    public class WeeklyOccurrence : ARecurrence {

        // It is just a list of maximum 7 Ioccurences        
        private readonly IList<IOccurrence> _occurrences = new List<IOccurrence>(7);
        private readonly DayOfWeek[] _daysOfWeek;

        protected override string SpanUnit => "Week";

        public WeeklyOccurrence(DateTime? start, DateTime? end, DayOfWeek[] weekDays, int interval = 1)
            : base(start, end, interval, 7)
            
            {
            if (weekDays == null) {
                throw new ArgumentException("Week Days must be specified", nameof(weekDays));
            }
            if(weekDays.Length == 0) {
                throw new ArgumentException("At least one Week Day must be specified", nameof(weekDays));
            }

            this._daysOfWeek = weekDays.OrderBy(x => x).ToArray();

            foreach (var dw in _daysOfWeek) {
                var dayOffset = GetDayOffset(this.Start, dw);
                _occurrences.Add(new DailyOccurrence(this.Start.AddDays(dayOffset), this.End, this.Span * this.Interval));
            }
        }

        
        public override DateTime? GetNext(DateTime from, bool asDate = false) {
            from = from.Date;
            if( from < this.Start || from > this.End ) {
                return null;
            }

            var minDt = DateTime.MaxValue;
            // look through all the occurrences ( maximum 7 )
            foreach(var occ in this._occurrences) {
                // dont' look before the start of the valid range
                var next = occ.GetNext(from < occ.Start ? occ.Start : from);
                if (next != null && next < minDt) {
                    minDt = next.Value;
                }
            }
            
            //todo: is there a test past the end?
            //if (minDt < DateTime.MaxValue)
            if(minDt < this.End)
                return minDt;
            return null;
        }


        public override string ToString() {
            return String.Format("Every {0:d} week{1:s} {2:s} on {3:s} ",
                this.Interval,
                this.Interval > 1 ? "s" : String.Empty,
                base.ToString(),
                DaysOfWeekToString()
                );
        }

        /// <summary>
        /// TODO: i1N
        /// </summary>
        /// <returns></returns>
        private string DaysOfWeekToString() {
            var culture = new System.Globalization.CultureInfo("en-CA");
            return _daysOfWeek.Aggregate("", (current, t) => current + culture.DateTimeFormat.GetDayName(t) + " ");
        }


        /// <summary>
        /// get the number of days from day to the day of the week dow
        /// </summary>
        /// <param name="day"></param>
        /// <param name="dow"></param>
        /// <returns></returns>
        public static int GetDayOffset(DateTime day, DayOfWeek dow) {
            if (dow >= day.DayOfWeek) {
                return dow - day.DayOfWeek;
            } else {
                return 6 - (int)day.DayOfWeek + (int)dow + 1;
            }
        }
    }

}
