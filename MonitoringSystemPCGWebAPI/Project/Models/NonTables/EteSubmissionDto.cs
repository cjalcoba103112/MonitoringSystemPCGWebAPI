namespace MonitoringSystemPCGWebAPI.Project.Models.NonTables
{
    public class EteSubmissionDto
    {
        public string? Explanation { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
    }
}
