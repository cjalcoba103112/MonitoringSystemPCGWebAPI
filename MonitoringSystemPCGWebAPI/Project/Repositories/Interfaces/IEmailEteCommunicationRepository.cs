
using Models;

namespace Repositories.Interfaces
{
    public interface IEmailEteCommunicationRepository : IGenericRepository<EmailEteCommunication>
    {
        Task<EmailEteCommunication?> GetByToken(string token);
        Task<EmailEteCommunication?> GetByPersonnelId(int id);
    }
}
