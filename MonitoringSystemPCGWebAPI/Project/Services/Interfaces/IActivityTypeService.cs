
using Models;

namespace Services.Interfaces
{
    public interface IActivityTypeService
    {
        Task<ActivityType?> InsertAsync(ActivityType data);
        Task<ActivityType?> UpdateAsync(ActivityType data);
        Task<IEnumerable<ActivityType>> GetAllAsync(ActivityType? filter);
        Task<ActivityType?> GetByIdAsync(int id);
        Task<ActivityType?> DeleteByIdAsync(int id);
        Task<IEnumerable<ActivityType>> BulkInsertAsync(List<ActivityType> data);
        Task<IEnumerable<ActivityType>> BulkUpdateAsync(List<ActivityType> data);
        Task<IEnumerable<ActivityType>> BulkUpsertAsync(List<ActivityType> data);
        Task<IEnumerable<ActivityType>> BulkMergeAsync(List<ActivityType> data);
    }
}
