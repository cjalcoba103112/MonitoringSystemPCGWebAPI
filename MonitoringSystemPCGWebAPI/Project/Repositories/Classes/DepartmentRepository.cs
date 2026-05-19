
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;

namespace Repositories.Classes
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public async Task<IEnumerable<Department>> GetAllAsync(Department department)
        {
            return await  _context.Department.Include(d => d.Oic).ThenInclude(o => o.Rank).ToListAsync();
        }
    }
}
