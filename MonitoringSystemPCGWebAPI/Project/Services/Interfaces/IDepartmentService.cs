
using Models;

namespace Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<Department?> InsertAsync(Department data);
        Task<Department?> UpdateAsync(Department data);
        Task<IEnumerable<Department>> GetAllAsync(Department? filter);
        Task<Department?> GetByIdAsync(int id);
        Task<Department?> DeleteByIdAsync(int id);
        Task<IEnumerable<Department>> BulkInsertAsync(List<Department> data);
        Task<IEnumerable<Department>> BulkUpdateAsync(List<Department> data);
        Task<IEnumerable<Department>> BulkUpsertAsync(List<Department> data);
        Task<IEnumerable<Department>> BulkMergeAsync(List<Department> data);
    }
}
