using ApplicationContexts;
using Microsoft.EntityFrameworkCore;
using Models;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;
using MonitoringSystemPCGWebAPI.Project.Services.Interfaces;
using Repositories.Interfaces;

namespace MonitoringSystemPCGWebAPI.Project.Services.Classes
{
    public class DashboardService : IDashboardService
    {
        public readonly ApplicationContext _context = new ApplicationContext();
        private readonly IActivityTypeRepository _activityTypeRepository;
        private readonly IPersonnelActivityRepository _personnelActivityRepository;
        private readonly IPersonnelRepository _personnelRepository;
        public DashboardService(IActivityTypeRepository activityTypeRepository, IPersonnelActivityRepository personnelActivityRepository, IPersonnelRepository personnelRepository)
        {
            _activityTypeRepository = activityTypeRepository;
            _personnelActivityRepository = personnelActivityRepository;
            _personnelRepository = personnelRepository;
        }

        public async Task<IEnumerable<Personnel>> GetPersonnelOngoingActivities()
        {
            var personnels = await _personnelRepository.GetAllAsync();

            // Keep only ongoing activities
            foreach (var person in personnels)
            {
                if (person.PersonnelActivities != null)
                {
                    person.PersonnelActivities = person.PersonnelActivities
                        .Where(a => a.Status == "Ongoing")
                        .ToList();
                }
            }

            return personnels;
        }

        public async Task<IEnumerable<PersonnelByActivityType>> GetPersonnelByActivityType(DateTime? startDate = null, DateTime? endDate = null)
        {
            List<PersonnelByActivityType> personnelPerActivities = new List<PersonnelByActivityType>();


            IEnumerable<ActivityType> activityTypes = await _activityTypeRepository.GetAllAsync();



            foreach (ActivityType activityType in activityTypes)
            {
                IEnumerable<PersonnelActivity> personnelActivities = await _personnelActivityRepository.GetAllAsync(new PersonnelActivity
                {
                    ActivityTypeId = activityType.ActivityTypeId
                }, x => x.Personnel.Rank);

                personnelActivities = personnelActivities.Where(x => x.Status == "Ongoing");

                if (personnelActivities.Count() == 0) continue;

                List<InfoData> info = personnelActivities
      .Select(c => new InfoData
      {
          Name = c.Personnel,
          Title = c.Title,
          EndDate = c.EndDate,
          StartDate = c.StartDate
      })
      .ToList();

                personnelPerActivities.Add(new PersonnelByActivityType
                {
                    Activity = activityType.ActivityTypeName,
                    Personnel = personnelActivities.Count(),
                    Info = info
                });
            }

            //PersonnelNoActivities
            var personnelsNoActivities = await _personnelRepository.GetNoActivities();
            List<InfoData> personnelNoActivitiesInfo = personnelsNoActivities
.Select(c => new InfoData
{
    Name = c
})
.ToList();
            personnelPerActivities.Add(new PersonnelByActivityType
            {
                Activity = "On duty",
                Personnel = personnelsNoActivities.Count(),
                Info = personnelNoActivitiesInfo
            });


            return personnelPerActivities;
        }

        public async Task<IEnumerable<PersonnelByDepartment>> GetPersonnelByDepartment()
        {
            // Get all personnel including Department
            var personnels = await _personnelRepository.GetAllAsync(null, x => x.Department, x => x.Rank, x => x.PersonnelActivities);


            foreach (var p in personnels)
            {
                foreach (var pa in p.PersonnelActivities)
                {

                    if (pa.Status != "Ongoing")
                    {
                        p.PersonnelActivities.Remove(pa);
                    }
                    else
                    {
                        pa.ActivityType = await _activityTypeRepository.GetByIdAsync(pa.ActivityTypeId ?? 0);
                    }

                }
            }

            // Group by Department
            var grouped = personnels
                .GroupBy(p => p.Department != null ? p.Department.DepartmentName : "Unknown")
                .Select(g => new PersonnelByDepartment
                {
                    Department = g.Key,
                    Personnel = g.Count(),
                    names = g.ToList()
                })
                .OrderBy(g => g.Department)
                .ToList();

            return grouped;
        }

        public async Task<List<PersonnelByDepartmentAndActivity>> GetPersonnelByDepartmentAndActivity(DateTime? startDate = null, DateTime? endDate = null)
        {

            var personnels = await _personnelRepository.GetAllAsync(
        null,
        x => x.Department,
        x => x.Rank,
        x => x.PersonnelActivities
    );

            var result = new List<PersonnelByDepartmentAndActivity>();


            foreach (var p in personnels)
            {
                var departmentName = p.Department?.DepartmentName ?? "N/A";


                var activeActivities = p.PersonnelActivities?
                    .Where(pa => pa.Status == "Ongoing")
                    .ToList();


                if (p.PersonnelActivities != null)
                {
                    p.PersonnelActivities = p.PersonnelActivities
                        .Where(x => x.Status == "Ongoing")
                        .ToList();
                }

                if (activeActivities == null || !activeActivities.Any())
                {
                    AddOrUpdate(result, departmentName, "On Duty", p);
                    continue;
                }

                // ✅ Process active activities only
                foreach (var pa in activeActivities)
                {
                    var activityType = pa.ActivityType
                        ?? await _activityTypeRepository.GetByIdAsync(pa.ActivityTypeId ?? 0);

                    var activityName = activityType?.ActivityTypeName ?? "On Duty";

                    AddOrUpdate(result, departmentName, activityName, p);
                }
            }

            return result;



        }

        private void AddOrUpdate(
        List<PersonnelByDepartmentAndActivity> list,
        string department,
        string activity,
        Personnel personnel)
        {
            var existing = list.FirstOrDefault(r => r.Department == department && r.Activity == activity);
            if (existing != null)
            {
                // Combine: increment count and add personnel to names
                if (!existing.Names.Any(p => p.PersonnelId == personnel.PersonnelId))
                {
                    existing.Names.Add(personnel);
                    existing.Personnel = existing.Names.Count;
                }
            }
            else
            {
                // Add new entry
                list.Add(new PersonnelByDepartmentAndActivity
                {
                    Department = department,
                    Activity = activity,
                    Personnel = 1,
                    Names = new List<Personnel> { personnel }
                });
            }
        }
    }
}
