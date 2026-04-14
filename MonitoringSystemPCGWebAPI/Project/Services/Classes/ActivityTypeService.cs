
using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Classes
{
    public class ActivityTypeService : IActivityTypeService
    {
        private readonly IActivityTypeRepository _activityTypeRepository;

        public ActivityTypeService(IActivityTypeRepository activityTypeRepository)
        {
            _activityTypeRepository = activityTypeRepository;
        }

        public async Task<ActivityType?> InsertAsync(ActivityType data)
        {
           
            return await _activityTypeRepository.InsertAsync(data);
        }

        public async Task<ActivityType?> UpdateAsync(ActivityType data)
        {
            return await _activityTypeRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<ActivityType>> GetAllAsync(ActivityType? filter)
        {
            return await _activityTypeRepository.GetAllAsync(filter);
        }

        public async Task<ActivityType?> GetByIdAsync(int id)
        {
            return await _activityTypeRepository.GetByIdAsync(id);
        }

        public async Task<ActivityType?> DeleteByIdAsync(int id)
        {
            return await _activityTypeRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<ActivityType>> BulkInsertAsync(List<ActivityType> data)
        {
            return await _activityTypeRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<ActivityType>> BulkUpdateAsync(List<ActivityType> data)
        {
            return await _activityTypeRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<ActivityType>> BulkUpsertAsync(List<ActivityType> data)
        {
            return await _activityTypeRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<ActivityType>> BulkMergeAsync(List<ActivityType> data)
        {
            return await _activityTypeRepository.BulkMergeAsync(data);
        }
    }
}