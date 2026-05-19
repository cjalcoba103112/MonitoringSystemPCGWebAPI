
using Models;

namespace Repositories.Interfaces
{
    public interface IPersonnelActivityRepository : IGenericRepository<PersonnelActivity>
    {
        Task<PersonnelActivity?> GetActivityWithApprovalDetailsAsync(int approvalId);
        Task<IEnumerable<PersonnelActivity>> GetAllAsync(PersonnelActivity personnelActivity);
        string GetActivityStatus(DateTime? startDate, DateTime? endDate, string? currentStatus = "");
        Task<IEnumerable<PersonnelActivity>> GetByPersonnelIdAsync(int personnelId, int? year = null);
        Task<IEnumerable<PersonnelActivity>> GetOverDue24HoursActivities(PersonnelActivity personnelActivity);
    }
}
