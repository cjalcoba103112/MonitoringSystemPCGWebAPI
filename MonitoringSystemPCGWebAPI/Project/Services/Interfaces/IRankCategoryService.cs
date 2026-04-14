
using Models;

namespace Services.Interfaces
{
    public interface IRankCategoryService
    {
        Task<RankCategory?> InsertAsync(RankCategory data);
        Task<RankCategory?> UpdateAsync(RankCategory data);
        Task<IEnumerable<RankCategory>> GetAllAsync(RankCategory? filter);
        Task<RankCategory?> GetByIdAsync(int id);
        Task<RankCategory?> DeleteByIdAsync(int id);
        Task<IEnumerable<RankCategory>> BulkInsertAsync(List<RankCategory> data);
        Task<IEnumerable<RankCategory>> BulkUpdateAsync(List<RankCategory> data);
        Task<IEnumerable<RankCategory>> BulkUpsertAsync(List<RankCategory> data);
        Task<IEnumerable<RankCategory>> BulkMergeAsync(List<RankCategory> data);
    }
}
