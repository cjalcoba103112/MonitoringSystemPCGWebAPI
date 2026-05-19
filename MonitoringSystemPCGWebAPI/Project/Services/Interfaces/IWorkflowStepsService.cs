
using Models;

namespace Services.Interfaces
{
    public interface IWorkflowStepsService
    {
        Task<WorkflowSteps?> GetByRoleId(int? roleId);
        Task<WorkflowSteps?> InsertAsync(WorkflowSteps data);
        Task<WorkflowSteps?> UpdateAsync(WorkflowSteps data);
        Task<IEnumerable<WorkflowSteps>> GetAllAsync(WorkflowSteps? filter);
        Task<WorkflowSteps?> GetByIdAsync(int id);
        Task<WorkflowSteps?> DeleteByIdAsync(int id);
        Task<IEnumerable<WorkflowSteps>> BulkInsertAsync(List<WorkflowSteps> data);
        Task<IEnumerable<WorkflowSteps>> BulkUpdateAsync(List<WorkflowSteps> data);
        Task<IEnumerable<WorkflowSteps>> BulkUpsertAsync(List<WorkflowSteps> data);
        Task<IEnumerable<WorkflowSteps>> BulkMergeAsync(List<WorkflowSteps> data);
    }
}
