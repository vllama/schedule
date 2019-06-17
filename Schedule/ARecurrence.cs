using System;

namespace VLL.Schedule {
    
    public abstract class ARecurrence : AOccurrence {

        protected int Interval { get; private set; }
        protected int Span { get; private set; }

        protected abstract string SpanUnit { get; }

        public static string[] OccurrenceTypes = { "Once", "Daily", "Weekly", "Monthly" };

        protected ARecurrence(DateTime? start, DateTime? end, int interval = 1, int span = 1) : base(start, end) {
            Interval = interval;
            Span = span;
        }
        
        //todo: add cultureinfo
        public override string ToString() {
            return base.ToString() + $"Every : {Interval} over a Span of: {Span} {SpanUnit}";
        }

        /// <summary>
        /// Factory method for creating any supported occurrence type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="interval"></param>
        /// <param name="dayOfWeek"></param>
        /// <param name="months"></param>
        /// <param name="daysOfMonth"></param>
        /// <param name="dateOfMonth"></param>
        /// <returns></returns>
        public static IOccurrence NewOccurrence(string type, DateTime? start, DateTime? end = null,
                                                int interval = 1,
                                                DayOfWeek[] dayOfWeek = null,
                                                Month[] months = null ,
                                                WeekOfMonth[] daysOfMonth = null,
                                                int[] dateOfMonth = null ) {
            switch (type) {
                case "Once":
                    return new OnceOccurrence(start);
                case "Daily":
                    return new DailyOccurrence(start, end, interval);
                case "Weekly":
                    return new WeeklyOccurrence(start, end, dayOfWeek, interval);
                case "Monthly":
                    if (dateOfMonth != null) {
                        return new MonthlyOccurrence(start, end, months, dateOfMonth);
                    }
                    if (daysOfMonth != null && dayOfWeek != null) {
                        return new MonthlyOccurrence(start, end, months, daysOfMonth, dayOfWeek);
                    }
                    break;
                default:
                    return null;
            }
            return null;
        }
    }
}
