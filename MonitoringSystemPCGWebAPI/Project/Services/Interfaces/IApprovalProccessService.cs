
using Models;

namespace Services.Interfaces
{
    public interface IApprovalProccessService
    {
        Task<ApprovalProccess?> InsertAsync(ApprovalProccess data);
        Task<ApprovalProccess?> UpdateAsync(ApprovalProccess data);
        Task<IEnumerable<ApprovalProccess>> GetAllAsync(ApprovalProccess? filter);
        Task<ApprovalProccess?> GetByIdAsync(int id);
        Task<ApprovalProccess?> DeleteByIdAsync(int id);
        Task<IEnumerable<ApprovalProccess>> BulkInsertAsync(List<ApprovalProccess> data);
        Task<IEnumerable<ApprovalProccess>> BulkUpdateAsync(List<ApprovalProccess> data);
        Task<IEnumerable<ApprovalProccess>> BulkUpsertAsync(List<ApprovalProccess> data);
        Task<IEnumerable<ApprovalProccess>> BulkMergeAsync(List<ApprovalProccess> data);
    }
}
