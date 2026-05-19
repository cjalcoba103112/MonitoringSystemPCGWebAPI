
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

        public async Task<IEnumerable<SidebarRoleMapping>> SyncRolePermissionsAsync(int roleId, List<int> sidebarIds)
        {
            // 1. Get all current mappings for this role
            var existingMappings = await _sidebarRoleMappingRepository.GetAllAsync(new SidebarRoleMapping { RoleId = roleId });

            // 2. Delete existing mappings
            if (existingMappings != null && existingMappings.Any())
            {
                foreach (var mapping in existingMappings)
                {
                    // Assuming your repository has DeleteByIdAsync or similar
                    await _sidebarRoleMappingRepository.DeleteByIdAsync(mapping.SidebarRoleMappingId ?? 0);
                }
            }

            // 3. Prepare new mappings
            var newMappings = sidebarIds.Select(sId => new SidebarRoleMapping
            {
                RoleId = roleId,
                SidebarId = sId
            }).ToList();
            foreach(var item in newMappings)
            {
                await _sidebarRoleMappingRepository.InsertAsync(item);
            }
            // 4. Bulk Insert the new list
            return newMappings;
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