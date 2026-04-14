using Utilities.Interfaces;
using System.Collections;

namespace Utilities.Classes
{
    public class DayUtility : IDayUtility
    {
        public decimal CountDays(DateTime? start, DateTime? end, bool? isMandatory = false)
        {
            if (start == null || end == null) throw new Exception("Invalid dates");

         
            if (start.Value.Date == end.Value.Date) return 1;

            decimal days = 0;
            for (var date = start.Value.Date; date < end.Value.Date; date = date.AddDays(1))
            {
                if (isMandatory == true &&
                   (date.DayOfWeek == DayOfWeek.Saturday ||
                    date.DayOfWeek == DayOfWeek.Sunday))
                    continue;

                days += 1;
            }

            return days;
        }
    }
}
