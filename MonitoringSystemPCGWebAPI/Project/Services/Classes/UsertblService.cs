
using Models;
using Repositories.Interfaces;
using Services.Interfaces;
using Utilities.Interfaces;

namespace Services.Classes
{
    public class UsertblService : IUsertblService
    {
        private readonly IUsertblRepository _usertblRepository;
        private readonly IEncryptUtility _encryptUtility;
        public UsertblService(IUsertblRepository usertblRepository, IEncryptUtility encryptUtility)
        {
            _usertblRepository = usertblRepository;
            _encryptUtility = encryptUtility;
        }

        public async Task<Usertbl?> InsertAsync(Usertbl data)
        {
            string salt =  _encryptUtility.GenerateRandomSalt();
            string hashedPassword = _encryptUtility.GenerateHashedPassword(data?.HashedPassword??"", salt);

            data.Salt = salt;
            data.HashedPassword = hashedPassword;
            return await _usertblRepository.InsertAsync(data);
        }

        public async Task<Usertbl?> UpdateAsync(Usertbl data)
        {
            var user = await _usertblRepository.GetByIdAsync(data.UserId??0);
            if (user == null) throw new Exception("Invalid User");

            string hashedPassword = _encryptUtility.GenerateHashedPassword(data?.HashedPassword ?? "", user.Salt);

            data.HashedPassword = hashedPassword;
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