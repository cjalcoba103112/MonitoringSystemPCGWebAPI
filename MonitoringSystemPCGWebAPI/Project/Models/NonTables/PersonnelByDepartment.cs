using Models;

namespace MonitoringSystemPCGWebAPI.Project.Models.NonTables
{
    public class PersonnelByDepartment
    {
        public string Department{  get; set; }
        public int Personnel {  get; set; }
        public List<Personnel> names { get; set; } = new List<Personnel>();
    }
}
