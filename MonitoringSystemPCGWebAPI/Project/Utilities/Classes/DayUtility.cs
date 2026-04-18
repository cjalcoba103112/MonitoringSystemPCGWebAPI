using Utilities.Interfaces;
using System.Collections;

namespace Utilities.Classes
{
    public class DayUtility : IDayUtility
    {
        public decimal CountDays(DateTime? start, DateTime? end, bool? isMandatory = false)
        {
            if (start == null || end == null) throw new Exception("Invalid dates");

            decimal days = 0;

            // Change < to <= to make the end date inclusive
            for (var date = start.Value.Date; date <= end.Value.Date; date = date.AddDays(1))
            {
                // Logic: If it's mandatory, skip counting the weekends
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
