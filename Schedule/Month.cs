
namespace VLL.Schedule {

    /// <summary>
    /// Ordinal moths of the year
    /// Numbers can be used , January is 1
    /// Empty moved to 13 to match : CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName 
    /// </summary>
    public enum Month {
        January = 1, February, March, April, May, June, July, August, September, October, November, December
        , Empty
    }
}
