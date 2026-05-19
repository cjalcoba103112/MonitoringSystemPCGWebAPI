
using Models;

namespace Services.Interfaces
{
    public interface IActivityAppealService
    {
        Task<ActivityAppeal?> GetByTokenAsync(string token);
        Task<ActivityAppeal?> InsertAsync(ActivityAppeal data);
        Task<ActivityAppeal?> SubmitAsync(ActivityAppeal data);
        Task<IEnumerable<ActivityAppeal>> GetAllAsync(ActivityAppeal? filter);
        Task<ActivityAppeal?> GetByIdAsync(int id);
        Task<ActivityAppeal?> DeleteByIdAsync(int id);
        Task<IEnumerable<ActivityAppeal>> BulkInsertAsync(List<ActivityAppeal> data);
        Task<IEnumerable<ActivityAppeal>> BulkUpdateAsync(List<ActivityAppeal> data);
        Task<IEnumerable<ActivityAppeal>> BulkUpsertAsync(List<ActivityAppeal> data);
        Task<IEnumerable<ActivityAppeal>> BulkMergeAsync(List<ActivityAppeal> data);
    }
}
