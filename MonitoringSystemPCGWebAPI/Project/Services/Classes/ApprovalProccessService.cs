
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Classes;
using Repositories.Interfaces;
using Services.Interfaces;
using static System.Net.WebRequestMethods;

namespace Services.Classes
{
    public class ApprovalProccessService : IApprovalProccessService
    {
        private readonly IApprovalProccessRepository _approvalProccessRepository;
        private readonly IPersonnelActivityRepository _personnelActivityRepository;
        public ApprovalProccessService(IApprovalProccessRepository approvalProccessRepository, IPersonnelActivityRepository personnelActivityRepository)
        {
            _approvalProccessRepository = approvalProccessRepository;
            _personnelActivityRepository = personnelActivityRepository;
        }

        public async Task<ApprovalProccess?> InsertAsync(ApprovalProccess data)
        {
            return await _approvalProccessRepository.InsertAsync(data);
        }

        public async Task<ApprovalProccess?> UpdateByCMAA(ApprovalProccess data)
        {
            var approval = await _approvalProccessRepository.GetByIdAsync(data?.Id??0);
            if (approval == null) throw new Exception("Approval Process is not found.");

            approval.CmaaRemarks = data.CmaaRemarks;
            approval.CmaaId = data.CmaaId;
            approval.CmaaIsApprove = data.CmaaIsApprove;
            approval.CurrentStage = (approval.CmaaIsApprove ?? false ) ? 2 : approval.CurrentStage;

            return await _approvalProccessRepository.UpdateAsync(approval);
        }
        public async Task<ApprovalProccess?> UpdateByOIC(ApprovalProccess data)
        {
            var approval = await _approvalProccessRepository.GetByIdAsync(data?.Id ?? 0);
            if (approval == null) throw new Exception("Approval Process is not found.");

            approval.OicRemarks = data.OicRemarks;
            approval.OicId = data.OicId;
            approval.OicIsApprove = data.OicIsApprove;
            approval.CurrentStage = (approval.OicIsApprove ?? false) ? 3 : approval.CurrentStage;

            return await _approvalProccessRepository.UpdateAsync(approval);
        }

        public async Task<ApprovalProccess?> UpdateByCSG(ApprovalProccess data)
        {
            var approval = await _approvalProccessRepository.GetByIdAsync(data?.Id ?? 0);
            if (approval == null) throw new Exception("Approval Process is not found.");

            approval.CsgRemarks = data.CsgRemarks;
            approval.CsgId = data.CsgId;
            approval.CsgIsApprove = data.CsgIsApprove;
            approval.CurrentStage = (approval.CsgIsApprove ?? false) ? 4 : approval.CurrentStage;

            return await _approvalProccessRepository.UpdateAsync(approval);
        }
        public async Task<ApprovalProccess?> UpdateByCO(ApprovalProccess data,int personnelActivityId)
        {
            var approval = await _approvalProccessRepository.GetByIdAsync(data?.Id ?? 0);
            if (approval == null) throw new Exception("Approval Process is not found.");

            var activity = await _personnelActivityRepository.GetByIdAsync(personnelActivityId);
            if (activity == null) throw new Exception("No Activity found.");

            approval.CoRemarks = data.CoRemarks;
            approval.CoId = data.CoId;
            approval.CoIsApprove = data.CoIsApprove;

            activity.IsFullyApproved = (approval.CoIsApprove ?? false);
            await _personnelActivityRepository.UpdateAsync(activity);

            return await _approvalProccessRepository.UpdateAsync(approval);
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

        public async Task<ApprovalProccess?> UpdateAsync(ApprovalProccess data)
        {
            return await _approvalProccessRepository.UpdateAsync(data);
        }
    }
}