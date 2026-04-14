
using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Classes
{
    public class RankCategoryService : IRankCategoryService
    {
        private readonly IRankCategoryRepository _rankCategoryRepository;

        public RankCategoryService(IRankCategoryRepository rankCategoryRepository)
        {
            _rankCategoryRepository = rankCategoryRepository;
        }

        public async Task<RankCategory?> InsertAsync(RankCategory data)
        {
            return await _rankCategoryRepository.InsertAsync(data);
        }

        public async Task<RankCategory?> UpdateAsync(RankCategory data)
        {
            return await _rankCategoryRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<RankCategory>> GetAllAsync(RankCategory? filter)
        {
            return await _rankCategoryRepository.GetAllAsync(filter);
        }

        public async Task<RankCategory?> GetByIdAsync(int id)
        {
            return await _rankCategoryRepository.GetByIdAsync(id);
        }

        public async Task<RankCategory?> DeleteByIdAsync(int id)
        {
            return await _rankCategoryRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<RankCategory>> BulkInsertAsync(List<RankCategory> data)
        {
            return await _rankCategoryRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<RankCategory>> BulkUpdateAsync(List<RankCategory> data)
        {
            return await _rankCategoryRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<RankCategory>> BulkUpsertAsync(List<RankCategory> data)
        {
            return await _rankCategoryRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<RankCategory>> BulkMergeAsync(List<RankCategory> data)
        {
            return await _rankCategoryRepository.BulkMergeAsync(data);
        }
    }
}