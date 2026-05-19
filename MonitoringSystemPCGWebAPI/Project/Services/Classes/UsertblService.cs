
using Models;
using Repositories.Interfaces;
using Services.Interfaces;
using Utilities.Classes;
using Utilities.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Classes
{
    public class UsertblService : IUsertblService
    {
        private readonly IUsertblRepository _usertblRepository;
        private readonly IPersonnelRepository _personnelRepository;
        private readonly IEncryptUtility _encryptUtility;
        private readonly IEmailSenderUtility _emailSenderUtility;
        private readonly IConfigurationRoot _config = new AppUtility().GetConfiguration();
        public UsertblService(IUsertblRepository usertblRepository, IEncryptUtility encryptUtility, IEmailSenderUtility emailSenderUtility, IPersonnelRepository personnelRepository)
        {
            _usertblRepository = usertblRepository;
            _emailSenderUtility = emailSenderUtility;
            _encryptUtility = encryptUtility;
            _personnelRepository = personnelRepository;
        }

        public async Task<Usertbl?> InsertAsync(Usertbl data)
        {
            var personnel = await _personnelRepository.GetByIdAsync(data?.PersonnelId ?? 0);

            if (personnel == null) throw new Exception("Invalid Personnel");

            string defaultPassword = $"welcome_{personnel.SerialNumber}";
            string salt = _encryptUtility.GenerateRandomSalt();
            string hashedPassword = _encryptUtility.GenerateHashedPassword(defaultPassword, salt);
            string token = Guid.NewGuid().ToString();

            data.Salt = salt;
            data.HashedPassword = hashedPassword;
            data.ChangePasswordToken = token;
            data.IsDefaultPassword = true;
            data.IsActive = true;

            personnel.HasAccount = true;

            await SendAccountEmailBody(defaultPassword, token, personnel);
            await _personnelRepository.UpdateAsync(personnel);

            var insertedUser = await _usertblRepository.InsertAsync(data);


            return insertedUser;
        }

        public async Task SendAccountEmailBody(string defaultPassword, string changePasswordToken, Personnel personnel)
        {
            string baseUrl = _config["FrontEnd:BaseUrl"] ?? "http://rtca-e-monitoring.runasp.net";
            string htmlBody = $@"
<table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""margin: 0; padding: 0; background-color: #f4f7f6; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;"">
        <tr>
            <td style=""padding: 40px 0;"">
                <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""550"" style=""background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 15px rgba(0,0,0,0.08);"">
                    
                    <tr>
                        <td style=""background-color: #1677ff; padding: 40px 20px; text-align: center;"">
                            <div style=""background: rgba(255,255,255,0.2); width: 60px; height: 60px; border-radius: 50%; display: inline-block; line-height: 60px; margin-bottom: 15px; border: 1px solid rgba(255,255,255,0.5);"">
                                <span style=""color: white; font-size: 30px;"">👋</span>
                            </div>
                            <h1 style=""margin: 0; color: white; font-size: 24px; font-weight: 700; letter-spacing: 1px;"">WELCOME TO RTC AURORA</h1>
                        </td>
                    </tr>

                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <p style=""font-size: 16px; color: #333; line-height: 1.6; margin-bottom: 20px;"">
                                Hello <strong>{personnel?.Rank?.RankCode} {personnel?.FirstName} {personnel?.MiddleName} {personnel?.LastName} {personnel?.SerialNumber}</strong>,
                            </p>
                            <p style=""font-size: 15px; color: #666; line-height: 1.6;"">
                                Your account for the <strong>E-Monitoring System</strong> has been successfully created. You can now log in using the temporary credentials provided below.
                            </p>

                            <div style=""background-color: #f9fafb; border: 1px solid #eaecf0; border-radius: 8px; padding: 20px; margin: 25px 0;"">
                                <table width=""100%"" cellspacing=""0"" cellpadding=""0"">
                                    <tr>
                                        <td style=""padding: 5px 0; color: #888; font-size: 13px; text-transform: uppercase;"">Login Email</td>
                                    </tr>
                                    <tr>
                                        <td style=""padding-bottom: 15px; font-size: 16px; color: #111; font-weight: 600;"">{personnel?.Email}</td>
                                    </tr>
                                    <tr>
                                        <td style=""padding: 5px 0; color: #888; font-size: 13px; text-transform: uppercase;"">Default Password</td>
                                    </tr>
                                    <tr>
                                        <td style=""padding-bottom: 5px; font-size: 16px; color: #111; font-weight: 600;"">
                                            <code style=""background: #eee; padding: 2px 6px; border-radius: 4px;"">{defaultPassword}</code>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""margin-bottom: 30px;"">
                                <tr>
                                    <td style=""background-color: #fff7e6; border-left: 4px solid #ffa940; padding: 12px 15px; color: #d46b08; font-size: 13px; line-height: 1.4;"">
                                        <strong>Security Note:</strong> For your protection, you are required to change this temporary password immediately after your first login.
                                    </td>
                                </tr>
                            </table>

                            <div style=""text-align: center;"">
                                <a href=""{baseUrl}/change-password/{changePasswordToken}"" style=""background-color: #1677ff; color: white; padding: 14px 28px; text-decoration: none; border-radius: 6px; font-weight: bold; font-size: 15px; display: inline-block; box-shadow: 0 2px 8px rgba(22, 119, 255, 0.3);"">
                                    Change Password & Log In
                                </a>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td style=""background-color: #fafafa; padding: 30px; text-align: center; border-top: 1px solid #f0f0f0;"">
                            <p style=""margin: 0; font-size: 12px; color: #999; line-height: 1.5;"">
                                This is an automated message. Please do not reply to this email.<br>
                                RTC Aurora E-Monitoring System
                            </p>
                            <div style=""margin-top: 15px;"">
                                <a href=""#"" style=""color: #1677ff; text-decoration: none; font-size: 12px;"">Support Center</a>
                                <span style=""color: #ddd; margin: 0 10px;"">|</span>
                                <a href=""#"" style=""color: #1677ff; text-decoration: none; font-size: 12px;"">Privacy Policy</a>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>";

            await _emailSenderUtility.SendEmailAsync(personnel?.Email, "Your Account Information", htmlBody);
        }

        public async Task<Usertbl?> UpdateAsync(Usertbl data)
        {
            var user = await _usertblRepository.GetByIdAsync(data.UserId ?? 0);
            if (user == null) throw new Exception("User not found.");

            user.UserName = !string.IsNullOrWhiteSpace(data.UserName) ? data.UserName : user.UserName;
            user.Email = !string.IsNullOrWhiteSpace(data.Email) ? data.Email : user.Email;
            user.FullName = !string.IsNullOrWhiteSpace(data.FullName) ? data.FullName : user.FullName;

            if (data.RoleId.HasValue && data.RoleId > 0)
            {
                user.RoleId = data.RoleId;
            }

            user.IsActive = data.IsActive ?? user.IsActive;

            if (!string.IsNullOrWhiteSpace(data.Password))
            {

                user.HashedPassword = _encryptUtility.GenerateHashedPassword(data.Password, user.Salt);
                user.IsDefaultPassword = false;
            }

            return await _usertblRepository.UpdateAsync(user);
        }
        public async Task<Usertbl?> ChangeDefaultPassword(Usertbl data)
        {
            var user = await _usertblRepository.GetByIdAsync(data.UserId ?? 0);
            if (user == null) throw new Exception("Invalid User");

            string hashedPassword = _encryptUtility.GenerateHashedPassword(data?.Password ?? "", user.Salt);

            user.HashedPassword = hashedPassword;
            user.IsDefaultPassword = false;
            return await _usertblRepository.UpdateAsync(user);
        }

        public async Task<IEnumerable<Usertbl>> GetAllAsync(Usertbl? filter)
        {
            return await _usertblRepository.GetAllAsync(filter);
        }

        public async Task<Usertbl?> GetByIdAsync(int id)
        {
            return await _usertblRepository.GetByIdAsync(id);
        }

        public async Task<Usertbl?> DeleteByIdAsync(int id)
        {
            var user = await _usertblRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("Invalid User");

            var personnel = await _personnelRepository.GetByIdAsync(user?.PersonnelId ?? 0);
            if (personnel != null)
            {
                personnel.HasAccount = false;
                await _personnelRepository.UpdateAsync(personnel);
            }

            return await _usertblRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<Usertbl>> BulkInsertAsync(List<Usertbl> data)
        {
            return await _usertblRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<Usertbl>> BulkUpdateAsync(List<Usertbl> data)
        {
            return await _usertblRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<Usertbl>> BulkUpsertAsync(List<Usertbl> data)
        {
            return await _usertblRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<Usertbl>> BulkMergeAsync(List<Usertbl> data)
        {
            return await _usertblRepository.BulkMergeAsync(data);
        }
    }
}