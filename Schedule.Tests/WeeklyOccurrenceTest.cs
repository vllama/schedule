using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VLL.Schedule.Tests {

    [TestClass]
    public class WeeklyOccurrenceTest : ScheduleTestBase {

        [TestMethod]
        public void WeeklyOccurrenceOutOfRangeTest() {

            var dow = today.DayOfWeek;
            var occ = new WeeklyOccurrence(today, null, new[]{dow},1);
            Trace.WriteLine(occ.ToString());
            visualiseOccurrencesInRange(occ,today, today.AddDays(50));

            Assert.AreEqual(today, occ.GetNext(today));
            Assert.IsNull(occ.GetNext(this.thirtyDaysAgo));

            var futureocc = new WeeklyOccurrence(today.AddDays(20), today.AddDays(40), new[] { DayOfWeek.Wednesday, DayOfWeek.Friday });
            visualiseOccurrencesInRange(futureocc);

            Assert.IsNull(futureocc.GetNext(today));
        }

        [TestMethod]
        public void WeeklyOccurrenceTodayOpenEndedTest() {
            {
                var occ = new WeeklyOccurrence(today, null, new[] { today.DayOfWeek });
                Assert.AreEqual(today, occ.GetNext(today));
            }
            {
                var occ = new WeeklyOccurrence(yesterday, null, new[] { today.DayOfWeek }, 2);
                Assert.AreEqual(today, occ.GetNext(today));

            }
            {
                var occ = new WeeklyOccurrence(yesterday, yesterday.AddDays(365), new[] { today.DayOfWeek }, 2);
                Assert.AreEqual(today, occ.GetNext(today));
                Assert.AreEqual(today.AddDays(14), occ.GetNext(today.AddDays(1)));
            }
        }

        [TestMethod]
        public void WeeklyOccurrenceGetDayOffsetTest() {
            var monday = new DateTime(2010, 05, 17);
            var tuesday = new DateTime(2010, 05, 18);
            var wednesday = new DateTime(2010, 05, 19);
            var thursday = new DateTime(2010, 05, 20);
            var friday = new DateTime(2010, 05, 21);
            var saturday = new DateTime(2010, 05, 22);
            var sunday = new DateTime(2010, 05, 23);

            Assert.AreEqual(1, WeeklyOccurrence.GetDayOffset(monday, DayOfWeek.Tuesday));
            Assert.AreEqual(2, WeeklyOccurrence.GetDayOffset(monday, DayOfWeek.Wednesday));
            Assert.AreEqual(3, WeeklyOccurrence.GetDayOffset(monday, DayOfWeek.Thursday));
            Assert.AreEqual(4, WeeklyOccurrence.GetDayOffset(monday, DayOfWeek.Friday));
            Assert.AreEqual(5, WeeklyOccurrence.GetDayOffset(monday, DayOfWeek.Saturday));
            Assert.AreEqual(6, WeeklyOccurrence.GetDayOffset(monday, DayOfWeek.Sunday));
            Assert.AreEqual(6, WeeklyOccurrence.GetDayOffset(tuesday, DayOfWeek.Monday));
            Assert.AreEqual(6, WeeklyOccurrence.GetDayOffset(wednesday, DayOfWeek.Tuesday));
            Assert.AreEqual(6, WeeklyOccurrence.GetDayOffset(wednesday, DayOfWeek.Tuesday));
            Assert.AreEqual(6, WeeklyOccurrence.GetDayOffset(thursday, DayOfWeek.Wednesday));
            Assert.AreEqual(6, WeeklyOccurrence.GetDayOffset(friday, DayOfWeek.Thursday));
            Assert.AreEqual(6, WeeklyOccurrence.GetDayOffset(saturday, DayOfWeek.Friday));
            Assert.AreEqual(6, WeeklyOccurrence.GetDayOffset(sunday, DayOfWeek.Saturday));
            Assert.AreEqual(0, WeeklyOccurrence.GetDayOffset(sunday, DayOfWeek.Sunday));
            Assert.AreEqual(6, WeeklyOccurrence.GetDayOffset(saturday, DayOfWeek.Friday));
            Assert.AreEqual(3, WeeklyOccurrence.GetDayOffset(thursday, DayOfWeek.Sunday));
        }

        [TestMethod]
        public void WeeklyOccurrenceMultipleDaysTest() {
            var start = new DateTime(2010, 6, 3);
            var end = new DateTime(2010, 8, 13);

            var days = new[] { DayOfWeek.Sunday, DayOfWeek.Saturday };

            var occ = new WeeklyOccurrence(start, end, days, 2);

            Assert.AreEqual(new DateTime(2010, 6, 5), occ.GetNext(start));
            Assert.AreEqual(new DateTime(2010, 6, 5), occ.GetNext(start));
            Assert.AreEqual(new DateTime(2010, 6, 5), occ.GetNext(start.AddDays(1)));
            Assert.AreEqual(new DateTime(2010, 6, 5), occ.GetNext(new DateTime(2010, 6, 5)));
            Assert.AreEqual(new DateTime(2010, 6, 6), occ.GetNext(new DateTime(2010, 6, 6)));
            Assert.AreEqual(new DateTime(2010, 6, 19), occ.GetNext(new DateTime(2010, 6, 7)));
            Assert.AreEqual(new DateTime(2010, 6, 19), occ.GetNext(new DateTime(2010, 6, 8)));
            Assert.AreEqual(new DateTime(2010, 6, 19), occ.GetNext(new DateTime(2010, 6, 9)));
            Assert.AreEqual(new DateTime(2010, 6, 19), occ.GetNext(new DateTime(2010, 6, 10)));
            Assert.AreEqual(new DateTime(2010, 6, 19), occ.GetNext(new DateTime(2010, 6, 11)));
            Assert.AreEqual(new DateTime(2010, 6, 19), occ.GetNext(new DateTime(2010, 6, 12)));
            Assert.AreEqual(new DateTime(2010, 6, 19), occ.GetNext(new DateTime(2010, 6, 13)));
            Assert.AreEqual(new DateTime(2010, 6, 19), occ.GetNext(new DateTime(2010, 6, 14)));
            Assert.AreEqual(new DateTime(2010, 6, 19), occ.GetNext(new DateTime(2010, 6, 15)));
            Assert.AreEqual(new DateTime(2010, 6, 19), occ.GetNext(new DateTime(2010, 6, 16)));
            Assert.AreEqual(new DateTime(2010, 6, 19), occ.GetNext(new DateTime(2010, 6, 17)));
            Assert.AreEqual(new DateTime(2010, 6, 19), occ.GetNext(new DateTime(2010, 6, 18)));
            Assert.AreEqual(new DateTime(2010, 6, 19), occ.GetNext(new DateTime(2010, 6, 19)));
            Assert.AreEqual(new DateTime(2010, 6, 20), occ.GetNext(new DateTime(2010, 6, 20)));
            Assert.AreEqual(new DateTime(2010, 7, 3), occ.GetNext(new DateTime(2010, 6, 21)));

            visualiseOccurrencesInRange(occ);
            Assert.IsNull(occ.GetNext(today));
            var docc = new DailyOccurrence(new DateTime(2010, 6, 5), end, 14);
            Assert.AreEqual(new DateTime(2010, 6, 19), docc.GetNext(new DateTime(2010, 6, 19)));
        }
    }
}
