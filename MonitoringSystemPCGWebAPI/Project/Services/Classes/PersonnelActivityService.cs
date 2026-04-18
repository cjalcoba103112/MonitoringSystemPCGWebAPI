
using Microsoft.EntityFrameworkCore;
using Models;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;
using Repositories.Interfaces;
using Services.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Classes;
using Utilities.Interfaces;

namespace Services.Classes
{
    public class PersonnelActivityService : IPersonnelActivityService
    {
        private readonly IPersonnelActivityRepository _personnelActivityRepository;
        private readonly IActivityTypeRepository _activityTypeRepository;
        private readonly IPersonnelRepository _personnelRepository;
        private readonly IEmailSenderUtility _emailSenderUtility;
        private readonly IAutoMapperUtility _mapper;
        private readonly IDayUtility _dayUtility;
        public PersonnelActivityService(IPersonnelActivityRepository personnelActivityRepository, IPersonnelRepository personnelRepository, IAutoMapperUtility mapper, IEmailSenderUtility emailSenderUtility, IActivityTypeRepository activityTypeRepository, IDayUtility dayUtility)
        {
            _personnelActivityRepository = personnelActivityRepository;
            _personnelRepository = personnelRepository;
            _mapper = mapper;
            _emailSenderUtility = emailSenderUtility;
            _activityTypeRepository = activityTypeRepository;
            _dayUtility = dayUtility;
        }
        public async Task<IEnumerable<PersonnelActivity>> GetActivitiesByPersonnelAsync(int personnelId, int? year = null)
        {
            var activities = await _personnelActivityRepository.GetByPersonnelIdAsync(personnelId, year);
            return activities;
        }

