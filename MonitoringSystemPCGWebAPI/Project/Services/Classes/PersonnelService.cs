
using Models;
using Models.NonTables;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;
using Repositories.Interfaces;
using Services.Interfaces;
using Utilities.Classes;
using Utilities.Interfaces;

namespace Services.Classes
{
    public class PersonnelService : IPersonnelService
    {
        private readonly IPersonnelRepository _personnelRepository;
        private readonly FileUtility _fileUtility = new FileUtility("wwwroot/images/profiles");
        public PersonnelService(IPersonnelRepository personnelRepository)
        {
            _personnelRepository = personnelRepository;
        }
        public async Task<IEnumerable<PersonnelLeaveDto>> GetPersonnelCreditsAsync(int personnelId, int? activityTypeId=null,int?year=null,DateTime?date=null)
        {
            return await _personnelRepository.GetPersonnelCreditsAsync(personnelId,activityTypeId, year,date);
        }
       
        public async Task<IEnumerable<EnlistedPersonnelETE>> GetEnlismentETE(Personnel? filter = null)
        {
            return await _personnelRepository.GetEnlismentETE(filter);
        }
        public async Task<Personnel?> InsertAsync(Personnel data,IFormFile? profile)
        {
            if(profile != null)
            {
                string randomFileName = _fileUtility.GetRandomFileName(profile.FileName);
                await _fileUtility.CreateAsync(randomFileName, profile.OpenReadStream());
                data.Profile = randomFileName;
            }
           
            return await _personnelRepository.InsertAsync(data);
        }

        public async Task<Personnel?> UpdateAsync(Personnel data, IFormFile? profile)
        {

            var existing = await _personnelRepository.GetByIdAsync(data.PersonnelId ??0);
            if (existing == null) return null;


            if (profile != null && profile.Length > 0)
            {
                
                if (!string.IsNullOrEmpty(existing.Profile))
                {
                    _fileUtility.Delete(existing.Profile);
                }

              
                string randomFileName = _fileUtility.GetRandomFileName(profile.FileName);
                await _fileUtility.CreateAsync(randomFileName, profile.OpenReadStream());

                data.Profile = randomFileName;
            }
            else
            {
                    
                data.Profile = existing.Profile;
            }
            return await _personnelRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<Personnel>> GetAllAsync(Personnel? filter)
        {
            IEnumerable<Personnel> personnels = await _personnelRepository.GetAllAsync(filter);

            return personnels;
        }
        public async Task<IEnumerable<Personnel>> GetETE(Personnel? filter)
        {
            IEnumerable<Personnel> personnels = await _personnelRepository.GetAllAsync(filter);

            
            return personnels.OrderBy(x => x.Rank.RankLevel);
        }

        public async Task<Personnel?> GetByIdAsync(int id)
        {
            return await _personnelRepository.GetByIdAsync(id);
        }

        public async Task<Personnel?> DeleteByIdAsync(int id)
        {
            return await _personnelRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<Personnel>> BulkInsertAsync(List<Personnel> data)
        {
            return await _personnelRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<Personnel>> BulkUpdateAsync(List<Personnel> data)
        {
            return await _personnelRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<Personnel>> BulkUpsertAsync(List<Personnel> data)
        {
            return await _personnelRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<Personnel>> BulkMergeAsync(List<Personnel> data)
        {
            return await _personnelRepository.BulkMergeAsync(data);
        }
    }
}