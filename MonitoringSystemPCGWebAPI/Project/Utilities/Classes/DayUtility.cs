using System;
using Utilities.Interfaces;

namespace Utilities.Classes
{
    public class DayUtility : IDayUtility
    {
        public decimal CountDays(DateTime? start, DateTime? end, bool? isMandatory = false)
        {
            if (start == null || end == null) throw new Exception("Invalid dates");
            if (start > end) return 0;

            decimal days = 0;
            var startDate = start.Value.Date;
            var endDate = end.Value.Date;

            // 1. Calculate the base count
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // If mandatory, skip weekends only
                if (isMandatory == true &&
                   (date.DayOfWeek == DayOfWeek.Saturday ||
                    date.DayOfWeek == DayOfWeek.Sunday))
                {
                    continue;
                }

                days += 1;
            }

            // 2. If it is NOT mandatory, apply the -1 day rule 
            // This ensures March 05-20 (16 inclusive days) becomes 15 days.
            // And April 17-20 (4 inclusive days) becomes 3 days.
            if (isMandatory == false && days > 0)
            {
                days -= 1;
            }

            return days;
        }
    }
}