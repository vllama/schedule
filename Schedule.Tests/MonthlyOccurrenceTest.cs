using System;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VLL.Schedule.Tests {


    [TestClass]
    public class MonthlyOccurrenceTest : ScheduleTestBase {
        Month[] EVERY_MONTH_OF_THE_YEAR = new[]{
                Month.January
                , Month.February
                , Month.March
                , Month.April
                , Month.May
                , Month.June
                , Month.July
                , Month.August
                , Month.September
                , Month.October
                , Month.November
                , Month.December};

        DayOfWeek[] EVERY_DAY_OF_THE_WEEK = new[] {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday,
            DayOfWeek.Saturday,
            DayOfWeek.Sunday            
        };

        [TestInitialize]

        public override void setUp() {
            base.setUp();
            
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Argument exception thrown")]
        public void MonthlyOccurrenceInitParam0Test() {
            new MonthlyOccurrence(today, today, new Month[] { }, new[] { 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Argument exception thrown")]
        public void MonthlyOccurrenceInitParam1Test() {
            new MonthlyOccurrence(today, today, new Month[] { Month.January }, new int[] { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Argument exception thrown")]
        public void MonthlyOccurrenceInitParam2Test() {
            new MonthlyOccurrence(today, today, new Month[] { Month.January }, new WeekOfMonth[] { }, new DayOfWeek[] { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Argument exception thrown")]
        public void MonthlyOccurrenceInitParam3Test() {
            new MonthlyOccurrence(today, today, new Month[] { Month.January }, new WeekOfMonth[] { }, new DayOfWeek[] { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Argument exception thrown")]
        public void MonthlyOccurrenceInitParam4Test() {
            new MonthlyOccurrence(today, today, new[] { Month.January }, new WeekOfMonth[] { WeekOfMonth.First }, new DayOfWeek[] { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Argument exception thrown")]
        public void MonthlyOccurrenceInitParam5Test() {
            new MonthlyOccurrence(today, today, new Month[] { Month.January }, new WeekOfMonth[] { }, new DayOfWeek[] { DayOfWeek.Sunday });
        }

        [TestMethod]
        public void ValidConstructTest() {
            var byDaysOfMonth = new MonthlyOccurrence(today, today, new[] { Month.January, Month.February, Month.March }, new[] { 1 });
            var byLastWeekOfMonth = new MonthlyOccurrence(today, today, new[] { Month.January, Month.February, Month.March }, new[] { 32 });
            var byDaysAndWeeks = new MonthlyOccurrence(today, today, new[] { Month.January, Month.February, Month.March }, new[] { WeekOfMonth.First }, new[] { DayOfWeek.Sunday });
        }

        [TestMethod]
        public void test_TestFeb28DOM() {
            var start = new DateTime(2010, 1, 1);
            var end = new DateTime(2020, 1, 1);
            Month[] months = { Month.February, Month.May, Month.December };
            int[] daysOfMonth = { 31 };

            var mocF2 = new MonthlyOccurrence(start, end, months, daysOfMonth);
            mocF2.GetNext(today);
            visualiseOccurrencesInRange(mocF2);


            months = new[]{ Month.February, Month.June, Month.December};
            mocF2 = new MonthlyOccurrence(start, end, months, daysOfMonth);
            mocF2.GetNext(today);
        }


        [TestMethod]
        public void DatesOfMonthTest() {
            var firstDaysOfEveryMonth = new MonthlyOccurrence(today, today.AddDays(365), EVERY_MONTH_OF_THE_YEAR, new[] { 1 });
            visualiseOccurrencesInRange(firstDaysOfEveryMonth);
            var allDaysOfEveryMonth = new MonthlyOccurrence(today, today.AddDays(365), EVERY_MONTH_OF_THE_YEAR, Enumerable.Range(1,31).ToArray());
            visualiseOccurrencesInRange(allDaysOfEveryMonth);

            //var feb31test = new DateTime(2016, 2, 31);
            var everySingleDay = new MonthlyOccurrence(null, null, EVERY_MONTH_OF_THE_YEAR, Enumerable.Range(1, 31).ToArray());
            foreach (var year in Enumerable.Range(DateTime.MinValue.Year, DateTime.MaxValue.Year - DateTime.MinValue.Year +1)) {
                foreach (var month in Enumerable.Range(1, 12)) {
                    foreach (var day in Enumerable.Range(1, DateTime.DaysInMonth(year, month))) {
                        var theDate = new DateTime(year, month, day);
                        Assert.AreEqual(theDate, everySingleDay.GetNext(theDate));
                    }
                }
            }
            //Assert.IsNotNull
        }

        [TestMethod]
        public void WeekDaysofMonthTest() {


            var firstTest = new MonthlyOccurrence(DateTime.MinValue, DateTime.MaxValue
                    , new Month[] { Month.January,Month.March }
                    , new WeekOfMonth[] { WeekOfMonth.First }
                    , new DayOfWeek[] { DayOfWeek.Sunday,DayOfWeek.Tuesday });

            visualiseOccurrencesInRange(firstTest, today.AddDays(-today.DayOfYear), today.AddDays(365-today.DayOfYear));

            foreach (var dt in AOccurrence.DateRange(DateTime.MinValue, DateTime.MaxValue)) {
                firstTest.GetNext(dt);
            }
          
        }

        [TestMethod]
        public void EverySecondTuesdayOfTheMonth() {
            //Run for the whole date range for every week of the month for all days of the week.

            var everySecondTuesday = new MonthlyOccurrence(DateTime.MinValue, DateTime.MaxValue
                    , EVERY_MONTH_OF_THE_YEAR
                    , new WeekOfMonth[] { WeekOfMonth.Second }
                    , new DayOfWeek[] { DayOfWeek.Tuesday});

            visualiseOccurrencesInRange(everySecondTuesday, today.AddDays(-today.DayOfYear), today.AddDays(365 - today.DayOfYear));

            foreach (var dt in AOccurrence.DateRange(DateTime.MinValue, DateTime.MaxValue)) {
                var result = everySecondTuesday.GetNext(dt);
                if (result.HasValue) {
                    Assert.AreEqual(DayOfWeek.Tuesday, result.Value.DayOfWeek, $"Got result of {result} which is a {result.Value.DayOfWeek}");
                } 
                //This test is inconclusive as it does not test the nulls
            }

        }


        [TestMethod]
        public void AllWeekDaysofMonthTest() {
            //Run for the whole date range for every week of the month for all days of the week.

            var firstTest = new MonthlyOccurrence(DateTime.MinValue, DateTime.MaxValue
                    , EVERY_MONTH_OF_THE_YEAR
                    , new WeekOfMonth[] { WeekOfMonth.First,WeekOfMonth.Second,WeekOfMonth.Third,WeekOfMonth.Fourth,WeekOfMonth.Last }
                    , EVERY_DAY_OF_THE_WEEK);

      //      visualiseOccurrencesInRange(firstTest, today.AddDays(-today.DayOfYear), today.AddDays(365 - today.DayOfYear));

            foreach (var dt in AOccurrence.DateRange(DateTime.MinValue, DateTime.MaxValue)) {
                Assert.IsNotNull(firstTest.GetNext(dt));
            }

        }


    }
}
