using System;
using System.Collections.Generic;
using System.Linq;

using VLL.Schedule.Extensions;

namespace VLL.Schedule {
   public class MonthlyOccurrence : ARecurrence {

       protected override string SpanUnit => "Month";

       private readonly int[] VALID_DATES_OF_MONTH = new[] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31
                                                 ,32 // = last
                                                };

        private IList<Month> _months = null;
        // either thes two
        private IList<WeekOfMonth> _weekOfMonth = null;
        private IList<DayOfWeek> _daysOfWeek = null;
        //or dates
        private IList<int> _datesOfMonth =null;


        #region Constructors

        /// <summary>
        /// A new Monthly Occurrence
        /// </summary>
        /// <param name="start">Starts on start date</param>
        /// <param name="end">Ends on end date</param>
        /// <param name="months">Which months of the year: (Jan-Dec)</param>
        private MonthlyOccurrence(DateTime? start, DateTime? end, Month[] months) : base(start, end) {

            if (months == null || months.Length == 0) {
                throw new ArgumentException("Months of Month cannot be empty.", nameof(months));
            }
            // distinct list
            this._months = months.Distinct().OrderBy(x => x).ToList();
        }

        /// <summary>
        /// A new Monthly Occurrence
        /// </summary>
        /// <param name="start">Starts on start date</param>
        /// <param name="end">Ends on end date</param>
        /// <param name="months">Which months of the year: (Jan-Dec)</param>
        /// <param name="daysOfMonth">Which days of the week of the month. Example ( first, second, third, fourth or last) Monday.. Sunday of the month.</param>
        /// <param name="weekDays">Which days of the week in the ISO8601 range of Monday -> Sunday </param>
        public MonthlyOccurrence(DateTime? start, DateTime? end, Month[] months, WeekOfMonth[] daysOfMonth, DayOfWeek[] weekDays) :
             this(start, end, months) {
            if (daysOfMonth == null || daysOfMonth.Length == 0) {
                throw new ArgumentException("Weeks of Month cannot be empty.", nameof(daysOfMonth));
            }
            if (weekDays == null || weekDays.Length == 0) {
                throw new ArgumentException("Days of Week cannot be empty.", nameof(weekDays));
            }
            _weekOfMonth = daysOfMonth.OrderBy(x => x).Distinct().ToList();
            _daysOfWeek = weekDays.OrderBy(x => x).Distinct().ToList();

        }

        /// <summary>
        /// A new Monthly Occurrence
        /// </summary>
        /// <param name="start">Starts on start date</param>
        /// <param name="end">Ends on end date</param>
        /// <param name="months">Which months of the year: (Jan-Dec)</param>
        /// <param name="dateOfMonth">Which dates of the months. 1-"Last" day of the month Last day is in the range (28,29,30,31) depending on the month.</param>
        public MonthlyOccurrence(DateTime? start, DateTime? end, Month[] months, int[] dateOfMonth) :
             this(start, end, months) {

            if (dateOfMonth == null || dateOfMonth.Length == 0) {
                throw new ArgumentException("Days of the Month cannot be empty.", nameof(dateOfMonth));
            }

            if (dateOfMonth.ToList().Except(VALID_DATES_OF_MONTH).Any()) {
                throw new ArgumentException("Only Valid Dates of the month are allowed", nameof(dateOfMonth));
            }
            _datesOfMonth = dateOfMonth.OrderBy(x => x).Distinct().ToList();
        }
        #endregion



        /// <see cref="IOccurrence.GetNext(DateTime,bool)"/> for more information.
        public override DateTime? GetNext(DateTime from, bool asDate = false) {
            if (from < this.Start || from > this.End) {
                return null;
            }
            if (this._datesOfMonth == null) {
                return NextWeekDaysOfMonth(from);
            } else {
                return NextDaysOfMonth(from); // asDate has no effec here.
            }
            throw new NotImplementedException("Not all branches were implemented.");
        }


