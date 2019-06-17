using System;
using System.Collections.Generic;

namespace VLL.Schedule
{
    public abstract class AOccurrence : IOccurrence {

        public DateTime Start { get; private set; }

        public DateTime End { get; private set; }

        public abstract DateTime? GetNext(DateTime from, bool asDate = false);

        public override string ToString() {
            return $"From: {Start} to: {End}";
        }

        protected AOccurrence(DateTime? start, DateTime? end) {
            if (end < start) {
                throw new ArgumentException("End must be > than Start", nameof(end));
            }

            this.Start = start?.Date ?? DateTime.MinValue;
            this.End = end?.Date ?? DateTime.MaxValue;
        }

        public static IEnumerable<DateTime> DateRange(DateTime startDate, DateTime endDate) {
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
    }
}
