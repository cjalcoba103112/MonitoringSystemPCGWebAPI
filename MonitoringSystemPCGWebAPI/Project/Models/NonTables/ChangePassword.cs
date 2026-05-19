namespace MonitoringSystemPCGWebAPI.Project.Models.NonTables
{
    public class ChangePassword
    {
        public string? UsernameOrEmail { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
