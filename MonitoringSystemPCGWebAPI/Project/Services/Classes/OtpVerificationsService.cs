
using Models;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;
using Repositories.Interfaces;
using Services.Interfaces;
using Utilities.Interfaces;

namespace Services.Classes
{
    public class OtpVerificationsService : IOtpVerificationsService
    {
        private readonly IOtpVerificationsRepository _otpVerificationsRepository;
        private readonly IEmailSenderUtility _emailSenderUtility; 
        public OtpVerificationsService(IOtpVerificationsRepository otpVerificationsRepository, IEmailSenderUtility emailSenderUtility)
        {
            _otpVerificationsRepository = otpVerificationsRepository;
            _emailSenderUtility = emailSenderUtility;
        }

        public async Task<OtpVerifications?> InsertAsync(OtpDTO data)
        {
            var oneHourAgo = DateTime.Now.AddHours(-1);
            var recentAttempts = await _otpVerificationsRepository.GetAttemptsCountAsync(data.Email, oneHourAgo);

            if (recentAttempts >= 4)
            {
                throw new Exception("Too many OTP attempts. Please try again in an hour.");
            }
            string otp = new Random().Next(100000, 999999).ToString();

            var otpEntry = new OtpVerifications
            {
                Email = data.Email,
                OtpCode = otp,
                ExpirationTime = DateTime.Now.AddMinutes(10), 
                IsUsed = false,
                CreatedAt = DateTime.Now
            };

            string subject = "🔒 Your Verification Code";

            string body = $@"
<div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f7f6; padding: 40px 0; width: 100%;"">
    <div style=""max-width: 500px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.05);"">
        
        <div style=""background-color: #1a73e8; padding: 20px; text-align: center;"">
            <h2 style=""color: #ffffff; margin: 0; font-size: 24px;"">Account Verification</h2>
        </div>

        <div style=""padding: 30px; text-align: center; color: #333333;"">
            <p style=""font-size: 16px; line-height: 1.5;"">Hello,</p>
            <p style=""font-size: 16px; line-height: 1.5;"">Use the following code to complete your signup. This code is valid for <strong>10 minutes</strong>.</p>
            
            <div style=""margin: 30px 0; padding: 15px; background-color: #f8f9fa; border: 1px dashed #1a73e8; border-radius: 4px;"">
                <span style=""font-size: 32px; font-weight: bold; letter-spacing: 5px; color: #1a73e8;"">{otp}</span>
            </div>

            <p style=""font-size: 13px; color: #777777;"">If you did not request this code, please ignore this email or contact support.</p>
        </div>

        <div style=""padding: 20px; background-color: #f1f3f4; text-align: center; font-size: 12px; color: #999999;"">
            &copy; {DateTime.Now.Year} Your Company Name. All rights reserved.
        </div>
    </div>
</div>";

            await _emailSenderUtility.SendEmailAsync(data.Email, subject, body);
            await _emailSenderUtility.SendEmailAsync(data.Email, subject, body);

            return await _otpVerificationsRepository.InsertAsync(otpEntry);
        }

        public async Task<OtpVerifications?> UpdateAsync(OtpVerifications data)
        {
            return await _otpVerificationsRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<OtpVerifications>> GetAllAsync(OtpVerifications? filter)
        {
            return await _otpVerificationsRepository.GetAllAsync(filter);
        }

        public async Task<OtpVerifications?> GetByIdAsync(int id)
        {
            return await _otpVerificationsRepository.GetByIdAsync(id);
        }

        public async Task<OtpVerifications?> DeleteByIdAsync(int id)
        {
            return await _otpVerificationsRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<OtpVerifications>> BulkInsertAsync(List<OtpVerifications> data)
        {
            return await _otpVerificationsRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<OtpVerifications>> BulkUpdateAsync(List<OtpVerifications> data)
        {
            return await _otpVerificationsRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<OtpVerifications>> BulkUpsertAsync(List<OtpVerifications> data)
        {
            return await _otpVerificationsRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<OtpVerifications>> BulkMergeAsync(List<OtpVerifications> data)
        {
            return await _otpVerificationsRepository.BulkMergeAsync(data);
        }
    }
}