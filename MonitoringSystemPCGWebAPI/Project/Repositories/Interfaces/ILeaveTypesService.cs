
using Models;

namespace Services.Interfaces
{
    public interface ILeaveTypesService
    {
        Task<LeaveTypes?> InsertAsync(LeaveTypes data);
        Task<LeaveTypes?> UpdateAsync(LeaveTypes data);
        Task<IEnumerable<LeaveTypes>> GetAllAsync(LeaveTypes? filter);
        Task<LeaveTypes?> GetByIdAsync(int id);
        Task<LeaveTypes?> DeleteByIdAsync(int id);
        Task<IEnumerable<LeaveTypes>> BulkInsertAsync(List<LeaveTypes> data);
        Task<IEnumerable<LeaveTypes>> BulkUpdateAsync(List<LeaveTypes> data);
        Task<IEnumerable<LeaveTypes>> BulkUpsertAsync(List<LeaveTypes> data);
        Task<IEnumerable<LeaveTypes>> BulkMergeAsync(List<LeaveTypes> data);
    }
}
