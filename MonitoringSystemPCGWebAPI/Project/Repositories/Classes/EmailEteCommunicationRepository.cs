
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;
using RTools_NTS.Util;

namespace Repositories.Classes
{
    public class EmailEteCommunicationRepository : GenericRepository<EmailEteCommunication>, IEmailEteCommunicationRepository
    {
        public async Task<EmailEteCommunication?> GetByPersonnelId(int id)
        {
            var data = await _context.EmailEteCommunication
               .Include(c => c.Personnel)
                   .ThenInclude(p => p.Rank)
               .FirstOrDefaultAsync(c => c.PersonnelId == id);

            if (data == null) return null;

            return data;
        }

        public async Task<EmailEteCommunication?> GetByToken(string token)
        {
            var data = await _context.EmailEteCommunication
                .Include(c => c.Personnel)
                    .ThenInclude(p => p.Rank)
                .FirstOrDefaultAsync(c => c.CommunicationToken == token);

            if (data == null) return null; 

            if (data.ExpiryDateTime < DateTime.UtcNow)
            {
                throw new Exception("EXPIRED");
            }

            return data;
        }
    }
}
