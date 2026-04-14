
using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Classes
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role?> InsertAsync(Role data)
        {
            return await _roleRepository.InsertAsync(data);
        }

        public async Task<Role?> UpdateAsync(Role data)
        {
            return await _roleRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<Role>> GetAllAsync(Role? filter)
        {
            return await _roleRepository.GetAllAsync(filter);
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        public async Task<Role?> DeleteByIdAsync(int id)
        {
            return await _roleRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<Role>> BulkInsertAsync(List<Role> data)
        {
            return await _roleRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<Role>> BulkUpdateAsync(List<Role> data)
        {
            return await _roleRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<Role>> BulkUpsertAsync(List<Role> data)
        {
            return await _roleRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<Role>> BulkMergeAsync(List<Role> data)
        {
            return await _roleRepository.BulkMergeAsync(data);
        }
    }
}