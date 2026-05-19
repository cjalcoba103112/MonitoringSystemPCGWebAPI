using System.Diagnostics;
using ApplicationContexts;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;
using Services.Interfaces;
using Utilities.Classes;
using Utilities.Interfaces;

namespace Services.Classes
{
    public class ApprovalProccessService : IApprovalProccessService
    {
        private readonly IApprovalProccessRepository _approvalProccessRepository;
        private readonly IPersonnelActivityRepository _personnelActivityRepository;
        private readonly IEmailSenderUtility _emailSenderUtility;
        private readonly IPersonnelRepository _personnelRepository;
        private readonly IDayUtility _dayUtility;
        private readonly IClaimsHelperUtility _claimsHelperUtility;
        private readonly IActivityAppealRepository _activityAppealRepository;
        private readonly ApplicationContext _context = new ApplicationContext();
        private readonly IConfigurationRoot _config = new AppUtility().GetConfiguration();
        public ApprovalProccessService(IApprovalProccessRepository approvalProccessRepository, IPersonnelActivityRepository personnelActivityRepository, IEmailSenderUtility emailSenderUtility, IPersonnelRepository personnelRepository, IDayUtility dayUtility, IClaimsHelperUtility claimsHelperUtility, IActivityAppealRepository activityAppealRepository )
        {
            _approvalProccessRepository = approvalProccessRepository;
            _personnelActivityRepository = personnelActivityRepository;
            _emailSenderUtility = emailSenderUtility;
            _personnelRepository = personnelRepository;
            _dayUtility = dayUtility;
            _claimsHelperUtility = claimsHelperUtility;
            _activityAppealRepository = activityAppealRepository;
        }

        public async Task<ApprovalProccess?> InsertAsync(ApprovalProccess data)
        {
            return await _approvalProccessRepository.InsertAsync(data);
        }

        public async Task<ApprovalProccess?> UpdateStageOne(ApprovalProccess data)
        {
            var approval = await _approvalProccessRepository.GetByIdAsync(data?.Id ?? 0);

            if (approval?.Id == null) throw new Exception("Approval Process is not found.");
            var activity = _context.PersonnelActivity.FirstOrDefault(pa => pa.ApprovalProccessId == (approval.Id ?? 0));
            int? userId = _claimsHelperUtility.GetUserId();

            approval.StageOneRemarks = data.StageOneRemarks;
            approval.StageOneId = data.StageOneId;
            approval.StageOneIsApprove = data.StageOneIsApprove;
            approval.CurrentStage = 2;

            if (approval.StageOneIsApprove == false)
            {

                await DeclineAsync(activity.PersonnelActivityId, data.StageOneRemarks, userId);
            }
            else
            {
               
                await SendStageNotification(approval.Id ?? 0, data.StageOneRemarks, 2);
            }

            var result = await _approvalProccessRepository.UpdateAsync(approval);



            return result;
        }

        public async Task<ApprovalProccess?> UpdateStageTwo(ApprovalProccess data)
        {
            var approval = await _approvalProccessRepository.GetByIdAsync(data?.Id ?? 0);
            if (approval == null) throw new Exception("Approval Process is not found.");

            var activity = _context.PersonnelActivity.FirstOrDefault(pa => pa.ApprovalProccessId == (approval.Id ?? 0));
            int? userId = _claimsHelperUtility.GetUserId();

            approval.StageTwoRemarks = data.StageTwoRemarks;
            approval.StageTwoId = data.StageTwoId;
            approval.StageTwoIsApprove = data.StageTwoIsApprove;
            approval.CurrentStage = 3;

            if (approval.StageTwoIsApprove == false)
            {

                await DeclineAsync(activity.PersonnelActivityId, data.StageTwoRemarks, userId);
            }
            else
            {
              
                await SendStageNotification(approval.Id ?? 0, data.StageTwoRemarks, 3);
            }


            var result = await _approvalProccessRepository.UpdateAsync(approval);

            return result;
        }

