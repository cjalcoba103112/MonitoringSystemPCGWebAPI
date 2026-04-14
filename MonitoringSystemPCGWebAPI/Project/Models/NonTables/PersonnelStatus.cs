using Models;

namespace MonitoringSystemPCGWebAPI.Project.Models.NonTables
{
    public class PersonnelStatus : Personnel
    {
        public IList<ActivityType> Activity { get; set; } = new List<ActivityType>();

    }
}
