using System;

namespace VLL.Schedule
{
    public interface IOccurrence {

        /// <summary>
        /// The start date of the ocurrence
        /// </summary>
        DateTime Start { get; }

        /// <summary>
        /// The End date of the ocurrence (End >= Start)
        /// </summary>
        DateTime End { get; }

        /// <summary>
        /// Subclasses Must Implement this method which gets the next occurrence which falls on or after from
        /// </summary>
        /// <param name="from">The date from which to test</param>
        /// <param name="asDate">Return as a Date ("Removes the time component") by calling .Date on the returned value</param>
        /// <returns>the next date occurring on or after from for which the occurrence will occur, or null if none exists.</returns>
        DateTime? GetNext(DateTime from, bool asDate = false);
    }
}
