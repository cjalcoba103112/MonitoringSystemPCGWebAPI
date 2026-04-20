
using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Classes
{
    public class ApprovalProccessService : IApprovalProccessService
    {
        private readonly IApprovalProccessRepository _approvalProccessRepository;

        public ApprovalProccessService(IApprovalProccessRepository approvalProccessRepository)
        {
            _approvalProccessRepository = approvalProccessRepository;
        }

        public async Task<ApprovalProccess?> InsertAsync(ApprovalProccess data)
        {
            return await _approvalProccessRepository.InsertAsync(data);
        }

        public async Task<ApprovalProccess?> UpdateAsync(ApprovalProccess data)
        {
            return await _approvalProccessRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<ApprovalProccess>> GetAllAsync(ApprovalProccess? filter)
        {
            return await _approvalProccessRepository.GetAllAsync(filter);
        }

        public async Task<ApprovalProccess?> GetByIdAsync(int id)
        {
            return await _approvalProccessRepository.GetByIdAsync(id);
        }

        public async Task<ApprovalProccess?> DeleteByIdAsync(int id)
        {
            return await _approvalProccessRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<ApprovalProccess>> BulkInsertAsync(List<ApprovalProccess> data)
        {
            return await _approvalProccessRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<ApprovalProccess>> BulkUpdateAsync(List<ApprovalProccess> data)
        {
            return await _approvalProccessRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<ApprovalProccess>> BulkUpsertAsync(List<ApprovalProccess> data)
        {
            return await _approvalProccessRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<ApprovalProccess>> BulkMergeAsync(List<ApprovalProccess> data)
        {
            return await _approvalProccessRepository.BulkMergeAsync(data);
        }
    }
}