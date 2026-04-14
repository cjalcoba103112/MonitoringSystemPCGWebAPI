
using Models;

namespace Services.Interfaces
{
    public interface ISidebarService
    {
        Task<Sidebar?> InsertAsync(Sidebar data);
        Task<Sidebar?> UpdateAsync(Sidebar data);
        Task<IEnumerable<Sidebar>> GetAllAsync(Sidebar? filter);
        Task<Sidebar?> GetByIdAsync(int id);
        Task<Sidebar?> DeleteByIdAsync(int id);
        Task<IEnumerable<Sidebar>> BulkInsertAsync(List<Sidebar> data);
        Task<IEnumerable<Sidebar>> BulkUpdateAsync(List<Sidebar> data);
        Task<IEnumerable<Sidebar>> BulkUpsertAsync(List<Sidebar> data);
        Task<IEnumerable<Sidebar>> BulkMergeAsync(List<Sidebar> data);
    }
}
