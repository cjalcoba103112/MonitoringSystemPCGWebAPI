
using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Classes
{
    public class LeaveTypesService : ILeaveTypesService
    {
        private readonly ILeaveTypesRepository _leaveTypesRepository;

        public LeaveTypesService(ILeaveTypesRepository leaveTypesRepository)
        {
            _leaveTypesRepository = leaveTypesRepository;
        }

        public async Task<LeaveTypes?> InsertAsync(LeaveTypes data)
        {
            return await _leaveTypesRepository.InsertAsync(data);
        }

        public async Task<LeaveTypes?> UpdateAsync(LeaveTypes data)
        {
            return await _leaveTypesRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<LeaveTypes>> GetAllAsync(LeaveTypes? filter)
        {
            return await _leaveTypesRepository.GetAllAsync(filter);
        }

        public async Task<LeaveTypes?> GetByIdAsync(int id)
        {
            return await _leaveTypesRepository.GetByIdAsync(id);
        }

        public async Task<LeaveTypes?> DeleteByIdAsync(int id)
        {
            return await _leaveTypesRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<LeaveTypes>> BulkInsertAsync(List<LeaveTypes> data)
        {
            return await _leaveTypesRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<LeaveTypes>> BulkUpdateAsync(List<LeaveTypes> data)
        {
            return await _leaveTypesRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<LeaveTypes>> BulkUpsertAsync(List<LeaveTypes> data)
        {
            return await _leaveTypesRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<LeaveTypes>> BulkMergeAsync(List<LeaveTypes> data)
        {
            return await _leaveTypesRepository.BulkMergeAsync(data);
        }
    }
}