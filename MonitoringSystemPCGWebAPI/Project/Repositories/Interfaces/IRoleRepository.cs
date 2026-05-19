
using Models;

namespace Repositories.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<IEnumerable<Role>> GetAllAsync(Role? filter);
    }
}
