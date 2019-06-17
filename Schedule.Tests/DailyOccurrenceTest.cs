using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace VLL.Schedule.Tests.Extensions {
    [TestClass]
    public class DailyOccurrenceTest : ScheduleTestBase {

     

        
        [TestMethod]
        public void DailyOccurrenceDaysSinceStart() {
            // test 0 interval 1
            {
                var dc0 = new DailyOccurrence(today, DateTime.MaxValue, 1);
                Assert.AreEqual<int>(0, dc0.GetDaysLeft(today));
            }
            //# test 1 interval 1
            {
                var dc1 = new DailyOccurrence(today, DateTime.MaxValue, 1);
                Assert.AreEqual<int>(0, dc1.GetDaysLeft(today.AddDays(1)));
            }
            { 
                //# test 0 interval 2
                var dc2 = new DailyOccurrence(today, DateTime.MaxValue, 2);
                Assert.AreEqual<int>(0, dc2.GetDaysLeft(today));
            }
            { 
                // # test 1 interval 2
                var dc3 = DailyOccurrence.From(today, 2);
                visualiseOccurrencesInRange(dc3, today.AddDays(-5), today.AddDays(5));
                Assert.AreEqual<int>(1, ((DailyOccurrence)dc3).GetDaysLeft(today.AddDays(1)));
            }

            {
                var dc4 = DailyOccurrence.From(today.AddDays(-3), 2);
                visualiseOccurrencesInRange(dc4, today.AddDays(-10), today.AddDays(10));
                Assert.AreEqual<int>(0, ((DailyOccurrence)dc4).GetDaysLeft(today.AddDays(1)));
            }
        }

        [TestMethod]
        public void DailyOccurrenceOutOfRangeTest() {
            var occ = new DailyOccurrence(twentyDaysAgo, tenDaysAgo, 1);
            Assert.IsNull(occ.GetNext(today));
            Assert.IsNull(occ.GetNext(today, true));


            var futureOcc = new DailyOccurrence(today.AddDays(20), today.AddDays(30), 1);
            Assert.IsNull(futureOcc.GetNext(today));

            var nextOccPastEnd = new DailyOccurrence(today.AddDays(20), today.AddDays(31), 4);
            visualiseOccurrencesInRange(nextOccPastEnd, today.AddDays(15), today.AddDays(38));
            Assert.IsNull(nextOccPastEnd.GetNext(today.AddDays(30)));
            Assert.IsNotNull(nextOccPastEnd.GetNext(today.AddDays(26)).Value);

        }
        [TestMethod]
        public void TodayOpenEndedTest() {
            { 
                var occ = DailyOccurrence.From(today);
                Assert.AreEqual(today, occ.GetNext(today));
            }
            {
                var occ = DailyOccurrence.From(yesterday);
                Assert.AreEqual(today, occ.GetNext(today));
                Assert.IsNull(occ.GetNext(today.AddDays(-2)));
                Assert.IsNull(occ.GetNext(today.AddDays(-3)));
            }

            {
                foreach (var i in Enumerable.Range(2, 30)) {
                    var startDTt = today.AddDays(-1000);
                    var occ = DailyOccurrence.From(startDTt, i);

                    var dt = new DateTime(startDTt.Year, startDTt.Month, startDTt.Day);


                    while (dt < today) {
                        dt = dt.AddDays(i);
                    }
                    Assert.AreEqual(dt, occ.GetNext(today), String.Format("For Start {0:s} interval {1:d}",today.ToString("YYYY-MM-dd"),i));
                }
            }

            {
                var occ = DailyOccurrence.From(today.AddDays(-20000));
                Assert.AreEqual(today, occ.GetNext(DateTime.Now.Date));
            }

            {
                var occ = DailyOccurrence.From(today.AddDays(-7), 3);
                Assert.AreEqual(today.AddDays(-7), occ.GetNext(today.AddDays(-7)));
                Assert.AreEqual(today.AddDays(-7).AddDays(3), today.AddDays(-4));
                Assert.AreEqual(today.AddDays(2), occ.GetNext(today));
            }

        }
    }
}
