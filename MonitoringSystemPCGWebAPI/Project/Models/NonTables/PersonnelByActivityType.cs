using Models;

namespace MonitoringSystemPCGWebAPI.Project.Models.NonTables
{
    public class PersonnelByActivityType
    {
        public string Activity {  get; set; }
        public int Personnel {  get; set; }
        public List<InfoData>? Info { get; set; } = new List<InfoData>();
    }

    public class InfoData
    {
        public Personnel? Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Title { get; set; }
    }
}
