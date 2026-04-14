
using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Classes
{
    public class PersonnelPromotionService : IPersonnelPromotionService
    {
        private readonly IPersonnelPromotionRepository _personnelPromotionRepository;

        public PersonnelPromotionService(IPersonnelPromotionRepository personnelPromotionRepository)
        {
            _personnelPromotionRepository = personnelPromotionRepository;
        }

        public async Task<PersonnelPromotion?> InsertAsync(PersonnelPromotion data)
        {
            return await _personnelPromotionRepository.InsertAsync(data);
        }

        public async Task<PersonnelPromotion?> UpdateAsync(PersonnelPromotion data)
        {
            return await _personnelPromotionRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<PersonnelPromotion>> GetAllAsync(PersonnelPromotion? filter)
        {
            return await _personnelPromotionRepository.GetAllAsync(filter);
        }

        public async Task<PersonnelPromotion?> GetByIdAsync(int id)
        {
            return await _personnelPromotionRepository.GetByIdAsync(id);
        }

        public async Task<PersonnelPromotion?> DeleteByIdAsync(int id)
        {
            return await _personnelPromotionRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<PersonnelPromotion>> BulkInsertAsync(List<PersonnelPromotion> data)
        {
            return await _personnelPromotionRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<PersonnelPromotion>> BulkUpdateAsync(List<PersonnelPromotion> data)
        {
            return await _personnelPromotionRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<PersonnelPromotion>> BulkUpsertAsync(List<PersonnelPromotion> data)
        {
            return await _personnelPromotionRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<PersonnelPromotion>> BulkMergeAsync(List<PersonnelPromotion> data)
        {
            return await _personnelPromotionRepository.BulkMergeAsync(data);
        }
    }
}