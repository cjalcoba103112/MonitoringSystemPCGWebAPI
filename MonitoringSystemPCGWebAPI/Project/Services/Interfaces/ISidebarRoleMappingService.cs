
using Models;

namespace Services.Interfaces
{
    public interface ISidebarRoleMappingService
    {
        Task<SidebarRoleMapping?> InsertAsync(SidebarRoleMapping data);
        Task<SidebarRoleMapping?> UpdateAsync(SidebarRoleMapping data);
        Task<IEnumerable<SidebarRoleMapping>> GetAllAsync(SidebarRoleMapping? filter);
        Task<SidebarRoleMapping?> GetByIdAsync(int id);
        Task<SidebarRoleMapping?> DeleteByIdAsync(int id);
        Task<IEnumerable<SidebarRoleMapping>> BulkInsertAsync(List<SidebarRoleMapping> data);
        Task<IEnumerable<SidebarRoleMapping>> BulkUpdateAsync(List<SidebarRoleMapping> data);
        Task<IEnumerable<SidebarRoleMapping>> BulkUpsertAsync(List<SidebarRoleMapping> data);
        Task<IEnumerable<SidebarRoleMapping>> BulkMergeAsync(List<SidebarRoleMapping> data);
    }
}
