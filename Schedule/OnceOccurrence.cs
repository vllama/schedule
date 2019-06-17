using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLL.Schedule
{
    public class OnceOccurrence : AOccurrence { 

        public OnceOccurrence(DateTime? start) : base(start,start) {

        }

        /// <summary>
        /// Creates a new OnceOccurrence which occurrs Now at the server.
        /// </summary>
        /// <returns>an IOccurrence</returns>
        public static IOccurrence Now() {
            return new OnceOccurrence(DateTime.UtcNow.Date);
        }

        /// <summary>
        /// get the first occurrence after the given date
        /// </summary>
        /// <param name="from"></param>
        /// <param name="asDate"></param>
        /// <returns></returns>
        public override DateTime? GetNext(DateTime from, bool asDate = false) {
            from = from.Date; // times cause funny things

            if (from <= End && from >= Start)  {
                if (asDate)
                    return Start.Date;
                return Start;
            }
            return null;
        }
    }
}
