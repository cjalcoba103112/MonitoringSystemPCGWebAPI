
using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Classes
{
    public class SidebarService : ISidebarService
    {
        private readonly ISidebarRepository _sidebarRepository;

        public SidebarService(ISidebarRepository sidebarRepository)
        {
            _sidebarRepository = sidebarRepository;
        }

        public async Task<Sidebar?> InsertAsync(Sidebar data)
        {
            return await _sidebarRepository.InsertAsync(data);
        }

        public async Task<Sidebar?> UpdateAsync(Sidebar data)
        {
            return await _sidebarRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<Sidebar>> GetAllAsync(Sidebar? filter)
        {
            return await _sidebarRepository.GetAllAsync(filter);
        }

        public async Task<Sidebar?> GetByIdAsync(int id)
        {
            return await _sidebarRepository.GetByIdAsync(id);
        }

        public async Task<Sidebar?> DeleteByIdAsync(int id)
        {
            return await _sidebarRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<Sidebar>> BulkInsertAsync(List<Sidebar> data)
        {
            return await _sidebarRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<Sidebar>> BulkUpdateAsync(List<Sidebar> data)
        {
            return await _sidebarRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<Sidebar>> BulkUpsertAsync(List<Sidebar> data)
        {
            return await _sidebarRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<Sidebar>> BulkMergeAsync(List<Sidebar> data)
        {
            return await _sidebarRepository.BulkMergeAsync(data);
        }
    }
}