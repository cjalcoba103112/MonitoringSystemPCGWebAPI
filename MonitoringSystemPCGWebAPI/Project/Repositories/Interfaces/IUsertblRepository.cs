
using Models;
using System.Linq.Expressions;

namespace Repositories.Interfaces
{
    public interface IUsertblRepository : IGenericRepository<Usertbl>
    {
        Usertbl? GetByChangePasswordToken(string changePasswordToken);
        Usertbl? GetUsernameOrEmail(string usernameOrEmail);
        Task<IEnumerable<Usertbl>> GetAllAsync(Usertbl? filter);
        Task<Usertbl?> GetByIdAsync(int id);
    }
}
