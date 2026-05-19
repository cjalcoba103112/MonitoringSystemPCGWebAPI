
using ApplicationContexts;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Classes
{
    public class WorkflowStepsService : IWorkflowStepsService
    {
        private readonly IWorkflowStepsRepository _workflowStepsRepository;
        private readonly ApplicationContext _context = new ApplicationContext();
        public WorkflowStepsService(IWorkflowStepsRepository workflowStepsRepository)
        {
            _workflowStepsRepository = workflowStepsRepository;
        }

        public async Task<WorkflowSteps?> InsertAsync(WorkflowSteps data)
        {
            return await _workflowStepsRepository.InsertAsync(data);
        }

        public async Task<WorkflowSteps?> UpdateAsync(WorkflowSteps data)
        {
            return await _workflowStepsRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<WorkflowSteps>> GetAllAsync(WorkflowSteps? filter)
        {
            return await _context.WorkflowSteps
                   .Include(w => w.RankCategory)
                   .Include(w=>w.Role)
                   .ToListAsync();
        }
        public async Task<WorkflowSteps?> GetByRoleId(int? roleId)
        {
            return await _context.WorkflowSteps
                   .Include(w => w.RankCategory)
                   .Include(w => w.Role)
                   .FirstOrDefaultAsync(w=>w.RoleId == roleId);
        }


        public async Task<WorkflowSteps?> GetByIdAsync(int id)
        {
            return await _workflowStepsRepository.GetByIdAsync(id);
        }

        public async Task<WorkflowSteps?> DeleteByIdAsync(int id)
        {
            return await _workflowStepsRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<WorkflowSteps>> BulkInsertAsync(List<WorkflowSteps> data)
        {
            return await _workflowStepsRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<WorkflowSteps>> BulkUpdateAsync(List<WorkflowSteps> data)
        {
            return await _workflowStepsRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<WorkflowSteps>> BulkUpsertAsync(List<WorkflowSteps> data)
        {
            return await _workflowStepsRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<WorkflowSteps>> BulkMergeAsync(List<WorkflowSteps> data)
        {
            return await _workflowStepsRepository.BulkMergeAsync(data);
        }
    }
}