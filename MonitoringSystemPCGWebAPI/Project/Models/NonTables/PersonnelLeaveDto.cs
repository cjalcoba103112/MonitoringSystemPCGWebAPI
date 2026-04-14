namespace MonitoringSystemPCGWebAPI.Project.Models.NonTables
{
    public class PersonnelLeaveDto
    {
        public int? ActivityTypeId { get; set; }
        public string? ActivityTypeName { get; set; }
        public decimal MaxCredits { get; set; }
        public decimal UsedCredits { get; set; }
        public decimal RemainingCredits { get; set; }
        public decimal ResetMonths { get; set; }
    }
}
