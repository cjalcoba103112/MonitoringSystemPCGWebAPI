
using Models;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;

namespace Services.Interfaces
{
    public interface IPersonnelActivityService
    {
        Task SendWarningEmailAsync(PersonnelActivity activity);
        Task<PersonnelActivity?> InsertAsync(PersonnelActivity data);
        Task<PersonnelActivity?> ApproveAsync(int? personnelActivityId, string? remarks);
        Task<PersonnelActivity?> DeclineAsync(int? personnelActivityId, string? remarks);
        Task<IEnumerable<PersonnelActivity>> GetActivitiesByPersonnelAsync(int personnelId, int? year = null);
        Task<PersonnelActivity?> UpdateAsync(PersonnelActivity data);
        Task<IEnumerable<PersonnelActivityDTO>> GetAllAsync(PersonnelActivity? filter = null);
        Task<PersonnelActivity?> GetByIdAsync(int id);
        Task<PersonnelActivity?> DeleteByIdAsync(int id);
        Task<IEnumerable<PersonnelActivity>> BulkInsertAsync(List<PersonnelActivity> data);
        Task<IEnumerable<PersonnelActivity>> BulkUpdateAsync(List<PersonnelActivity> data);
        Task<IEnumerable<PersonnelActivity>> BulkUpsertAsync(List<PersonnelActivity> data);
        Task<IEnumerable<PersonnelActivity>> BulkMergeAsync(List<PersonnelActivity> data);
    }
}
