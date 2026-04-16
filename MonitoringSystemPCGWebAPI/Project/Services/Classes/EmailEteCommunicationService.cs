
using Humanizer;
using Microsoft.VisualBasic;
using Models;
using Repositories.Interfaces;
using Services.Interfaces;
using Utilities.Classes;
using Utilities.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Classes
{
    public class EmailEteCommunicationService : IEmailEteCommunicationService
    {
        private readonly IEmailEteCommunicationRepository _emailEteCommunicationRepository;
        private readonly IEmailSenderUtility _emailSenderUtility;
        private readonly IPersonnelRepository _personnelRepository;
        private readonly IFileUtility _fileUtility;

        private string baseUrl = "";
        public EmailEteCommunicationService(IEmailEteCommunicationRepository emailEteCommunicationRepository, IEmailSenderUtility emailSenderUtility, IPersonnelRepository personnelRepository, IAppUtility appUtility)
        {
            _emailEteCommunicationRepository = emailEteCommunicationRepository;
            _emailSenderUtility = emailSenderUtility;
            _personnelRepository = personnelRepository;
            _fileUtility = new FileUtility("wwwroot/documents/etes");
            baseUrl = new AppUtility().GetConfiguration()["FrontEnd:BaseUrl"] ?? "";

        }

        private string RequestExplanationHtmlBody(Personnel personnel, string remarks, string token, DateTime? expiryDateTime, string humanizedRemaining, DateTime eteDate)
        {
            string replyUrl = $"{baseUrl}/ete-explanation/{token}";
            string htmlBody = $@"
<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden;'>
    <div style='background-color: #001529; padding: 20px; text-align: center;'>
        <h2 style='color: #ffffff; margin: 0;'>ETE REQUEST FOR EXPLANATION</h2>
    </div>
    
    <div style='padding: 30px; line-height: 1.6; color: #333333;'>
        <div style='text-align: right; margin-bottom: 10px;'>
            <span style='background-color: #cf1322; color: white; padding: 4px 10px; border-radius: 4px; font-size: 11px; font-weight: bold;'>
                ACTION REQUIRED: BELOW 11 MONTHS
            </span>
        </div>

        <p>Hello <b>{personnel?.Rank?.RankCode} {personnel?.FirstName} {personnel?.MiddleName} {personnel?.LastName} {personnel?.SerialNumber} </b>,</p>
        
        <p>This is a formal request for explanation regarding your <strong>Expiration of Term of Enlistment (ETE)</strong> status.</p>
        
        <p style='color: #cf1322; font-weight: bold;'>
            Our records show you have less than 11 months remaining before your ETE, and the required documentation has not yet been finalized.
        </p>

        <div style='background-color: #fff1f0; border: 1px solid #ffa39e; border-radius: 8px; padding: 15px; margin: 20px 0;'>
            <table style='width: 100%;'>
                <tr>
                    <td style='color: #666;'>Next ETE Date:</td>
                    <td style='text-align: right; font-weight: bold;'>{eteDate:MMMM dd, yyyy}</td>
                </tr>
                <tr>
                    <td style='color: #666;'>Time Remaining:</td>
                    <td style='text-align: right; font-weight: bold; color: #cf1322;'>{humanizedRemaining} (Critical)</td>
                </tr>
            </table>
        </div>

        <p><strong>Instruction/Remarks:</strong></p>
        <p style='background-color: #f5f5f5; padding: 15px; border-left: 4px solid #cf1322; font-style: italic;'>
            {remarks}
        </p>

        <p>Please click the button below to provide your formal explanation as to why documentation was not submitted within the prescribed period, or to acknowledge this final notice.</p>

        <div style='text-align: center; margin-top: 30px;'>
            <a href='{replyUrl}' 
               style='background-color: #cf1322; color: white; padding: 12px 25px; text-decoration: none; border-radius: 4px; font-weight: bold; display: inline-block;'>
               Submit Explanation Now
            </a>
        </div>

        <p style='margin-top: 30px; font-size: 0.8em; color: #888;'>
            Note: This link and your ability to reply will expire on <strong>{expiryDateTime:MMMM dd, yyyy}</strong>.
        </p>
    </div>
    <div style='background-color: #fafafa; padding: 15px; text-align: center; font-size: 0.8em; color: #aaaaaa; border-top: 1px solid #eeeeee;'>
        Automated Personnel Compliance System - Unauthorized use is prohibited.
    </div>
</div>";

            return htmlBody;
        }

        private string NotifyHtmlBody(Personnel personnel, string remarks, string token, DateTime? expiryDateTime, string humanizedRemaining, DateTime eteDate)
        {
            // Use the specific notification route for warnings
            string replyUrl = $"{baseUrl}/ete-notify/{token}";
            string fullName = $"{personnel?.Rank?.RankCode} {personnel?.FirstName} {personnel?.MiddleName} {personnel?.LastName} {personnel?.SerialNumber}";
            string formattedEteDate = eteDate.ToString("dd MMM yyyy").ToUpper();
            string formattedExpiry = expiryDateTime?.ToString("dd MMM yyyy HH:mm") ?? "N/A";

            return $@"
    <div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; border: 1px solid #e0e0e0; border-top: 8px solid #2d5a27;"">
        <div style=""padding: 25px; background-color: #f4fbf3; text-align: center; border-bottom: 1px solid #eee;"">
            <h2 style=""margin: 0; color: #2d5a27; letter-spacing: 1px;"">ETE PREPARATION NOTICE</h2>
            <p style=""margin: 5px 0 0; font-size: 14px; color: #666;"">ETE COMPLIANCE & MONITORING SYSTEM</p>
        </div>

        <div style=""padding: 30px;"">
            <p style=""font-size: 16px;"">Dear <strong>{fullName}</strong>,</p>
            
            <p style=""font-size: 15px;"">This is a proactive reminder regarding your upcoming <strong>End of Term of Enlistment (ETE)</strong> on <span style=""color: #2d5a27; font-weight: bold;"">{formattedEteDate}</span>.</p>
            
            <div style=""background-color: #f0f7ff; border-left: 4px solid #0056b3; padding: 15px; margin: 20px 0;"">
                <p style=""margin: 0; font-weight: bold; color: #0056b3;"">Status Update:</p>
                <p style=""margin: 5px 0 0; font-size: 14px;"">You have <strong>{humanizedRemaining}</strong> remaining. Please take this time to review your clearance requirements and begin coordinating with your respective offices.</p>
            </div>

            <div style=""background-color: #fff9e6; border: 1px dashed #e6b800; padding: 15px; border-radius: 6px; margin-bottom: 25px;"">
                <p style=""margin: 0; font-size: 13px; color: #856404;"">
                    <strong>Note:</strong> Per regulations, if documents are not finalized before the <b>11-month mark</b>, the system will automatically require a <b>Formal Statement of Explanation</b>.
                </p>
            </div>

            <p style=""font-size: 15px; font-weight: bold; margin-bottom: 5px;"">Administrative Remarks:</p>
            <div style=""background-color: #f9f9f9; padding: 15px; border-radius: 4px; font-style: italic; color: #555; border: 1px solid #eee; margin-bottom: 25px;"">
                ""{remarks}""
            </div>

            <p style=""font-size: 14px; color: #444;"">Please visit the portal below to view your preparation checklist and monitor your progress:</p>

            <div style=""text-align: center; margin: 35px 0;"">
                <a href=""{replyUrl}"" style=""background-color: #2d5a27; color: white; padding: 14px 30px; text-decoration: none; font-weight: bold; border-radius: 5px; font-size: 16px; display: inline-block;"">View Preparation Portal</a>
            </div>

            <p style=""font-size: 12px; color: #888; text-align: center;"">
                <strong>Security Notice:</strong> This link is unique to you and will expire on <br/>
                <span style=""color: #333;"">{formattedExpiry}</span>.
            </p>
        </div>

        <div style=""padding: 20px; background-color: #eee; text-align: center; font-size: 11px; color: #777;"">
            <p style=""margin: 0;"">This is an automated system-generated notification to assist you in staying compliant.</p>
            <p style=""margin: 5px 0 0;"">&copy; {DateTime.Now.Year} ETE Compliance Portal | Human Resource Management Office</p>
        </div>
    </div>";
        }
        public async Task<EmailEteCommunication?> InsertAsync(EmailEteCommunication data)
        {
            data.CommunicationToken = Guid.NewGuid().ToString();
            data.SentDateTime = DateTime.Now;
            data.ExpiryDateTime = DateTime.Now.AddDays(7);

            var personnel = await _personnelRepository.GetByIdAsync(data.PersonnelId ?? 0);
            if (personnel == null) throw new Exception("Personnel not found.");
            if (data.NextEte == null) throw new Exception("Personnel ETE date is not defined.");

            DateTime eteDate = data.NextEte.Value;
            int daysRemaining = (eteDate - DateTime.Now).Days;
            var diff = eteDate.Subtract(DateTime.Now);
            string humanizedRemaining = diff.Humanize(precision: 3, maxUnit: TimeUnit.Year, minUnit: TimeUnit.Day);

            string statusColor = data.EmailCategory == "REQUEST EXPLANATION" ? "#cf1322" : "#faad14";
            string statusText = data.EmailCategory == "REQUEST EXPLANATION" ? "CRITICAL: BELOW 11 MONTHS" : "PROACTIVE DOCUMENT PREPARATION";
            string subject = data.EmailCategory == "REQUEST EXPLANATION" ? "ETE REQUEST EXPLANATION" : "ETE PREPARATION NOTICE";
            var result = await _emailEteCommunicationRepository.InsertAsync(data);

            string htmlBody = "";

            if (data.EmailCategory == "REQUEST EXPLANATION")
            {
                htmlBody = RequestExplanationHtmlBody(personnel, data?.Remarks ?? "", data.CommunicationToken, data.ExpiryDateTime, humanizedRemaining, eteDate);
            }
            else
            {
                htmlBody = NotifyHtmlBody(personnel, data?.Remarks ?? "", data.CommunicationToken, data.ExpiryDateTime, humanizedRemaining, eteDate);
            }
            await _emailSenderUtility.SendEmailAsync(personnel.Email, subject, htmlBody);

            return result;
        }

        public async Task<EmailEteCommunication?> GetByToken(string token)
        {
            return await _emailEteCommunicationRepository.GetByToken(token);
        }

        public async Task<EmailEteCommunication?> GetByPersonnelId(int id, DateTime? nextETE)
        {
            return await _emailEteCommunicationRepository.GetByPersonnelId(id, nextETE);
        }

        public async Task<EmailEteCommunication?> UpdateAsync(EmailEteCommunication data, IFormFile? supportingDocument)
        {
            string? oldFilePath = data.SupportingDocument;

            if (supportingDocument != null)
            {
                string newFilename = _fileUtility.GetRandomFileName(supportingDocument.FileName);

                string relativePath = "/documents/etes/" + newFilename;

                await _fileUtility.CreateAsync(newFilename, supportingDocument.OpenReadStream());

                data.SupportingDocument = relativePath;
            }

            var result = await _emailEteCommunicationRepository.UpdateAsync(data);

            if (result != null && supportingDocument != null && !string.IsNullOrEmpty(oldFilePath))
            {
               
                string oldFileName = Path.GetFileName(oldFilePath);

                _fileUtility.Delete(oldFileName);
            }

            return result;
        }

        public async Task<IEnumerable<EmailEteCommunication>> GetAllAsync(EmailEteCommunication? filter)
        {
            return await _emailEteCommunicationRepository.GetAllAsync(filter);
        }

        public async Task<EmailEteCommunication?> GetByIdAsync(int id)
        {
            return await _emailEteCommunicationRepository.GetByIdAsync(id);
        }

        public async Task<EmailEteCommunication?> DeleteByIdAsync(int id)
        {
            return await _emailEteCommunicationRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<EmailEteCommunication>> BulkInsertAsync(List<EmailEteCommunication> data)
        {
            return await _emailEteCommunicationRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<EmailEteCommunication>> BulkUpdateAsync(List<EmailEteCommunication> data)
        {
            return await _emailEteCommunicationRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<EmailEteCommunication>> BulkUpsertAsync(List<EmailEteCommunication> data)
        {
            return await _emailEteCommunicationRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<EmailEteCommunication>> BulkMergeAsync(List<EmailEteCommunication> data)
        {
            return await _emailEteCommunicationRepository.BulkMergeAsync(data);
        }

        
    }
}