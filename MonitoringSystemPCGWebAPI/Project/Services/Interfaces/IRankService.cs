
using Models;

namespace Services.Interfaces
{
    public interface IRankService
    {
        Task<Rank?> InsertAsync(Rank data);
        Task<Rank?> UpdateAsync(Rank data);
        Task<IEnumerable<Rank>> GetAllAsync(Rank? filter);
        Task<Rank?> GetByIdAsync(int id);
        Task<Rank?> DeleteByIdAsync(int id);
        Task<IEnumerable<Rank>> BulkInsertAsync(List<Rank> data);
        Task<IEnumerable<Rank>> BulkUpdateAsync(List<Rank> data);
        Task<IEnumerable<Rank>> BulkUpsertAsync(List<Rank> data);
        Task<IEnumerable<Rank>> BulkMergeAsync(List<Rank> data);
    }
}
