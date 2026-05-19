
using ApplicationContexts;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Classes
{
    public class ActivityAppealService : IActivityAppealService
    {
        private readonly IActivityAppealRepository _activityAppealRepository;
        private readonly IPersonnelActivityRepository _personnelActivityRepository;
        private readonly ApplicationContext _context = new ApplicationContext();

        public ActivityAppealService(IActivityAppealRepository activityAppealRepository, IPersonnelActivityRepository personnelActivityRepository)
        {
            _activityAppealRepository = activityAppealRepository;
            _personnelActivityRepository = personnelActivityRepository;
        }

        public async Task<ActivityAppeal?> InsertAsync(ActivityAppeal data)
        {
            return await _activityAppealRepository.InsertAsync(data);
        }

        public async Task<ActivityAppeal?> SubmitAsync(ActivityAppeal data)
        {
            var personnelActivity = await _personnelActivityRepository.GetByIdAsync(data.PersonnelActivityId ?? 0);
            if(personnelActivity == null) throw new Exception("Personnel Activity not found");

            personnelActivity.Status = "Appeal";
            data.IsUsed = true;
            await _personnelActivityRepository.UpdateAsync(personnelActivity);
            return await _activityAppealRepository.UpdateAsync(data);
        }

        //public async Task<IEnumerable<ActivityAppeal>> GetAllAsync(ActivityAppeal? filter)h
        //{
        //    return await _activityAppealRepository.GetAllAsync(filter);
        //}

        public async Task<ActivityAppeal?> GetByIdAsync(int id)
        {
            return await _activityAppealRepository.GetByIdAsync(id);
        }

        public async Task<ActivityAppeal?> DeleteByIdAsync(int id)
        {
            return await _activityAppealRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<ActivityAppeal>> BulkInsertAsync(List<ActivityAppeal> data)
        {
            return await _activityAppealRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<ActivityAppeal>> BulkUpdateAsync(List<ActivityAppeal> data)
        {
            return await _activityAppealRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<ActivityAppeal>> BulkUpsertAsync(List<ActivityAppeal> data)
        {
            return await _activityAppealRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<ActivityAppeal>> BulkMergeAsync(List<ActivityAppeal> data)
        {
            return await _activityAppealRepository.BulkMergeAsync(data);
        }

        public async Task<ActivityAppeal?> GetByTokenAsync(string token)
        {
            return await _context.ActivityAppeal
                .Include(a=>a.PersonnelActivity)
                    .ThenInclude(p=>p.ActivityType)
                .FirstOrDefaultAsync(a => a.AppealToken == token && a.IsUsed == false);
        }

        public async Task<IEnumerable<ActivityAppeal>> GetAllAsync(ActivityAppeal? filter)
        {
            return await _activityAppealRepository.GetAllAsync(filter);
        }
    }
}