        public async Task<ApprovalProccess?> UpdateStageThree(ApprovalProccess data)
        {
            var approval = await _approvalProccessRepository.GetByIdAsync(data?.Id ?? 0);
            if (approval == null) throw new Exception("Approval Process is not found.");

            var activity = _context.PersonnelActivity.FirstOrDefault(pa => pa.ApprovalProccessId == (approval.Id ?? 0));
            int? userId = _claimsHelperUtility.GetUserId();

            approval.StageThreeRemarks = data.StageThreeRemarks;
            approval.StageThreeId = data.StageThreeId;
            approval.StageThreeIsApprove = data.StageThreeIsApprove;
            approval.CurrentStage = 4;

            if (approval.StageThreeIsApprove == false)
            {

                await DeclineAsync(activity.PersonnelActivityId, data.StageThreeRemarks, userId);
            }
            else
            {
                await SendStageNotification(approval.Id ?? 0, data.StageThreeRemarks, 4);
            }

            var result = await _approvalProccessRepository.UpdateAsync(approval);


            return result;
        }

        public async Task<ApprovalProccess?> UpdateFinalStage(ApprovalProccess data, int personnelActivityId)
        {
            var approval = await _approvalProccessRepository.GetByIdAsync(data?.Id ?? 0);
            if (approval == null) throw new Exception("Approval Process is not found.");

            var activity = await _personnelActivityRepository.GetByIdAsync(personnelActivityId);
            if (activity == null) throw new Exception("No Activity found.");
            int? userId = _claimsHelperUtility.GetUserId();

            approval.StageFourRemarks = data.StageFourRemarks;
            approval.StageFourId = data.StageFourId;
            approval.StageFourIsApprove = data.StageFourIsApprove;

            approval.CurrentStage = 5;

            activity.IsFullyApproved = (approval.StageFourIsApprove ?? false);
            activity.Status = (approval.StageFourIsApprove ?? false)
                ? _personnelActivityRepository.GetActivityStatus(activity.StartDate, activity.EndDate, activity.Status)
                : "Declined";

            await _personnelActivityRepository.UpdateAsync(activity);
            var result = await _approvalProccessRepository.UpdateAsync(approval);

            if (approval.StageFourIsApprove == false)
            {

                await DeclineAsync(activity.PersonnelActivityId, data.StageFourRemarks, userId, true);
            }
            else
            {
                await SendStageNotification(approval.Id ?? 0, data.StageFourRemarks, 5);
            }

            return result;
        }

