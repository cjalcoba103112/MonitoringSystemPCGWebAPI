
using Microsoft.EntityFrameworkCore;
using Models;
using Models.NonTables;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;
using Repositories.Interfaces;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using Utilities.Classes;
using Utilities.Interfaces;

namespace Repositories.Classes
{
    public class PersonnelRepository : GenericRepository<Personnel>, IPersonnelRepository
    {
        private readonly DayUtility _dayUtility = new DayUtility();
        private readonly IEmailEteCommunicationRepository _emailEteCommunicationRepository = new EmailEteCommunicationRepository();
        public async Task<Personnel?> GetByIdAsync(int id)
        {
          return await _context.Personnel
               .Include(p => p.PersonnelActivities)
                   .ThenInclude(a => a.ActivityType)
               .Include(p => p.EnlistmentRecords)
               .Include(p => p.Rank)
                   .ThenInclude(a => a.RankCategory)
               .Include(p => p.Department)
               .Include(p => p.PersonnelPromotions)
               .Include(p=>p.User)
               .Where(p=>p.PersonnelId == id)
               .FirstOrDefaultAsync();

        }
        public async Task<IEnumerable<Personnel>> GetAllAsync(Personnel? filter = null)
        {
            IQueryable<Personnel> query = _context.Personnel
                .Include(p => p.PersonnelActivities)
                    .ThenInclude(a => a.ActivityType)
                .Include(p => p.EnlistmentRecords)
                .Include(p => p.Rank)
                    .ThenInclude(a => a.RankCategory)
                .Include(p => p.Department)
                .Include(p => p.PersonnelPromotions);


            // Apply dynamic filter
            if (filter != null)
            {
                var parameter = Expression.Parameter(typeof(Personnel), "p");
                Expression? combined = null;

                foreach (var property in typeof(Personnel).GetProperties())
                {
                    var value = property.GetValue(filter);
                    if (value == null) continue; // skip nulls

                    var member = Expression.Property(parameter, property);
                    var constant = Expression.Constant(value, property.PropertyType);
                    var equalsCheck = Expression.Equal(member, constant);

                    combined = combined == null ? equalsCheck : Expression.AndAlso(combined, equalsCheck);
                }

                if (combined != null)
                {
                    var lambda = Expression.Lambda<Func<Personnel, bool>>(combined, parameter);
                    query = query.Where(lambda);
                }
            }

            query = query.OrderBy(p => p.Rank!.RankLevel)
                         .ThenBy(p => p.SerialNumber);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<PersonnelLeaveDto>> GetPersonnelCreditsAsync(
       int personnelId,
       int? activityTypeId = null,
       int? year = null,
       DateTime? date = null) // Added date parameter
        {
            // 1. Setup reference points
            DateTime referenceDate = date ?? DateTime.Now;
            int targetYear = year ?? referenceDate.Year;
            int referenceMonth = referenceDate.Month;

            var activityTypes = await _context.ActivityType.ToListAsync();
            var personnel = await _context.Personnel
                .Include(p => p.PersonnelActivities)
                .FirstOrDefaultAsync(p => p.PersonnelId == personnelId);

            if (personnel == null) return null;

            var result = activityTypes
                .Where(at => activityTypeId == null || at.ActivityTypeId == activityTypeId)
                .Select(at =>
                {
                    int resetMonths = at.ResetMonths ?? 12;
                    int maxCredits = at.MaxCredits ?? 0;
                    bool isMandatory = at.IsMandatoryLeave ?? false;

                    int periodIndex = (referenceMonth - 1) / resetMonths;
                    int startMonthOfPeriod = (periodIndex * resetMonths) + 1;
                    int endMonthOfPeriod = startMonthOfPeriod + resetMonths - 1;

                    var used = personnel.PersonnelActivities?
                        .Where(a =>
                            a.ActivityTypeId == at.ActivityTypeId &&
                            a.StartDate != null &&
                            a.StartDate.Value.Year == targetYear &&
                            a.StartDate.Value.Month >= startMonthOfPeriod &&
                            a.StartDate.Value.Month <= endMonthOfPeriod &&
                            (a.Status != "Declined" && a.Status !="Pending Approval" && a.Status != "Suspended")
                        )
                        .Sum(a => _dayUtility.CountDays(a.StartDate!.Value, a.EndDate!.Value, isMandatory))
                        ?? 0;

                    return new PersonnelLeaveDto
                    {
                        ActivityTypeId = at.ActivityTypeId,
                        ActivityTypeName = at.ActivityTypeName,
                        MaxCredits = maxCredits,
                        UsedCredits = used,
                        RemainingCredits = Math.Max(0, maxCredits - used),
                        ResetMonths = resetMonths
                    };
                })
                .ToList();

            return result;
        }

        public async Task<IEnumerable<EnlistedPersonnelETE>> GetEnlismentETE(Personnel? filter = null)
        {
            IQueryable<Personnel> query = _context.Personnel
                .Include(p => p.PersonnelActivities)
                    .ThenInclude(a => a.ActivityType)
                .Include(p => p.EnlistmentRecords)
                .Include(p => p.Rank)
                    .ThenInclude(a => a.RankCategory)
                .Include(p => p.Department)
                .Where(p => p.Rank.RankCategoryId == 2);

            // Apply dynamic filter FIRST
            if (filter != null)
            {
                var parameter = Expression.Parameter(typeof(Personnel), "p");
                Expression? combined = null;

                foreach (var property in typeof(Personnel).GetProperties())
                {
                    var value = property.GetValue(filter);
                    if (value == null) continue;

                    var member = Expression.Property(parameter, property);
                    var constant = Expression.Constant(value, property.PropertyType);
                    var equalsCheck = Expression.Equal(member, constant);

                    combined = combined == null ? equalsCheck : Expression.AndAlso(combined, equalsCheck);
                }

                if (combined != null)
                {
                    var lambda = Expression.Lambda<Func<Personnel, bool>>(combined, parameter);
                    query = query.Where(lambda);
                }
            }

            var personnelList = await query
                .OrderBy(p => p.Rank!.RankLevel)
                .ThenBy(p => p.SerialNumber)
                .ToListAsync();

            var result = new List<EnlistedPersonnelETE>();

            foreach (var personnel in personnelList)
            {
                var latestEnlistmentStart = personnel.EnlistmentRecords?
                    .OrderByDescending(e => e.EnlistmentStart)
                    .FirstOrDefault()?.EnlistmentStart ?? personnel?.DateEnlisted;
                string remarks;
                var today = DateTime.UtcNow;

                DateTime? dateOfLatestReEnlistment = personnel?.EnlistmentRecords?.LastOrDefault()?.EnlistmentStart ?? personnel?.DateEnlisted;


                if (dateOfLatestReEnlistment.HasValue)
                {
                    while (dateOfLatestReEnlistment.Value.AddYears(3) < today)
                    {
                        dateOfLatestReEnlistment = dateOfLatestReEnlistment.Value.AddYears(3);
                    }

                }

                DateTime? nextEte = dateOfLatestReEnlistment?.AddYears(3);


                var daysRemaining = Math.Ceiling((nextEte - today)?.TotalDays ?? 0);
                if (personnel?.DateEnlisted == null)
                {
                    remarks = "NO RECORD";
                }
                else if (latestEnlistmentStart > today)
                {
                    remarks = "ALREADY SUBMITTED";
                }
                else
                {
                    if (daysRemaining < 0)
                        remarks = "EXPIRED";
                    else if (daysRemaining <= 30)
                        remarks = $"CRITICAL";
                    else if (daysRemaining <= 90)
                        remarks = $"NEAR ETE";
                    else
                        remarks = "ACTIVE";
                }

                var enlistedDate = personnel?.DateEnlisted;

                int yearsInService = today.Year - (enlistedDate?.Year ?? today.Year);

                if (today < enlistedDate?.AddYears(yearsInService))
                {
                    yearsInService--;
                }

               var emailEte =  await _emailEteCommunicationRepository.GetByPersonnelId(personnel?.PersonnelId??0, nextEte);
            
                result.Add(new EnlistedPersonnelETE
                {
                    Email = personnel.Email,
                    PersonnelId = personnel.PersonnelId,
                    SerialNumber = personnel.SerialNumber,
                    FirstName = personnel.FirstName,
                    MiddleName = personnel.MiddleName,
                    LastName = personnel.LastName,
                    RankId = personnel.RankId,
                    Rank = personnel.Rank,
                    DepartmentId = personnel.DepartmentId,
                    Department = personnel.Department,
                    EmploymentStatus = personnel.EmploymentStatus,
                    DateEnlisted = personnel.DateEnlisted,
                    PersonnelActivities = personnel.PersonnelActivities,
                    EnlistmentRecords = personnel.EnlistmentRecords,
                    DateEnteredService = personnel.DateEnteredService,
                    Profile = personnel.Profile,
                    ETEDaysRemaining = daysRemaining,
                    DateOfLatestReEnlistment = dateOfLatestReEnlistment,
                    NextETE = nextEte,
                    YearsInService = yearsInService,
                    Remarks = remarks,
                    EmailCategory = emailEte?.EmailCategory,
                    SupportingDocument = emailEte?.SupportingDocument
                });
            }

            return result;
        }
        public async Task<IEnumerable<Personnel>> GetNoActivities(
    DateTime? startDate = null,
    DateTime? endDate = null)
        {
            var query = _context.Personnel
                .Include(x => x.PersonnelActivities)
                .Include(x => x.Rank)
                .Include(x => x.Department)
                .OrderBy(x => x.Rank.RankLevel)
                .ThenBy(x => x.SerialNumber)
                .AsQueryable();

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(p =>
                        !p.PersonnelActivities.Any(a =>
                            a.Status == "Ongoing" &&
                            a.StartDate <= endDate &&
                            a.EndDate >= startDate
                        )
                    );
            }
            else
            {
                query = query.Where(p =>
          !p.PersonnelActivities.Any(a =>
              a.Status == "Ongoing"
          )
      );

            }

            return await query.ToListAsync();
        }
        public DateTime GetPeriodEnd(DateTime periodStart, int resetMonths)
        {
            return periodStart.AddMonths(resetMonths).AddDays(-1);
        }
        private DateTime GetPeriodStart(int resetMonths)
        {
            var now = DateTime.Now;

            int months = ((now.Month - 1) / resetMonths) * resetMonths + 1;

            return new DateTime(now.Year, months, 1);
        }
      
    }

}