        /// <summary>
        /// Method Implements calculating if ia day is one of the _weekDaysOfMonth x _daysOfTheWeek of the _months
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        private DateTime? NextWeekDaysOfMonth(DateTime from) {
            if (_daysOfWeek == null)
                throw new ApplicationException("Cannot get NextDaysInMonth without daysOfWeek being specified. Did you use the wrong constructor?");
            if (_weekOfMonth == null)
                throw new ApplicationException("Cannot get NextDaysInMonth without weekOfMonth being specified. Did you use the wrong constructor?");
            //example 1 3 monday and wednesday of the month
            int fromYear = from.Year;
            int fromMonth = from.Month;
            int fromDay = from.Day;
            
            // Iterate through the year starting at the year of the "from" date
            foreach (var year in Enumerable.Range(from.Year, End.Year - from.Year + 1)) {
                //Iterate through the months starting at the month of the "from" date
                foreach (var month in _months.Where(mon => from.Month >= (int)mon)) {
                    //Get a list of all the days in the month
                    var range = Enumerable.Range(1, DateTime.DaysInMonth(year, (int)month));

                    // Get a list of all the Days in the month from Range, where the weekday is one of the _daysOfTheWeek
                    // example get all Tuesdays and Thursdays of the days in the range
                    var daysOfTheWeekInRange = range.Where(d=>_daysOfWeek.Contains(new DateTime(year, (int)month, d).DayOfWeek));

                    foreach (var day in daysOfTheWeekInRange) {
                        var DateToTest = new DateTime(year, (int)month, day);
                        if (DateToTest < from) {
                            continue;
                        }
                        var ordinal = DateToTest.GetOrdinalInMonth();

                        //todo.. bug here.. is contains the right thing?
                        //_weekWeekOfMonth is assumed to be sorted.  Does contains find the first one??? TODO:test
                        if (_weekOfMonth.Contains(ordinal)){
                            return DateToTest;
                        }
                    }
                }
            }
            return null;
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        private DateTime? NextDaysOfMonth(DateTime from) {
            if (_datesOfMonth == null)
                throw new ApplicationException("Cannot get NextDaysInmonth without dateOfMonth being specified. Did you use the wrong constructor?");

            int fromYear = from.Year;
            int fromMonth = from.Month;
            int fromDay = from.Day;

            foreach (var year in Enumerable.Range(from.Year, End.Year - from.Year + 1)) {
                foreach (var month in _months) {
                    foreach (var d in _datesOfMonth) {
                        // support special case of "Last" day of the month
                        var day = d;
                        if (d == 32) { // magic nuber here
                            day = DateTime.DaysInMonth(year, (int)month);
                        }
                        #region EarlyOptimization

                        //-- 17 seconds assume .net does only one allocation as test below proves
                        /*if (day <= DateTime.DaysInMonth(year, (int)month)
                            && new DateTime(year, (int)month ,day) >= from
                            && new DateTime(year, (int)month, day) <= End ) {
                            return new DateTime(year, (int)month, day);
                        }*/
                        //-- 17 seconds
                        // 
                        /*  if (day <= DateTime.DaysInMonth(year, (int)month)) { // skip non existant dates
                              var dtToTest = new DateTime(year, (int)month, day);
                              if (dtToTest >= from) {
                                  return dtToTest;

                              }
                          }*/
                        #endregion
                        // Todo: create a cache of daysinmonth if speed becomes an issue
                        // 11 seconds ( shave 5 seconds by not allocating a datetime at every iteration when running across all dates)
                        if (day <= DateTime.DaysInMonth(year, (int)month)) { // skip non existant dates
                            if (year >= fromYear &&  (int)month >= fromMonth && day >= fromDay) {
                                return new DateTime(year, (int)month, day); 
                            }
                        }
                    }
                }
            }
            return null;
        }

      
    }


}
