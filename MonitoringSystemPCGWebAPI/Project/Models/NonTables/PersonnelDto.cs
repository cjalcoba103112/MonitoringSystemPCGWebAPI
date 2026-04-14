namespace MonitoringSystemPCGWebAPI.Project.Models.NonTables
{
    public class PersonnelDto
    {
        public int? PersonnelId { get; set; }
        public string? FullName { get; set; }

        public List<PersonnelLeaveDto> Leaves { get; set; } = new();
    }
}
