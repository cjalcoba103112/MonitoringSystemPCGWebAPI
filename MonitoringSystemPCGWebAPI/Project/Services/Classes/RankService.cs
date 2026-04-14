
using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Classes
{
    public class RankService : IRankService
    {
        private readonly IRankRepository _rankRepository;

        public RankService(IRankRepository rankRepository)
        {
            _rankRepository = rankRepository;
        }

        public async Task<Rank?> InsertAsync(Rank data)
        {
            return await _rankRepository.InsertAsync(data);
        }

        public async Task<Rank?> UpdateAsync(Rank data)
        {
            return await _rankRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<Rank>> GetAllAsync(Rank? filter)
        {
            IEnumerable<Rank> ranks = await _rankRepository.GetAllAsync(filter,x=>x.RankCategory);
            return ranks.OrderBy(r => r.RankLevel);
        }

        public async Task<Rank?> GetByIdAsync(int id)
        {
            return await _rankRepository.GetByIdAsync(id);
        }

        public async Task<Rank?> DeleteByIdAsync(int id)
        {
            return await _rankRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<Rank>> BulkInsertAsync(List<Rank> data)
        {
            return await _rankRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<Rank>> BulkUpdateAsync(List<Rank> data)
        {
            return await _rankRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<Rank>> BulkUpsertAsync(List<Rank> data)
        {
            return await _rankRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<Rank>> BulkMergeAsync(List<Rank> data)
        {
            return await _rankRepository.BulkMergeAsync(data);
        }
    }
}