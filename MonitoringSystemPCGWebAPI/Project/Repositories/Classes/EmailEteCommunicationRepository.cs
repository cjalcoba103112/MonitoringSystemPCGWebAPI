
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;
using RTools_NTS.Util;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repositories.Classes
{
    public class EmailEteCommunicationRepository : GenericRepository<EmailEteCommunication>, IEmailEteCommunicationRepository
    {
        public async Task<EmailEteCommunication?> GetByPersonnelId(int id, DateTime? nextETE)
        {
            var query = _context.EmailEteCommunication
                .Include(c => c.Personnel)
                    .ThenInclude(p => p.Rank)
                .AsQueryable();

            query = query.Where(c => c.PersonnelId == id);

            if (nextETE.HasValue)
            {
                query = query.Where(c => c.NextEte.HasValue && c.NextEte.Value.Date == nextETE.Value.Date);
            }
           
            return await query.OrderByDescending(c=>c.Id).FirstOrDefaultAsync();
        }

        public async Task<EmailEteCommunication?> GetByToken(string token)
        {
            var data = await _context.EmailEteCommunication
                .Include(c => c.Personnel)
                    .ThenInclude(p => p.Rank)
                .OrderByDescending(c => c.Id)
                .FirstOrDefaultAsync(c => c.CommunicationToken == token);

            if (data.ExpiryDateTime < DateTime.UtcNow)
            {
                throw new Exception("EXPIRED");
            }

            return data;
        }
    }
}
