
using Models;

namespace Services.Interfaces
{
    public interface IRoleService
    {
        Task<Role?> InsertAsync(Role data);
        Task<Role?> UpdateAsync(Role data);
        Task<IEnumerable<Role>> GetAllAsync(Role? filter);
        Task<Role?> GetByIdAsync(int id);
        Task<Role?> DeleteByIdAsync(int id);
        Task<IEnumerable<Role>> BulkInsertAsync(List<Role> data);
        Task<IEnumerable<Role>> BulkUpdateAsync(List<Role> data);
        Task<IEnumerable<Role>> BulkUpsertAsync(List<Role> data);
        Task<IEnumerable<Role>> BulkMergeAsync(List<Role> data);
    }
}