        public async Task<PersonnelActivity?> InsertAsync(PersonnelActivity data)
        {
            var personnel = await _personnelRepository.GetByIdAsync(data.PersonnelId ?? 0);
            if (personnel == null) throw new Exception("No Personnel found");

            var activityType = await _activityTypeRepository.GetByIdAsync(data.ActivityTypeId ?? 0);

          
            decimal totalDays = _dayUtility.CountDays(data.StartDate, data.EndDate, activityType.IsMandatoryLeave);

           
            var credits = await _personnelRepository.GetPersonnelCreditsAsync(
                personnelId: data.PersonnelId ?? 0,
                activityTypeId: data.ActivityTypeId ?? 0,
                date:data.StartDate
            );

            decimal remainingCredits = credits?.FirstOrDefault()?.RemainingCredits??0;
            decimal remainingApprovedCredits = remainingCredits - totalDays;

            await _emailSenderUtility.SendEmailAsync(personnel?.Email, $"Leave Request Received (Pending) - {activityType?.ActivityTypeName}",
                $@"<div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #f0f0f0; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.05);"">
        <div style=""background-color: #faad14; padding: 30px; text-align: center;"">
            <div style=""background: white; width: 60px; height: 60px; border-radius: 50%; display: inline-block; line-height: 60px; margin-bottom: 15px;"">
                <span style=""color: #faad14; font-size: 30px;"">⏳</span>
            </div>
            <h2 style=""margin: 0; color: #ffffff; font-size: 22px; letter-spacing: 0.5px;"">Pending Approval</h2>
        </div>
        <div style=""padding: 30px; background-color: #ffffff;"">
            <p style=""font-size: 16px; color: #333;"">Hello <strong>{personnel?.Rank?.RankCode} {personnel?.FirstName} {personnel?.MiddleName} {personnel?.LastName} {personnel?.SerialNumber}</strong>,</p>
            <p style=""font-size: 15px; color: #666; line-height: 1.5;"">Your leave request for <strong>{activityType?.ActivityTypeName}</strong> has been received and is currently <strong>awaiting review</strong> by the designated authority.</p>
            
            <div style=""margin: 25px 0; border-top: 2px solid #fcf4e6; border-bottom: 2px solid #fcf4e6; padding: 15px 0;"">
                <h4 style=""margin: 0 0 10px 0; color: #d48806; text-transform: uppercase; font-size: 12px; letter-spacing: 1px;"">Request Summary</h4>
                <table style=""width: 100%; font-size: 15px; color: #444;"">
                    <tr>
                        <td style=""padding: 5px 0;""><strong>Title:</strong></td>
                        <td style=""padding: 5px 0; text-align: right;"">{data?.Title ?? "N/A"}</td>
                    </tr>
                    <tr>
                        <td style=""padding: 5px 0; vertical-align: top;""><strong>Reason for Action:</strong></td>
                        <td style=""padding: 5px 0; text-align: right; line-height: 1.6; color: #444;"">
                            {data?.Reason?.Replace("\n", "<br />")}
                        </td>
                    </tr>
                    <tr>
                        <td style=""padding: 5px 0;""><strong>Period:</strong></td>
                        <td style=""padding: 5px 0; text-align: right;"">{data.StartDate?.ToString("dd MMMM yyyy").ToUpper()} — {data.EndDate?.ToString("dd MMMM yyyy").ToUpper()}</td>
                    </tr>
                    <tr>
                        <td style=""padding: 5px 0;""><strong>Total Duration:</strong></td>
                        <td style=""padding: 5px 0; text-align: right;"">{totalDays} Day(s)</td>
                    </tr>
                </table>
            </div>

            <div style=""background-color: #fffbe6; padding: 15px; border-radius: 8px; margin-bottom: 20px; border: 1px solid #ffe58f;"">
                <table style=""width: 100%;"">
                    <tr>
                        <td style=""font-size: 13px; color: #856404;"">
                            <strong>Estimated Balance:</strong><br/>
                            Remaining credits if this request is approved:
                        </td>
                        <td style=""font-size: 20px; color: #faad14; font-weight: bold; text-align: right; vertical-align: middle;"">
                            {remainingApprovedCredits}
                        </td>
                    </tr>
                </table>
            </div>

            <div style=""background-color: #f9f9f9; padding: 15px; border-radius: 8px; margin-bottom: 20px; border: 1px dashed #d9d9d9;"">
                <table style=""width: 100%;"">
                    <tr>
                        <td style=""font-size: 14px; color: #666;"">Remaining Leave Credits:</td>
                        <td style=""font-size: 18px; color: #1677ff; font-weight: bold; text-align: right;"">{remainingCredits}</td>
                    </tr>
                </table>
            </div>

            <p style=""font-size: 14px; color: #888; background-color: #f9f9f9; padding: 15px; border-radius: 6px; border-left: 4px solid #faad14;"">
                <strong>What's Next?</strong> Your request is now in the queue for approval. You will be notified automatically via email once the status has been updated.
            </p>
        </div>
        <div style=""background-color: #fafafa; padding: 20px; text-align: center; border-top: 1px solid #f0f0f0;"">
            <p style=""margin: 0; font-size: 12px; color: #999;"">Thank you for using the <strong>RTC Aurora E-Monitoring</strong>.</p>
            <p style=""margin: 5px 0 0 0; font-size: 11px; color: #bbb;"">© 2026 RTC Aurora E-Monitoring. All rights reserved.</p>
        </div>
    </div>"
            );

            data.Status = "Pending Approval";
            return await _personnelActivityRepository.InsertAsync(data);
        }

