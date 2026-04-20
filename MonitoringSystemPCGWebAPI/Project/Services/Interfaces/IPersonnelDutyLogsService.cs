
using Models;

namespace Services.Interfaces
{
    public interface IPersonnelDutyLogsService
    {
        Task<PersonnelDutyLogs?> InsertAsync(PersonnelDutyLogs data);
        Task<PersonnelDutyLogs?> UpdateAsync(PersonnelDutyLogs data);
        Task<IEnumerable<PersonnelDutyLogs>> GetAllAsync(PersonnelDutyLogs? filter);
        Task<PersonnelDutyLogs?> GetByIdAsync(int id);
        Task<PersonnelDutyLogs?> DeleteByIdAsync(int id);
        Task<IEnumerable<PersonnelDutyLogs>> BulkInsertAsync(List<PersonnelDutyLogs> data);
        Task<IEnumerable<PersonnelDutyLogs>> BulkUpdateAsync(List<PersonnelDutyLogs> data);
        Task<IEnumerable<PersonnelDutyLogs>> BulkUpsertAsync(List<PersonnelDutyLogs> data);
        Task<IEnumerable<PersonnelDutyLogs>> BulkMergeAsync(List<PersonnelDutyLogs> data);
    }
}
