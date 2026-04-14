
using Models;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;

namespace Services.Interfaces
{
    public interface IOtpVerificationsService
    {
        Task<OtpVerifications?> InsertAsync(OtpDTO data);
        Task<OtpVerifications?> UpdateAsync(OtpVerifications data);
        Task<IEnumerable<OtpVerifications>> GetAllAsync(OtpVerifications? filter);
        Task<OtpVerifications?> GetByIdAsync(int id);
        Task<OtpVerifications?> DeleteByIdAsync(int id);
        Task<IEnumerable<OtpVerifications>> BulkInsertAsync(List<OtpVerifications> data);
        Task<IEnumerable<OtpVerifications>> BulkUpdateAsync(List<OtpVerifications> data);
        Task<IEnumerable<OtpVerifications>> BulkUpsertAsync(List<OtpVerifications> data);
        Task<IEnumerable<OtpVerifications>> BulkMergeAsync(List<OtpVerifications> data);
    }
}
