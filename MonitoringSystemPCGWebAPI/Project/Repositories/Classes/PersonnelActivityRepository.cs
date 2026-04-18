
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;

namespace Repositories.Classes
{
    public class PersonnelActivityRepository : GenericRepository<PersonnelActivity>, IPersonnelActivityRepository
    {

        public string GetActivityStatus(DateTime? startDate, DateTime? endDate,string? currentStatus = "")
        {
            if (currentStatus == "Suspended") return currentStatus;
            var today = DateTime.UtcNow.Date;

            // Inactive → already ended
            if (endDate.HasValue && today > endDate.Value.Date)
                return "Inactive";

            // Ongoing → within range
            if (startDate.HasValue && endDate.HasValue &&
                startDate.Value.Date <= today &&
                today <= endDate.Value.Date)
                return "Ongoing";

            // Pending → not yet started or missing dates
            return "Pending";
        }
        public async Task<IEnumerable<PersonnelActivity>> GetByPersonnelIdAsync(int personnelId, int? year = null)
        {
            int targetYear = year ?? DateTime.UtcNow.Year;

            return await _context.PersonnelActivity.Include(a => a.ActivityType)
                .Where(a => a.PersonnelId == personnelId &&
                            a.StartDate.HasValue &&
                            a.StartDate.Value.Year == targetYear)
                .OrderByDescending(a => a.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PersonnelActivity>> GetOverDue24HoursActivities(PersonnelActivity personnelActivity)
        {
            var cutoff = DateTime.Now.AddHours(-24);

            var overdueActivities = await _context.PersonnelActivity
                 .Where(pa => pa.EndDate <= cutoff         
                           && pa.IsFullyApproved == true) 
                 .ToListAsync();

            return overdueActivities;
        }
    }
}
