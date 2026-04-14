
using Models;

namespace Services.Interfaces
{
    public interface IEnlistmentRecordService
    {
        Task<EnlistmentRecord?> InsertAsync(EnlistmentRecord data);
        Task<EnlistmentRecord?> UpdateAsync(EnlistmentRecord data);
        Task<IEnumerable<EnlistmentRecord>> GetAllAsync(EnlistmentRecord? filter);
        Task<EnlistmentRecord?> GetByIdAsync(int id);
        Task<EnlistmentRecord?> DeleteByIdAsync(int id);
        Task<IEnumerable<EnlistmentRecord>> BulkInsertAsync(List<EnlistmentRecord> data);
        Task<IEnumerable<EnlistmentRecord>> BulkUpdateAsync(List<EnlistmentRecord> data);
        Task<IEnumerable<EnlistmentRecord>> BulkUpsertAsync(List<EnlistmentRecord> data);
        Task<IEnumerable<EnlistmentRecord>> BulkMergeAsync(List<EnlistmentRecord> data);
    }
}
