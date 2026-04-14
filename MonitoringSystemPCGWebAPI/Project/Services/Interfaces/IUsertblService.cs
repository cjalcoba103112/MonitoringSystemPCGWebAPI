
using Models;

namespace Services.Interfaces
{
    public interface IUsertblService
    {
        Task<Usertbl?> InsertAsync(Usertbl data);
        Task<Usertbl?> UpdateAsync(Usertbl data);
        Task<IEnumerable<Usertbl>> GetAllAsync(Usertbl? filter);
        Task<Usertbl?> GetByIdAsync(int id);
        Task<Usertbl?> DeleteByIdAsync(int id);
        Task<IEnumerable<Usertbl>> BulkInsertAsync(List<Usertbl> data);
        Task<IEnumerable<Usertbl>> BulkUpdateAsync(List<Usertbl> data);
        Task<IEnumerable<Usertbl>> BulkUpsertAsync(List<Usertbl> data);
        Task<IEnumerable<Usertbl>> BulkMergeAsync(List<Usertbl> data);
    }
}