       public async Task<PersonnelActivity?> ApproveAsync(int? personnelActivityId, string? remarks)
{
    var data = await _personnelActivityRepository.GetByIdAsync(personnelActivityId ?? 0);
    if (data == null) throw new Exception("No Activity found");

    var personnel = await _personnelRepository.GetByIdAsync(data.PersonnelId ?? 0);
    if (personnel == null) throw new Exception("No Personnel found");

    var activityType = await _activityTypeRepository.GetByIdAsync(data.ActivityTypeId ?? 0);
    
    // Calculate days for this specific request
    decimal totalDays = _dayUtility.CountDays(data.StartDate, data.EndDate, activityType.IsMandatoryLeave);

    // Fetch the credits as they stand CURRENTLY (before this approval is finalized in DB)
    var credits = await _personnelRepository.GetPersonnelCreditsAsync(
       personnelId: data.PersonnelId ?? 0,
       activityTypeId: data.ActivityTypeId ?? 0,
       date: data.StartDate
    );

    // Calculate the new balance
    decimal currentBalance = credits?.FirstOrDefault()?.RemainingCredits ?? 0;
    decimal remainingCredits = currentBalance - totalDays;

    await _emailSenderUtility.SendEmailAsync(personnel?.Email, $"Leave Request Approved - {activityType?.ActivityTypeName}",
$@"<div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #f0f0f0; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.05);"">
    <div style=""background-color: #52c41a; padding: 30px; text-align: center;"">
        <div style=""background: white; width: 60px; height: 60px; border-radius: 50%; display: inline-block; line-height: 60px; margin-bottom: 15px;"">
            <span style=""color: #52c41a; font-size: 30px;"">✔</span>
        </div>
        <h2 style=""margin: 0; color: #ffffff; font-size: 22px; letter-spacing: 0.5px;"">Request Approved Successfully</h2>
    </div>
    <div style=""padding: 30px; background-color: #ffffff;"">
        <p style=""font-size: 16px; color: #333;"">Hello <strong>{personnel?.Rank?.RankCode} {personnel?.FirstName} {personnel?.MiddleName} {personnel?.LastName} {personnel?.SerialNumber}</strong>,</p>
        <p style=""font-size: 15px; color: #666; line-height: 1.5;"">Good news! Your leave request for <strong>{activityType?.ActivityTypeName}</strong> has been <strong>approved</strong>. The system has been updated accordingly.</p>
        
        <div style=""margin: 25px 0; border-top: 2px solid #f6ffed; border-bottom: 2px solid #f6ffed; padding: 15px 0;"">
            <h4 style=""margin: 0 0 10px 0; color: #389e0d; text-transform: uppercase; font-size: 12px; letter-spacing: 1px;"">Final Summary</h4>
            <table style=""width: 100%; font-size: 15px; color: #444;"">
                 <tr>
                    <td style=""padding: 5px 0;""><strong>Title:</strong></td>
                    <td style=""padding: 5px 0; text-align: right;"">{data?.Title ?? "N/A"}</td>
                </tr>
                <tr>
                    <td style=""padding: 5px 0; vertical-align: top;""><strong>Reason for Action:</strong></td>
                    <td style=""padding: 5px 0; text-align: right; line-height: 1.6; color: #444;"">
                        {data?.Reason?.Replace("\n", "<br />")}
                    </td>
                </tr>
                <tr>
                    <td style=""padding: 5px 0;""><strong>Period:</strong></td>
                    <td style=""padding: 5px 0; text-align: right;"">{data.StartDate?.ToString("dd MMMM yyyy").ToUpper()} — {data.EndDate?.ToString("dd MMMM yyyy").ToUpper()}</td>
                </tr>
                <tr>
                    <td style=""padding: 5px 0;""><strong>Total Duration:</strong></td>
                    <td style=""padding: 5px 0; text-align: right;"">{totalDays} Day(s)</td>
                </tr>
                <tr>
                     <td style=""padding: 5px 0; vertical-align: top;""><strong>Approver Remarks:</strong></td>
                      <td style=""padding: 5px 0; text-align: right; color: #237804; font-weight: 600;"">{remarks ?? "N/A"}</td>
                </tr>
            </table>
        </div>

        <div style=""background-color: #f0f5ff; border: 1px solid #adc6ff; padding: 15px; border-radius: 8px; margin-bottom: 20px;"">
            <table style=""width: 100%;"">
                <tr>
                    <td style=""font-size: 14px; color: #1d39c4;""><strong>Updated Leave Balance:</strong></td>
                    <td style=""font-size: 18px; color: #2f54eb; font-weight: bold; text-align: right;"">{remainingCredits} Day(s)</td>
                </tr>
            </table>
        </div>

        <p style=""font-size: 14px; color: #389e0d; background-color: #f6ffed; padding: 15px; border-radius: 6px; border-left: 4px solid #52c41a;"">
            <strong>Notice:</strong> Please coordinate with your department for the turnover of duties before your leave starts. Have a restful break!
        </p>
    </div>
    <div style=""background-color: #fafafa; padding: 20px; text-align: center; border-top: 1px solid #f0f0f0;"">
        <p style=""margin: 0; font-size: 12px; color: #999;"">Thank you for using the <strong>RTC Aurora E-Monitoring</strong>.</p>
        <p style=""margin: 5px 0 0 0; font-size: 11px; color: #bbb;"">© 2026 RTC Aurora E-Monitoring. All rights reserved.</p>
    </div>
</div>"
    );

    data.Remarks = remarks;
    data.IsFullyApproved = true;
    data.Status = _personnelActivityRepository.GetActivityStatus(data.StartDate, data.EndDate, data.Status);
    
    return await _personnelActivityRepository.UpdateAsync(data);
}

