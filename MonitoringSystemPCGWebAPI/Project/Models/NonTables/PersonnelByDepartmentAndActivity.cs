using Models;

namespace MonitoringSystemPCGWebAPI.Project.Models.NonTables
{
    public class PersonnelByDepartmentAndActivity
    {
        public string Department { get; set; }
        public string Activity { get; set; }
        public int Personnel { get; set; }
        public List<Personnel> Names { get; set; }
    }
}
