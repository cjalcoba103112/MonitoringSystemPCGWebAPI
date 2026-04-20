
using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Classes
{
    public class PersonnelDutyLogsService : IPersonnelDutyLogsService
    {
        private readonly IPersonnelDutyLogsRepository _personnelDutyLogsRepository;

        public PersonnelDutyLogsService(IPersonnelDutyLogsRepository personnelDutyLogsRepository)
        {
            _personnelDutyLogsRepository = personnelDutyLogsRepository;
        }

        public async Task<PersonnelDutyLogs?> InsertAsync(PersonnelDutyLogs data)
        {
            return await _personnelDutyLogsRepository.InsertAsync(data);
        }

        public async Task<PersonnelDutyLogs?> UpdateAsync(PersonnelDutyLogs data)
        {
            return await _personnelDutyLogsRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<PersonnelDutyLogs>> GetAllAsync(PersonnelDutyLogs? filter)
        {
            return await _personnelDutyLogsRepository.GetAllAsync(filter);
        }

        public async Task<PersonnelDutyLogs?> GetByIdAsync(int id)
        {
            return await _personnelDutyLogsRepository.GetByIdAsync(id);
        }

        public async Task<PersonnelDutyLogs?> DeleteByIdAsync(int id)
        {
            return await _personnelDutyLogsRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<PersonnelDutyLogs>> BulkInsertAsync(List<PersonnelDutyLogs> data)
        {
            return await _personnelDutyLogsRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<PersonnelDutyLogs>> BulkUpdateAsync(List<PersonnelDutyLogs> data)
        {
            return await _personnelDutyLogsRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<PersonnelDutyLogs>> BulkUpsertAsync(List<PersonnelDutyLogs> data)
        {
            return await _personnelDutyLogsRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<PersonnelDutyLogs>> BulkMergeAsync(List<PersonnelDutyLogs> data)
        {
            return await _personnelDutyLogsRepository.BulkMergeAsync(data);
        }
    }
}