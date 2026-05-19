using Models;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;

namespace MonitoringSystemPCGWebAPI.Project.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Usertbl?> GetUserByChangePasswordToken(string token);
        Task<Usertbl?> Signup(Signup data);
        Task<Usertbl?> Login(Login data);
        Task<Usertbl?> ChangePassword(ChangePassword data);
    }
}
