using Models;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;

namespace MonitoringSystemPCGWebAPI.Project.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<IEnumerable<Personnel>> GetPersonnelOngoingActivities();
        Task<IEnumerable<PersonnelByActivityType>> GetPersonnelByActivityType(DateTime? startDate= null, DateTime? endDate = null);
        Task<IEnumerable<PersonnelByDepartment>> GetPersonnelByDepartment();
        Task<List<PersonnelByDepartmentAndActivity>> GetPersonnelByDepartmentAndActivity(DateTime? startDate = null, DateTime? endDate = null);
    }
}
