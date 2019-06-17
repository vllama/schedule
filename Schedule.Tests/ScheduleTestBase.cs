using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VLL.Schedule.Tests {

    public abstract class ScheduleTestBase {

        protected DateTime _date;
        protected DateTime _datein;
        protected DateTime _dateout;
        protected DateTime _dateeq;

        protected DateTime today = DateTime.MinValue;
        protected DateTime yesterday = DateTime.MinValue;
        protected DateTime thirtyDaysAgo = DateTime.MinValue;
        protected DateTime twentyDaysAgo = DateTime.MinValue;
        protected DateTime tenDaysAgo = DateTime.MinValue;

        [TestInitialize]
        public virtual void setUp() {
            _date = new DateTime(2010, 02, 01);
            _datein = new DateTime(2010, 01, 01);
            _dateout = new DateTime(2010, 05, 01);
            _dateeq = new DateTime(2010, 02, 01);

            today = DateTime.Now.Date;
            yesterday = today.AddDays(-1);
            thirtyDaysAgo = today.AddDays(-30);
            twentyDaysAgo = today.AddDays(-20);
            tenDaysAgo = today.AddDays(-10);
        }
        
        //range(start_date, end_date):
        //for n in range((end_date - start_date).days):
        //  yield start_date + datetime.timedelta(n)
        /// <summary>
        /// This method will draw the ocurrences on a timeline
        /// </summary>
        /// <param name="occ">An IOccurrence instance</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void visualiseOccurrencesInRange(IOccurrence occ, DateTime? start = null, DateTime? end = null) {
            if (!end.HasValue) {
                end = occ.End;
            }

            if (!start.HasValue) {
                start = occ.Start;
            }
            Trace.WriteLine(String.Format(" Range from {0} to {1} ", start, end));

            var now = DateTime.Now.Date;
            Trace.Write("|");

            foreach (var sd in AOccurrence.DateRange(start.Value, end.Value)) {
                Trace.Write(String.Format("     {0,4:d} |", (sd - now).Days + 1));
            }

            Trace.Write("\n|");
            foreach (var sd in AOccurrence.DateRange(start.Value, end.Value)) {
                Trace.Write(String.Format("{0:yyyy-MM-dd}|", sd));
            }

            Trace.Write("\n|");
            foreach (var sd in AOccurrence.DateRange(start.Value, end.Value)) {
                if (sd == occ.GetNext(sd, true)) {
                    Trace.Write(String.Format("{0:yyyy-MM-dd}|", sd));
                } else {
                    Trace.Write("          |");
                }
            }
        }
    }
    
}
