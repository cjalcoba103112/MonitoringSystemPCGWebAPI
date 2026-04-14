
using System.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Repositories.Interfaces
{
    public interface IGenericRepository<T>
        where T : class
    {
        Task<T?> GetFirstOrDefaultAsync(T? filter = null, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllAsync(T? filter = null, params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(int id);
        Task<T?> InsertAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<T?> DeleteByIdAsync(int id);
        Task<IEnumerable<T>> BulkInsertAsync(List<T> data);
        Task<IEnumerable<T>> BulkUpdateAsync(List<T> data);
        Task<IEnumerable<T>> BulkUpsertAsync(List<T> data);
        Task<IEnumerable<T>> BulkMergeAsync(List<T> entities, Expression<Func<T, bool>>? deleteFilter = null);
    }
}
