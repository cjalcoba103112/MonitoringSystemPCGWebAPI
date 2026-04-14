
using Models;

namespace Repositories.Interfaces
{
    public interface IOtpVerificationsRepository : IGenericRepository<OtpVerifications>
    {
        Task<int> GetAttemptsCountAsync(string email, DateTime since);
    }
}
