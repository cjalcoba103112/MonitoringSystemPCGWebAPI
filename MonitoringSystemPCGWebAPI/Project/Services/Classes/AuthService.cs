using Models;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;
using MonitoringSystemPCGWebAPI.Project.Services.Interfaces;
using Repositories.Classes;
using Repositories.Interfaces;
using Utilities.Interfaces;

namespace MonitoringSystemPCGWebAPI.Project.Services.Classes
{
    public class AuthService : IAuthService
    {
        private readonly IUsertblRepository _usertblRepository;
        private readonly IEncryptUtility _encryptUtility;
        private readonly IJwtUtility _jwtUtility;
        public AuthService(IUsertblRepository usertblRepository, IEncryptUtility encryptUtility, IJwtUtility jwtUtility)
        {
            _usertblRepository = usertblRepository;
            _encryptUtility = encryptUtility;
            _jwtUtility = jwtUtility;
        }

        public async Task<Usertbl?> Login(Login data)
        {
            var user = await _usertblRepository.GetFirstOrDefaultAsync(new Usertbl
            {
                UserName = data.UserName,
            });

            if (user == null) throw new Exception("Invalid Username or password");

            var encryptedPassword = _encryptUtility.GenerateHashedPassword(data.Password, user?.Salt);
            if (encryptedPassword != user.HashedPassword) throw new Exception("Invalid Username or password");
            return user;
        }

        public async Task<Usertbl?> Signup(Signup data)
        {

            var duplicateUser = await _usertblRepository.GetFirstOrDefaultAsync(new Usertbl { UserName = data.UserName });
            if (duplicateUser != null) throw new Exception($"Duplicate user {duplicateUser.UserName}");

            var randomSalt = _encryptUtility.GenerateRandomSalt();
            var hashedPassword = _encryptUtility.GenerateHashedPassword(data.Password, randomSalt);

            return await _usertblRepository.InsertAsync(
                new Usertbl
                {
                    FullName = data.FullName,
                    UserName = data.UserName,
                    Salt = randomSalt,
                    HashedPassword = hashedPassword,
                });
        }

    }
}
