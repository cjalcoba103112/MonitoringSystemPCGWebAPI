
using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Classes
{
    public class EnlistmentRecordService : IEnlistmentRecordService
    {
        private readonly IEnlistmentRecordRepository _enlistmentRecordRepository;

        public EnlistmentRecordService(IEnlistmentRecordRepository enlistmentRecordRepository)
        {
            _enlistmentRecordRepository = enlistmentRecordRepository;
        }

        public async Task<EnlistmentRecord?> InsertAsync(EnlistmentRecord data)
        {
            return await _enlistmentRecordRepository.InsertAsync(data);
        }

        public async Task<EnlistmentRecord?> UpdateAsync(EnlistmentRecord data)
        {
            return await _enlistmentRecordRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<EnlistmentRecord>> GetAllAsync(EnlistmentRecord? filter)
        {
            return await _enlistmentRecordRepository.GetAllAsync(filter);
        }

        public async Task<EnlistmentRecord?> GetByIdAsync(int id)
        {
            return await _enlistmentRecordRepository.GetByIdAsync(id);
        }

        public async Task<EnlistmentRecord?> DeleteByIdAsync(int id)
        {
            return await _enlistmentRecordRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<EnlistmentRecord>> BulkInsertAsync(List<EnlistmentRecord> data)
        {
            return await _enlistmentRecordRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<EnlistmentRecord>> BulkUpdateAsync(List<EnlistmentRecord> data)
        {
            return await _enlistmentRecordRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<EnlistmentRecord>> BulkUpsertAsync(List<EnlistmentRecord> data)
        {
            return await _enlistmentRecordRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<EnlistmentRecord>> BulkMergeAsync(List<EnlistmentRecord> data)
        {
            return await _enlistmentRecordRepository.BulkMergeAsync(data);
        }
    }
}