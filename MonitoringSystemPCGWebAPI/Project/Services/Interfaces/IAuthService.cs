using Models;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;

namespace MonitoringSystemPCGWebAPI.Project.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Usertbl?> Signup(Signup data);
        Task<Usertbl?> Login(Login data);
    }
}
