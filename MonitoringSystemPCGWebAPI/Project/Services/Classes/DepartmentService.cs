
using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Classes
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<Department?> InsertAsync(Department data)
        {
            return await _departmentRepository.InsertAsync(data);
        }

        public async Task<Department?> UpdateAsync(Department data)
        {
            return await _departmentRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<Department>> GetAllAsync(Department? filter)
        {
            return await _departmentRepository.GetAllAsync(filter);
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _departmentRepository.GetByIdAsync(id);
        }

        public async Task<Department?> DeleteByIdAsync(int id)
        {
            return await _departmentRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<Department>> BulkInsertAsync(List<Department> data)
        {
            return await _departmentRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<Department>> BulkUpdateAsync(List<Department> data)
        {
            return await _departmentRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<Department>> BulkUpsertAsync(List<Department> data)
        {
            return await _departmentRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<Department>> BulkMergeAsync(List<Department> data)
        {
            return await _departmentRepository.BulkMergeAsync(data);
        }
    }
}