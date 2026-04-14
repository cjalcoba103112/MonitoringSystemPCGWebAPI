
using Models;
using Models.NonTables;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;

namespace Services.Interfaces
{
    public interface IPersonnelService
    {
        Task<IEnumerable<PersonnelLeaveDto>> GetPersonnelCreditsAsync(int personnelId, int? activityTypeId = null, int? year = null, DateTime? date = null);
        Task<IEnumerable<EnlistedPersonnelETE>> GetEnlismentETE(Personnel? filter = null);
        Task<Personnel?> InsertAsync(Personnel data, IFormFile? profile);
        Task<Personnel?> UpdateAsync(Personnel data, IFormFile profile);
        Task<IEnumerable<Personnel>> GetAllAsync(Personnel? filter);
        Task<Personnel?> GetByIdAsync(int id);
        Task<Personnel?> DeleteByIdAsync(int id);
        Task<IEnumerable<Personnel>> BulkInsertAsync(List<Personnel> data);
        Task<IEnumerable<Personnel>> BulkUpdateAsync(List<Personnel> data);
        Task<IEnumerable<Personnel>> BulkUpsertAsync(List<Personnel> data);
        Task<IEnumerable<Personnel>> BulkMergeAsync(List<Personnel> data);
    }
}
