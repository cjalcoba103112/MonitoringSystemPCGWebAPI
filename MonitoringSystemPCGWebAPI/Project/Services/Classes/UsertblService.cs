
using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Classes
{
    public class UsertblService : IUsertblService
    {
        private readonly IUsertblRepository _usertblRepository;

        public UsertblService(IUsertblRepository usertblRepository)
        {
            _usertblRepository = usertblRepository;
        }

        public async Task<Usertbl?> InsertAsync(Usertbl data)
        {
            return await _usertblRepository.InsertAsync(data);
        }

        public async Task<Usertbl?> UpdateAsync(Usertbl data)
        {
            return await _usertblRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<Usertbl>> GetAllAsync(Usertbl? filter)
        {
            return await _usertblRepository.GetAllAsync(filter);
        }

        public async Task<Usertbl?> GetByIdAsync(int id)
        {
            return await _usertblRepository.GetByIdAsync(id);
        }

        public async Task<Usertbl?> DeleteByIdAsync(int id)
        {
            return await _usertblRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<Usertbl>> BulkInsertAsync(List<Usertbl> data)
        {
            return await _usertblRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<Usertbl>> BulkUpdateAsync(List<Usertbl> data)
        {
            return await _usertblRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<Usertbl>> BulkUpsertAsync(List<Usertbl> data)
        {
            return await _usertblRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<Usertbl>> BulkMergeAsync(List<Usertbl> data)
        {
            return await _usertblRepository.BulkMergeAsync(data);
        }
    }
}