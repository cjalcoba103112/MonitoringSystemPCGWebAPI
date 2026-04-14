
using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Classes
{
    public class SidebarRoleMappingService : ISidebarRoleMappingService
    {
        private readonly ISidebarRoleMappingRepository _sidebarRoleMappingRepository;

        public SidebarRoleMappingService(ISidebarRoleMappingRepository sidebarRoleMappingRepository)
        {
            _sidebarRoleMappingRepository = sidebarRoleMappingRepository;
        }

        public async Task<SidebarRoleMapping?> InsertAsync(SidebarRoleMapping data)
        {
            return await _sidebarRoleMappingRepository.InsertAsync(data);
        }

        public async Task<SidebarRoleMapping?> UpdateAsync(SidebarRoleMapping data)
        {
            return await _sidebarRoleMappingRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<SidebarRoleMapping>> GetAllAsync(SidebarRoleMapping? filter)
        {
            return await _sidebarRoleMappingRepository.GetAllAsync(filter);
        }

        public async Task<SidebarRoleMapping?> GetByIdAsync(int id)
        {
            return await _sidebarRoleMappingRepository.GetByIdAsync(id);
        }

        public async Task<SidebarRoleMapping?> DeleteByIdAsync(int id)
        {
            return await _sidebarRoleMappingRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<SidebarRoleMapping>> BulkInsertAsync(List<SidebarRoleMapping> data)
        {
            return await _sidebarRoleMappingRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<SidebarRoleMapping>> BulkUpdateAsync(List<SidebarRoleMapping> data)
        {
            return await _sidebarRoleMappingRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<SidebarRoleMapping>> BulkUpsertAsync(List<SidebarRoleMapping> data)
        {
            return await _sidebarRoleMappingRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<SidebarRoleMapping>> BulkMergeAsync(List<SidebarRoleMapping> data)
        {
            return await _sidebarRoleMappingRepository.BulkMergeAsync(data);
        }
    }
}