        public async Task<PersonnelActivity?> DeclineAsync(int? personnelActivityId, string? remarks, int? userId = null, bool isFinal = false)
        {
            var data = await _context.PersonnelActivity
                .Include(pa => pa.ApprovalProccess)
                .FirstOrDefaultAsync(pa => pa.PersonnelActivityId == personnelActivityId);

            if (data == null) throw new Exception("No Activity found");

            var personnel = await _personnelRepository.GetByIdAsync(data.PersonnelId ?? 0);
            var activityType = await _context.ActivityType.FirstOrDefaultAsync(at => at.ActivityTypeId == data.ActivityTypeId);


            string token = Guid.NewGuid().ToString();

            var user = await _context.Usertbl
.Include(p => p.Personnel).ThenInclude(p => p.Rank)
.Include(p => p.Role)
.FirstOrDefaultAsync(d => d.UserId == userId);

            string? appealTargetRoleName = null;
            if (!isFinal && data.ApprovalProccess != null)
            {
                // The target is the NEXT stage in the workflow
                int nextStage = (data.ApprovalProccess.CurrentStage ?? 0) + 1;

                // Query your WorkflowSteps table to see what Role is assigned to the next stage
                // for this specific personnel type (Officer vs Non-Officer)
                var nextStep = await _context.WorkflowSteps
                    .Include(ws => ws.Role)
                    .FirstOrDefaultAsync(ws =>
                        ws.StepNumber == nextStage &&
                        ws.RankCategoryId == (data.PersonnelType == "Officer" ? 1 : 2));

                appealTargetRoleName = nextStep?.Role?.RoleName ?? "Higher Authority";
            }

            await _activityAppealRepository.InsertAsync(new ActivityAppeal
            {
                PersonnelActivityId = personnelActivityId,
                AppealToken = token,
                IsUsed = false,
                DisapprovedRoleName = user?.Role?.RoleName, // e.g., "CMAA"
                AppealTargetRoleName = appealTargetRoleName, // e.g., "OIC"
                ExpiryDate = DateTime.Now.AddDays(3)
            });

            string disapproverName = user.Personnel != null
                ? $"{user.Personnel?.Rank?.RankCode} {user.Personnel.FirstName} {user.Personnel.MiddleName} {user.Personnel.LastName} {user.Personnel.SerialNumber}"
                : "Approving Authority";

            string baseUrl = _config["FrontEnd:BaseUrl"] ?? "http://rtca-e-monitoring.runasp.net";
            string appealUrl = $"{baseUrl}/activities/appeal/{token}";

            await _emailSenderUtility.SendEmailAsync(personnel?.Email, $"Request Declined - {activityType?.ActivityTypeName}",
                $@"<div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #f0f0f0; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.05);"">
            
            <div style=""background-color: #ff4d4f; padding: 30px; text-align: center;"">
                <div style=""background: white; width: 60px; height: 60px; border-radius: 50%; display: inline-block; line-height: 60px; margin-bottom: 15px;"">
                    <span style=""color: #ff4d4f; font-size: 30px;"">✘</span>
                </div>
                <h2 style=""margin: 0; color: #ffffff; font-size: 22px; letter-spacing: 0.5px;"">Request Declined</h2>
            </div>

            <div style=""padding: 30px; background-color: #ffffff;"">
                <p style=""font-size: 16px; color: #333;"">Hello <strong>{personnel?.Rank?.RankCode} {personnel?.FirstName} {personnel?.MiddleName} {personnel?.LastName} {personnel?.SerialNumber}</strong>,</p>
                <p style=""font-size: 15px; color: #666; line-height: 1.5;"">We regret to inform you that your request for <strong>{activityType?.ActivityTypeName}</strong> has been <strong>Declined</strong>.</p>
                
                <div style=""background-color: #fff2f0; border: 1px solid #ffccc7; padding: 12px; border-radius: 8px; margin: 20px 0; text-align: center;"">
                    <span style=""font-size: 12px; color: #ff4d4f; text-transform: uppercase; font-weight: bold;"">Declined By:</span><br/>
                    <strong style=""font-size: 15px; color: #434343;"">{user?.Role?.RoleName}</strong><br/>
                </div>

                <div style=""margin: 25px 0; border-top: 1px solid #f0f0f0; border-bottom: 1px solid #f0f0f0; padding: 15px 0;"">
                    <table style=""width: 100%; font-size: 14px; color: #444;"">
                        <tr>
                            <td style=""padding: 5px 0;""><strong>Period:</strong></td>
                            <td style=""padding: 5px 0; text-align: right;"">{data.StartDate?.ToString("dd MMM yyyy").ToUpper()} — {data.EndDate?.ToString("dd MMM yyyy").ToUpper()}</td>
                        </tr>
 <tr>
                            <td style=""padding: 5px 0; vertical-align: top;""><strong>Day/s</strong></td>
                            <td style=""padding: 5px 0; text-align: right; color: #cf1322; font-weight: 600;"">{data.Days}</td>
                        </tr>
                        <tr>
                            <td style=""padding: 5px 0; vertical-align: top;""><strong>Reason for Decline:</strong></td>
                            <td style=""padding: 5px 0; text-align: right; color: #cf1322; font-weight: 600;"">{remarks ?? "No specific reason provided."}</td>
                        </tr>
                    </table>
                </div>
{(!isFinal ? $@"
<div style=""text-align: center; margin: 30px 0;"">
    <a href=""{appealUrl}"" style=""background-color: #1890ff; color: white; padding: 12px 25px; text-decoration: none; font-weight: bold; border-radius: 6px; display: inline-block; box-shadow: 0 2px 0 rgba(0,0,0,0.045);"">
        File an Appeal
    </a>
</div>" : string.Empty)}
                

                <p style=""font-size: 13px; color: #888; text-align: center; font-style: italic;"">
                    If you wish to appeal this decision, please click the button above or contact your unit's HR officer.
                </p>
            </div>

            <div style=""background-color: #fafafa; padding: 20px; text-align: center; border-top: 1px solid #f0f0f0;"">
                <p style=""margin: 0; font-size: 11px; color: #bbb;"">© 2026 RTC Aurora E-Monitoring System</p>
            </div>
        </div>"
            );

            // Update the DB record
            data.Remarks = remarks;
            data.Status = "Declined";

            // Also mark the specific stage that caused the decline as false
            if (data.ApprovalProccess != null)
            {
                switch (data.ApprovalProccess.CurrentStage)
                {
                    case 1: data.ApprovalProccess.StageOneIsApprove = false; break;
                    case 2: data.ApprovalProccess.StageTwoIsApprove = false; break;
                    case 3: data.ApprovalProccess.StageThreeIsApprove = false; break;
                    case 4: data.ApprovalProccess.StageFourIsApprove = false; break;
                }
            }

            return await _personnelActivityRepository.UpdateAsync(data);
        }
        public async Task<IEnumerable<ApprovalProccess>> GetAllAsync(ApprovalProccess? filter)
        {
            return await _approvalProccessRepository.GetAllAsync(filter);
        }

        public async Task<ApprovalProccess?> GetByIdAsync(int id)
        {
            return await _approvalProccessRepository.GetByIdAsync(id);
        }

        public async Task<ApprovalProccess?> DeleteByIdAsync(int id)
        {
            return await _approvalProccessRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<ApprovalProccess>> BulkInsertAsync(List<ApprovalProccess> data)
        {
            return await _approvalProccessRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<ApprovalProccess>> BulkUpdateAsync(List<ApprovalProccess> data)
        {
            return await _approvalProccessRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<ApprovalProccess>> BulkUpsertAsync(List<ApprovalProccess> data)
        {
            return await _approvalProccessRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<ApprovalProccess>> BulkMergeAsync(List<ApprovalProccess> data)
        {
            return await _approvalProccessRepository.BulkMergeAsync(data);
        }

        public async Task<ApprovalProccess?> UpdateAsync(ApprovalProccess data)
        {
            return await _approvalProccessRepository.UpdateAsync(data);
        }
        private async Task SendStageNotification(int approvalId, string remarks, int nextStage)
        {
            var activity = await _personnelActivityRepository.GetActivityWithApprovalDetailsAsync(approvalId);
            if (activity?.Personnel == null) return;

            bool isOfficer = activity.PersonnelType == "Officer";
            if (isOfficer && nextStage < 3) return;

            // Pass activity.Status to the email generator
            string emailBody = await GenerateApprovalEmailBody(activity.Personnel, activity, remarks, nextStage, "System Administrator");

            // Dynamic Subject based on status
            string statusText = activity.Status == "Declined" ? "DISAPPROVED" :
                               (activity.IsFullyApproved ?? false) ? "APPROVED" : "Update";

            string subject = $"{statusText}: {activity.ActivityType?.ActivityTypeName} ({activity.Title ?? ""})";

            await _emailSenderUtility.SendEmailAsync(activity.Personnel.Email, subject, emailBody);
        }
        private async Task<string> GenerateApprovalEmailBody(Personnel personnel, PersonnelActivity activity, string remarks, int currentStage, string approverName = "System Administrator")
        {
            bool isOfficer = activity.PersonnelType == "Officer";
            int totalSteps = isOfficer ? 2 : 4;
            var workFlows = await _context.WorkflowSteps
    .Include(w => w.Role) // Ensure Role is loaded to avoid NullReference on RoleName
    .ToListAsync();

            // 2. Fix the conditional array logic
            string[] labels = isOfficer
                ? workFlows?
                    .Where(w => w.RankCategoryId == 1)
                    .Select(w => w.Role.RoleName)
                    .ToArray()
                : workFlows?
                    .Where(w => w.RankCategoryId == 2)
                    .Select(w => w.Role.RoleName)
                    .ToArray();


            decimal totalDays = _dayUtility.CountDays(activity.StartDate, activity.EndDate, activity.ActivityType.IsMandatoryLeave);

            // Formatting the Duration string
            string durationText = $"{totalDays} Day{(totalDays == 1 ? "" : "s")}";

            var credits = await _personnelRepository.GetPersonnelCreditsAsync(personnelId: activity.PersonnelId ?? 0, activityTypeId: activity.ActivityTypeId ?? 0, date: activity.StartDate);
            decimal currentBalance = credits?.FirstOrDefault()?.RemainingCredits ?? 0;
            decimal remainingCredits = currentBalance - totalDays;

            // Determine status of the current step
            bool? currentStepStatus = (currentStage - 1) switch
            {
                1 => activity.ApprovalProccess.StageOneIsApprove,
                2 => activity.ApprovalProccess.StageTwoIsApprove,
                3 => activity.ApprovalProccess.StageThreeIsApprove,
                4 => activity.ApprovalProccess.StageFourIsApprove,
                _ => null
            };

            bool isDisapproved = currentStepStatus == false;
            bool isFullyApproved = activity.IsFullyApproved ?? false;

            // --- LOGIC CALCULATIONS ---
            int labelIdx = isOfficer ? (currentStage - 3) : (currentStage - 1);
            int nextLabelIdx = isOfficer ? (currentStage - 2) : currentStage;

            // Safe retrieval of names
            string currentStageName = (labelIdx >= 0 && labelIdx < labels.Length) ? labels[labelIdx - 1] : "Administrator";
            string nextStageName = (nextLabelIdx >= 0 && nextLabelIdx < labels.Length) ? labels[nextLabelIdx] : "Final Processing";

            // Define the "Next Step" text
            string progressionText = isFullyApproved
                ? "Process Completed"
                : $"Next Step: Moving to <b>{nextStageName.ToUpper()}</b>";

            // Status-specific visual overrides
            string themeColor = "#1677ff";
            string icon = "⏳";
            string statusText = "REQUEST UPDATED";

            if (isFullyApproved)
            {
                themeColor = "#52c41a"; icon = "✔";
                statusText = "FINAL APPROVAL GRANTED";
            }
            else if (isDisapproved)
            {
                themeColor = "#faad14"; // Warning Orange (since it's disapproved but still moving)
                icon = "⚠";
                statusText = $"DISAPPROVED BY {currentStageName.ToUpper()}";
            }
            else
            {
                themeColor = "#1677ff"; // Info Blue
                icon = "📥";
                statusText = $"APPROVED BY {currentStageName.ToUpper()}";
            }

            // Build the HTML Header
            string statusHeaderHtml = $@"
<div style=""background-color: {themeColor}; padding: 20px 20px; text-align: center; color: white;"">
    <div style=""background: rgba(255,255,255,0.2); width: 50px; height: 50px; border-radius: 50%; display: inline-block; line-height: 50px; margin-bottom: 12px; border: 1px solid white;"">
         <span style=""color: white; font-size: 22px;"">{icon}</span>
    </div>
    <h2 style=""margin: 0; font-size: 17px; font-weight: 700; letter-spacing: 1px; text-transform: uppercase;"">
        {statusText}
    </h2>
    <div style=""margin-top: 8px; font-size: 12px; opacity: 0.9; letter-spacing: 0.5px;"">
        {progressionText}
    </div>
</div>";

            // --- STEPPER LOGIC ---
            string stepperHtml = "";
            for (int i = 1; i <= totalSteps; i++)
            {
                int actualDbStage = isOfficer ? (i + 2) : i;
                bool? thisStepApprove = actualDbStage switch
                {
                    1 => activity.ApprovalProccess.StageOneIsApprove,
                    2 => activity.ApprovalProccess.StageTwoIsApprove,
                    3 => activity.ApprovalProccess.StageThreeIsApprove,
                    4 => activity.ApprovalProccess.StageFourIsApprove,
                    _ => null
                };

                bool isStepDisapproved = thisStepApprove == false;
                bool isStepApproved = thisStepApprove == true;
                bool isStepActive = (actualDbStage <= currentStage - 1);

                string color = "#5a5a5a"; // Gray (Pending)
                string circleText = i.ToString();

                if (isStepDisapproved) { color = "#f5222d"; circleText = "✘"; }
                else if (isStepApproved) { color = "#52c41a"; circleText = "✔"; }
                else if (isStepActive) { color = "#1677ff"; }

                stepperHtml += $@"
            <td style=""text-align: center; width: {(100 / totalSteps)}%;"">
                <div style=""display: inline-block; width: 28px; height: 28px; line-height: 28px; border-radius: 50%; background-color: {color}; color: white; font-size: 11px; font-weight: bold; margin-bottom: 5px;"">
                    {circleText}
                </div>
                <div style=""font-size: 9px; color: {color}; font-weight: bold;"">
                    {labels[i - 1]}
                </div>
            </td>";

                if (i < totalSteps)
                {
                    string lineColor = (isStepApproved) ? "#52c41a" : "#d9d9d9";
                    stepperHtml += $@"<td style=""padding-top: 10px; vertical-align: top;""><div style=""height: 2px; width: 20px; background-color: {lineColor}; margin-top: 13px;""></div></td>";
                }
            }

            return $@"
    <div style=""font-family: 'Segoe UI', Tahoma, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #f0f0f0; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.05);"">
        <div style=""background-color: {themeColor}; padding: 30px; text-align: center; color: white;"">
            <h2 style=""margin: 0; font-size: 18px; letter-spacing: 1px; text-transform: uppercase;"">{statusHeaderHtml}</h2>
        </div>
        <div style=""padding: 30px; background-color: white;"">
            <p style=""font-size: 16px; color: #333;"">Hello <strong>{personnel?.Rank?.RankCode} {personnel?.FirstName} {personnel?.LastName} {personnel?.SerialNumber}</strong>,</p>
            
            <p style=""font-size: 15px; color: #666; line-height: 1.5;"">
                {(isDisapproved
                            ? $"We regret to inform you that your request for <strong>{activity.ActivityType?.ActivityTypeName}</strong> was not approved at the current stage."
                            : $"Your request for <strong>{activity.ActivityType?.ActivityTypeName}</strong> is currently being processed. Status details are provided below:")}
            </p>

            <div style=""margin: 25px 0; padding: 20px; background-color: #fafafa; border-radius: 8px; border: 1px solid #f0f0f0;"">
                <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"">
                    <tr>{stepperHtml}</tr>
                </table>
            </div>

            <div style=""margin: 25px 0; border-top: 2px solid #f8f9fa; border-bottom: 2px solid #f8f9fa; padding: 15px 0;"">
                <h4 style=""margin: 0 0 10px 0; color: #555; text-transform: uppercase; font-size: 11px; letter-spacing: 1px;"">Request Summary</h4>
                <table style=""width: 100%; font-size: 14px; color: #444; border-collapse: collapse;"">
                    <tr><td style=""padding: 6px 0; color: #888;"">Title:</td><td style=""text-align: right; font-weight: 600;"">{activity.Title}</td></tr>
                    <tr><td style=""padding: 6px 0; color: #888;"">Reason for Action:</td><td style=""text-align: right;"">{activity.Reason ?? "Not Specified"}</td></tr>
                    <tr><td style=""padding: 6px 0; color: #888;"">Inclusive Dates:</td><td style=""text-align: right;"">{activity.StartDate?.ToString("dd MMM yyyy")} - {activity.EndDate?.ToString("dd MMM yyyy")}</td></tr>
                    <tr><td style=""padding: 6px 0; color: #888;"">Total Duration:</td><td style=""text-align: right; font-weight: 600;"">{durationText}</td></tr>
                    
                    <tr style=""border-top: 1px dashed #eee;"">
                        <td style=""padding: 12px 0 0 0; color: {(isDisapproved ? "#f5222d" : "#1677ff")}; font-weight: bold;"">{(isDisapproved ? "Disapproved By:" : "Current Reviewer:")}</td>
                        <td style=""padding: 12px 0 0 0; text-align: right; font-weight: bold;"">{approverName}</td>
                    </tr>
                    <tr>
                        <td style=""padding: 5px 0; color: #888;"">Remarks:</td>
                        <td style=""padding: 5px 0; text-align: right; color: {(isDisapproved ? "#f5222d" : "#d48806")}; font-style: italic;"">""{remarks ?? "No remarks provided"}""</td>
                    </tr>
                </table>
            </div>

            {(isFullyApproved ? $@"
            <div style=""background-color: #f6ffed; border: 1px solid #b7eb8f; padding: 15px; border-radius: 8px;"">
                <table style=""width: 100%;"">
                    <tr>
                        <td style=""font-size: 14px; color: #389e0d;""><strong>Updated Leave Balance:</strong></td>
                        <td style=""font-size: 18px; color: #237804; font-weight: bold; text-align: right;"">{remainingCredits} Day(s)</td>
                    </tr>
                </table>
            </div>" : "")}
        </div>
        <div style=""background-color: #fafafa; padding: 20px; text-align: center; border-top: 1px solid #f0f0f0;"">
            <p style=""margin: 0; font-size: 11px; color: #999;"">RTC Aurora E-Monitoring System Automated Notification</p>
        </div>
    </div>";
        }
    }
}