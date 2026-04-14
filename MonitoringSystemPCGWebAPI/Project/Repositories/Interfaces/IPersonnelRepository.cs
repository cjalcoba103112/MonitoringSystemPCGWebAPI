
using Models;
using Models.NonTables;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;
using System.Linq.Expressions;

namespace Repositories.Interfaces
{
    public interface IPersonnelRepository : IGenericRepository<Personnel>
    {
        Task<Personnel?> GetByIdAsync(int id);
        Task<IEnumerable<PersonnelLeaveDto>> GetPersonnelCreditsAsync(
      int personnelId,
      int? activityTypeId = null,
      int? year = null,
      DateTime? date = null);
        Task<IEnumerable<Personnel>> GetAllAsync(Personnel? filter = null);
        //Task<IEnumerable<PersonnelDto>> GetAllAsync(Personnel? filter = null);

        Task<IEnumerable<Personnel>> GetNoActivities(DateTime? startDate = null,DateTime? endDate=null);
        Task<IEnumerable<EnlistedPersonnelETE>> GetEnlismentETE(Personnel? filter = null);
    }
}
