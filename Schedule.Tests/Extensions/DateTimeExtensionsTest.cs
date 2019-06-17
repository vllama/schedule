using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using VLL.Schedule.Extensions;

namespace VLL.Schedule.Tests.Extensions {

    [TestClass]
    public class DateTimeExtensionsTest {

        CultureInfo myCI;
        Calendar myCal;

        [TestInitialize]
        public void SetUp() {
            myCI = new CultureInfo("en-US");
            myCal = myCI.Calendar;
        }

        [TestMethod]
        public void LastDayOfTheYearTest() {
            myCI.DateTimeFormat.CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek;
            myCI.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
            myCal = myCI.Calendar;
            foreach (var year in Enumerable.Range(2000, 20)) {
                DateTime firstDOY = new System.DateTime(year, 1, 1);
                Trace.WriteLine(String.Format("First week of year {0} is {1}.", firstDOY.Year, firstDOY.Iso8601WeekOfYear()));

                DateTime LastDay = new System.DateTime(year, 12, 31);
                Trace.WriteLine(String.Format("There are {0} weeks in the year ({1}).", LastDay.Iso8601WeekOfYear(), LastDay.Year));

                for (int month = 1; month <= 12; month++) {
                    for (int day = 1; day <= DateTime.DaysInMonth(year, month); day+=7) {
                        DateTime aWeekDay = new DateTime(year, month, day);
                        Trace.WriteLine(String.Format("Week of the month for date {0} is {1}, Week of year is {2}", aWeekDay, aWeekDay.Iso8601WeekOfYear(), aWeekDay.Iso8601WeekOfYear()));
                    }
                }
            }

            Calendar cal = CultureInfo.InvariantCulture.Calendar;
            // 1/1/1990 starts on a Monday
            DateTime dt = new DateTime(1990, 1, 1);
            Trace.WriteLine("Starting at " +dt + " day: " +cal.GetDayOfWeek(dt) + " Week: " + dt.Iso8601WeekOfYear());

            for (int i = 0; i < 100000; i++) {
                for (int days = 0; days < 7; dt = dt.AddDays(1), days++) {
                    if (dt.Iso8601WeekOfYear() != cal.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)) {
                        Trace.WriteLine("Iso Week " + dt.Iso8601WeekOfYear() +
                            " GetWeekOfYear: " +cal.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) +
                            " Date: " +dt + " Day: " +cal.GetDayOfWeek(dt));
                    }
                }
            }
        }

        [TestMethod]
        public void MonthWeekNumberTest() {
            // For example, 2004 begins on a Thursday, so the first week of 
            // ISO year 2004 begins on Monday, 29 Dec 2003 and ends on Sunday, 4 Jan 2004
            Assert.AreEqual(0, new DateTime(2010, 1, 1).WeekOfMonth(), "Friday - Jan 1 2010");
            Assert.AreEqual(1, new DateTime(2010, 1, 8).WeekOfMonth(), "Friday - Jan 8 2010");
            Assert.AreEqual(0, new DateTime(2009, 8, 1).WeekOfMonth(), "Saturday - Aug 1 2009");
            Assert.AreEqual(0, new DateTime(2009, 8, 2).WeekOfMonth(), "Sunday - Aug 2 2009");
            Assert.AreEqual(1, new DateTime(2009, 8, 3).WeekOfMonth(), "Monday - Aug 3 2009");
            Assert.AreEqual(0, new DateTime(2016, 5, 1).WeekOfMonth(), "Sunday - May 1 2016");
            Assert.AreEqual(1, new DateTime(2016, 5, 6).WeekOfMonth(), "Friday - May 6 2016");

            /*
            foreach (var year in Enumerable.Range(DateTime.MinValue.Year, DateTime.MaxValue.Year)) {
                foreach (var month in Enumerable.Range(1, 12)) {
                    // test the first 7 days
                    foreach ( var day in Enumerable.Range(1, 7)) {
                        var theDate = new DateTime(year, month, day);
                        DayOfWeek dow = theDate.DayOfWeek;
                        if (( dow == DayOfWeek.Friday|| dow == DayOfWeek.Saturday|| dow == DayOfWeek.Sunday) && theDate.Day <= 4) {
                            Assert.AreEqual(0, theDate.WeekOfMonth(), String.Format("{0:s} {1:yyyy-MM-dd}", dow.ToString(), theDate));
                        } else {
                            Assert.AreEqual(1, theDate.WeekOfMonth(), String.Format("{0:s} {1:yyyy-MM-dd}", dow.ToString(), theDate));
                        }
                    }
                }
            }*/
        }


        [TestMethod]
        public void WeekOfyearTest() {
            /// Jan 1 2004 = Thurs, Jan2= fri, jan3 = sat, jan4 = sun
            /// Jan 1 
            Assert.AreEqual(1, new DateTime(2004, 1, 1).Iso8601WeekOfYear(), "First week of 2004 is the week contianing jan 4'th");
            Assert.AreEqual(2, new DateTime(2004, 1, 5).Iso8601WeekOfYear(), "First week of 2004 is the week contianing jan 4'th");
            Assert.AreEqual(53, new DateTime(2010, 1, 1).Iso8601WeekOfYear(), "First week of 2010 is the week contianing jan 4'th");
            DateTime LastDay = new System.DateTime(2009, 12, 31);
            Trace.WriteLine(String.Format("There are {0} weeks in the year ({1}).", LastDay.Iso8601WeekOfYear(), LastDay.Year));

        }

        [TestMethod]
        public void MonthWeekNumberOverRangeTest() {
            
        }

    }
}
