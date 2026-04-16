
using Models;

namespace Services.Interfaces
{
    public interface IEmailEteCommunicationService
    {
        Task<EmailEteCommunication?> GetByPersonnelId(int id,DateTime? nextETE);
        Task<EmailEteCommunication?> GetByToken(string token);
        Task<EmailEteCommunication?> InsertAsync(EmailEteCommunication data);
        Task<EmailEteCommunication?> UpdateAsync(EmailEteCommunication data, IFormFile? supportingDocument);
        Task<IEnumerable<EmailEteCommunication>> GetAllAsync(EmailEteCommunication? filter);
        Task<EmailEteCommunication?> GetByIdAsync(int id);
        Task<EmailEteCommunication?> DeleteByIdAsync(int id);
        Task<IEnumerable<EmailEteCommunication>> BulkInsertAsync(List<EmailEteCommunication> data);
        Task<IEnumerable<EmailEteCommunication>> BulkUpdateAsync(List<EmailEteCommunication> data);
        Task<IEnumerable<EmailEteCommunication>> BulkUpsertAsync(List<EmailEteCommunication> data);
        Task<IEnumerable<EmailEteCommunication>> BulkMergeAsync(List<EmailEteCommunication> data);
    }
}
