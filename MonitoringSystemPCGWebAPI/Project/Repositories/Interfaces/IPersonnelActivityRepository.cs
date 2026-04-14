
using Models;

namespace Repositories.Interfaces
{
    public interface IPersonnelActivityRepository : IGenericRepository<PersonnelActivity>
    {
        string GetActivityStatus(DateTime? startDate, DateTime? endDate, string? currentStatus = "");
        Task<IEnumerable<PersonnelActivity>> GetByPersonnelIdAsync(int personnelId, int? year = null);
    }
}
