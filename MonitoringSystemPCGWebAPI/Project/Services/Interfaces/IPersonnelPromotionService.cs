
using Models;

namespace Services.Interfaces
{
    public interface IPersonnelPromotionService
    {
        Task<PersonnelPromotion?> InsertAsync(PersonnelPromotion data);
        Task<PersonnelPromotion?> UpdateAsync(PersonnelPromotion data);
        Task<IEnumerable<PersonnelPromotion>> GetAllAsync(PersonnelPromotion? filter);
        Task<PersonnelPromotion?> GetByIdAsync(int id);
        Task<PersonnelPromotion?> DeleteByIdAsync(int id);
        Task<IEnumerable<PersonnelPromotion>> BulkInsertAsync(List<PersonnelPromotion> data);
        Task<IEnumerable<PersonnelPromotion>> BulkUpdateAsync(List<PersonnelPromotion> data);
        Task<IEnumerable<PersonnelPromotion>> BulkUpsertAsync(List<PersonnelPromotion> data);
        Task<IEnumerable<PersonnelPromotion>> BulkMergeAsync(List<PersonnelPromotion> data);
    }
}
