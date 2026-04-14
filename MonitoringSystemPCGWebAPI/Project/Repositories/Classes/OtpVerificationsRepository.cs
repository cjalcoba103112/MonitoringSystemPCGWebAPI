
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;

namespace Repositories.Classes
{
    public class OtpVerificationsRepository : GenericRepository<OtpVerifications>, IOtpVerificationsRepository
    {
        public async Task<int> GetAttemptsCountAsync(string email, DateTime since)
        {
            return await _context.OtpVerifications
                .AsNoTracking() // Recommended for read-only count operations
                .CountAsync(x => x.Email == email &&
                                 x.CreatedAt >= since &&
                                 x.IsUsed == false);
        }
    }
}