        public async Task<PersonnelActivity?> DeclineAsync(int? personnelActivityId, string? remarks)
        {
            var data = await _personnelActivityRepository.GetByIdAsync(personnelActivityId ?? 0);
            if (data == null) throw new Exception("No Activity found");

            var personnel = await _personnelRepository.GetByIdAsync(data.PersonnelId ?? 0);
            if (personnel == null) throw new Exception("No Personnel found");

            var activityType = await _activityTypeRepository.GetByIdAsync(data.ActivityTypeId ?? 0);
            decimal totalDays = _dayUtility.CountDays(data.StartDate, data.EndDate, activityType.IsMandatoryLeave);

            // Update the email to "Decline" theme
            await _emailSenderUtility.SendEmailAsync(personnel?.Email, $"Leave Request Declined - {activityType?.ActivityTypeName}",
                $@"<div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #f0f0f0; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.05);"">
            
            <div style=""background-color: #ff4d4f; padding: 30px; text-align: center;"">
                <div style=""background: white; width: 60px; height: 60px; border-radius: 50%; display: inline-block; line-height: 60px; margin-bottom: 15px;"">
                    <span style=""color: #ff4d4f; font-size: 30px;"">✘</span>
                </div>
                <h2 style=""margin: 0; color: #ffffff; font-size: 22px; letter-spacing: 0.5px;"">Request Declined</h2>
            </div>

            <div style=""padding: 30px; background-color: #ffffff;"">
                <p style=""font-size: 16px; color: #333;"">Hello <strong>{personnel?.Rank?.RankCode} {personnel?.FirstName} {personnel?.LastName}</strong>,</p>
                <p style=""font-size: 15px; color: #666; line-height: 1.5;"">We regret to inform you that your leave request for <strong>{activityType?.ActivityTypeName}</strong> has been <strong>Declined</strong> by the approving authority.</p>
                
                <div style=""margin: 25px 0; border-top: 2px solid #fff1f0; border-bottom: 2px solid #fff1f0; padding: 15px 0;"">
                    <h4 style=""margin: 0 0 10px 0; color: #cf1322; text-transform: uppercase; font-size: 12px; letter-spacing: 1px;"">Request Details</h4>
                    <table style=""width: 100%; font-size: 14px; color: #444;"">
                    <tr>
                        <td style=""padding: 5px 0;""><strong>Title:</strong></td>
                     <td style=""padding: 5px 0; text-align: right;"">{data?.Title ?? "N/A"}</td>
                    </tr>
                   <tr>
    <td style=""padding: 5px 0; vertical-align: top;""><strong>Reason for Action:</strong></td>
    <td style=""padding: 5px 0; text-align: right; line-height: 1.6; color: #444;"">
        {data?.Reason?.Replace("\n", "<br />")}
    </td>
</tr>
                         <tr>
                            <td style=""padding: 5px 0;""><strong>Period:</strong></td>
                            <td style=""padding: 5px 0; text-align: right;"">{data.StartDate?.ToString("dd MMM yyyy").ToUpper()} — {data.EndDate?.ToString("dd MMM yyyy").ToUpper()}</td>
                        </tr>
                        <tr>
                            <td style=""padding: 5px 0;""><strong>Total Duration:</strong></td>
                            <td style=""padding: 5px 0; text-align: right;"">{totalDays} Day(s)</td>
                        </tr>
                        <tr>
                            <td style=""padding: 5px 0; vertical-align: top;""><strong>Remarks:</strong></td>
                            <td style=""padding: 5px 0; text-align: right; color: #cf1322; font-weight: 600;"">{remarks ?? "N/A"}</td>
                        </tr>
                    </table>
                </div>

                <p style=""font-size: 14px; color: #888; background-color: #fff1f0; padding: 15px; border-radius: 6px; border-left: 4px solid #ff4d4f;"">
                    <strong>Next Step:</strong> If you believe this was an error or would like to provide more information, please coordinate with your immediate supervisor or resubmit a new request with the required adjustments.
                </p>
            </div>

            <div style=""background-color: #fafafa; padding: 20px; text-align: center; border-top: 1px solid #f0f0f0;"">
                <p style=""margin: 0; font-size: 12px; color: #999;"">This is an automated notification from <strong>RTC Aurora E-Monitoring</strong>.</p>
                <p style=""margin: 5px 0 0 0; font-size: 11px; color: #bbb;"">© 2026 RTC Aurora E-Monitoring. All rights reserved.</p>
            </div>
        </div>"
            );

            data.Remarks = remarks;
            data.Status = "Declined";
            return await _personnelActivityRepository.UpdateAsync(data);
        }
        public async Task<PersonnelActivity?> UpdateAsync(PersonnelActivity data)
        {
            if (data.Status != "Pending Approval" && data.Status == "Suspended")
            {
                data.Status = _personnelActivityRepository.GetActivityStatus(data.StartDate, data.EndDate, data.Status);
            }

            return await _personnelActivityRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<PersonnelActivityDTO>> GetAllAsync(PersonnelActivity? filter = null)
        {
            IEnumerable<PersonnelActivity> personnelActivities = await _personnelActivityRepository
                .GetAllAsync(filter, x => x.Personnel.Rank, x => x.ActivityType,x=>x.Personnel);

            var sortedData = personnelActivities
                .OrderBy(x => x.Status != "Pending Approval")
                .ThenByDescending(x => x.EndDate)
                .ThenBy(x => x.Personnel?.Rank?.RankLevel);

            return _mapper.MapList<PersonnelActivity, PersonnelActivityDTO>(sortedData);
        }

        public async Task<PersonnelActivity?> GetByIdAsync(int id)
        {
            return await _personnelActivityRepository.GetByIdAsync(id);
        }

        public async Task<PersonnelActivity?> DeleteByIdAsync(int id)
        {
            return await _personnelActivityRepository.DeleteByIdAsync(id);
        }
        public async Task<IEnumerable<PersonnelActivity>> BulkInsertAsync(List<PersonnelActivity> data)
        {
            return await _personnelActivityRepository.BulkInsertAsync(data);
        }
        public async Task<IEnumerable<PersonnelActivity>> BulkUpdateAsync(List<PersonnelActivity> data)
        {
            return await _personnelActivityRepository.BulkUpdateAsync(data);
        }
        public async Task<IEnumerable<PersonnelActivity>> BulkUpsertAsync(List<PersonnelActivity> data)
        {
            return await _personnelActivityRepository.BulkUpsertAsync(data);
        }
        public async Task<IEnumerable<PersonnelActivity>> BulkMergeAsync(List<PersonnelActivity> data)
        {
            return await _personnelActivityRepository.BulkMergeAsync(data);
        }

        public async Task SendWarningEmailAsync(PersonnelActivity activity)
        {
            string emailLayout = GenerateLeaveReminderEmail(activity);

            string subject = $"⏳ FINAL NOTICE: Leave Activity Ending ({activity?.EndDate:dd MMM yyyy})";

           
            await _emailSenderUtility.SendEmailAsync(
                activity?.Personnel?.Email,
                subject,
                emailLayout
            );
        }
        private string GenerateLeaveReminderEmail(PersonnelActivity activity)
        {
            var personnel = activity?.Personnel;
            var activityType = activity?.ActivityType;

            int totalDays = (activity.EndDate.HasValue && activity.StartDate.HasValue)
                ? (activity.EndDate.Value - activity.StartDate.Value).Days + 1
                : 0;

            string formattedReason = activity.Reason?.Replace("\n", "<br>") ?? "No reason provided";

            return $@"
<div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #f0f0f0; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.05);"">
    
    <div style=""background-color: #faad14; padding: 30px; text-align: center;"">
        <div style=""background: white; width: 60px; height: 60px; border-radius: 50%; display: inline-block; line-height: 60px; margin-bottom: 15px;"">
            <span style=""color: #faad14; font-size: 30px;"">⚠️</span>
        </div>
        <h2 style=""margin: 0; color: #ffffff; font-size: 22px; letter-spacing: 0.5px;"">{activityType?.ActivityTypeName} Activity Reminder</h2>
    </div>

    <div style=""padding: 30px; background-color: #ffffff;"">
        <p style=""font-size: 16px; color: #333;"">Hello <strong>{personnel?.Rank?.RankCode} {personnel?.FirstName} {personnel?.LastName} {personnel?.SerialNumber}</strong>,</p>
        
        <p style=""font-size: 15px; color: #666; line-height: 1.5;"">
            This is an automated reminder that your scheduled <strong>{activityType?.ActivityTypeName}</strong> is reaching its end date within the next 24 hours.
        </p>

        <div style=""background-color: #fff1f0; border: 1px dashed #ff4d4f; padding: 20px; border-radius: 12px; text-align: center; margin: 20px 0;"">
            <span style=""color: #cf1322; font-size: 18px; font-weight: 800; display: block;"">LESS THAN 24 HOURS REMAINING</span>
            <span style=""color: #5f6368; font-size: 13px;"">Scheduled End Date: <strong>{activity?.EndDate:dd MMMM yyyy}</strong></span>
        </div>

        <div style=""margin: 25px 0; border-top: 2px solid #fff7e6; border-bottom: 2px solid #fff7e6; padding: 15px 0;"">
            <h4 style=""margin: 0 0 10px 0; color: #d48806; text-transform: uppercase; font-size: 12px; letter-spacing: 1px;"">Activity Summary</h4>
            <table style=""width: 100%; font-size: 15px; color: #444;"">
                 <tr>
                    <td style=""padding: 8px 0;""><strong>Title:</strong></td>
                    <td style=""padding: 8px 0; text-align: right;"">{activity?.Title ?? "N/A"}</td>
                </tr>
                <tr>
                    <td style=""padding: 8px 0; vertical-align: top;""><strong>Reason:</strong></td>
                    <td style=""padding: 8px 0; text-align: right; line-height: 1.6; color: #444;"">
                        {formattedReason}
                    </td>
                </tr>
                <tr>
                    <td style=""padding: 8px 0;""><strong>Full Period:</strong></td>
                    <td style=""padding: 8px 0; text-align: right;"">{activity.StartDate?.ToString("dd MMM yyyy")} — {activity.EndDate?.ToString("dd MMM yyyy")}</td>
                </tr>
                <tr>
                    <td style=""padding: 8px 0;""><strong>Total Duration:</strong></td>
                    <td style=""padding: 8px 0; text-align: right;"">{totalDays} Day(s)</td>
                </tr>
            </table>
        </div>

        <p style=""font-size: 14px; color: #855800; background-color: #fffbe6; padding: 15px; border-radius: 6px; border-left: 4px solid #faad14;"">
            <strong>Important:</strong> Please ensure all pending tasks are handed over and you are prepared to resume duties as scheduled following the expiration of this leave.
        </p>
    </div>

    <div style=""background-color: #fafafa; padding: 20px; text-align: center; border-top: 1px solid #f0f0f0;"">
        <p style=""margin: 0; font-size: 12px; color: #999;"">Sent by <strong>RTCA E-Monitoring Automated Service</strong></p>
        <p style=""margin: 5px 0 0 0; font-size: 11px; color: #bbb;"">Server Time: {DateTime.Now:dd MMM yyyy • h:mm tt}</p>
        <p style=""margin: 5px 0 0 0; font-size: 11px; color: #bbb;"">© 2026 RTC Aurora. All rights reserved.</p>
    </div>
</div>";
        }
    }
}