
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationContexts;
using Microsoft.EntityFrameworkCore;
using Models;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;
using Repositories.Interfaces;
using Services.Interfaces;
using Utilities.Classes;
using Utilities.Interfaces;
using static System.Net.WebRequestMethods;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        private readonly IApprovalProccessRepository _approvalProccessRepository;
        private readonly IWorkflowStepsRepository _workflowStepsRepository;
        private readonly ApplicationContext _context = new ApplicationContext();
        public PersonnelActivityService(IPersonnelActivityRepository personnelActivityRepository, IPersonnelRepository personnelRepository, IAutoMapperUtility mapper, IEmailSenderUtility emailSenderUtility, IActivityTypeRepository activityTypeRepository, IDayUtility dayUtility, IApprovalProccessRepository approvalProccessRepository, IWorkflowStepsRepository workflowStepsRepository)
        {
            _personnelActivityRepository = personnelActivityRepository;
            _personnelRepository = personnelRepository;
            _mapper = mapper;
            _emailSenderUtility = emailSenderUtility;
            _activityTypeRepository = activityTypeRepository;
            _dayUtility = dayUtility;
            _approvalProccessRepository = approvalProccessRepository;
            _workflowStepsRepository = workflowStepsRepository;
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
                date: data.StartDate
            );

            decimal remainingCredits = credits?.FirstOrDefault()?.RemainingCredits ?? 0;
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



            var approvalProccess = new ApprovalProccess
            {
                CurrentStage = personnel?.Rank?.RankCategory?.Name == "Officer" ? 3 : 1,
            };

          
            var savedApproval = await _approvalProccessRepository.InsertAsync(approvalProccess);

    
            data.ApprovalProccessId = savedApproval.Id; 
            data.PersonnelType = personnel?.Rank?.RankCategory?.Name ?? "Non-Officer";
            data.Status = "Pending Approval";

            // 3. Save the main Activity
            var activity = await _personnelActivityRepository.InsertAsync(data);

            return activity;
        }

            public async Task<PersonnelActivity?> ApproveAsync(int? personnelActivityId, string? remarks)
            {
                var data = await _context.PersonnelActivity.Include(pa => pa.ApprovalProccess).FirstOrDefaultAsync(pa => pa.PersonnelActivityId == personnelActivityId);
                //var data = await _personnelActivityRepository.GetByIdAsync(personnelActivityId ?? 0);

                if (data == null) throw new Exception("No Activity found");

            
                var personnel = await _personnelRepository.GetByIdAsync(data.PersonnelId ?? 0);
                if (personnel == null) throw new Exception("No Personnel found");

                var activityType = await _activityTypeRepository.GetByIdAsync(data.ActivityTypeId ?? 0);
               // var approver = await _context.Personnel.FirstOrDefaultAsync(p => p.PersonnelId == approverId);
               //var nonOfficerWorkSteps = await _context.WorkflowSteps
               // .Include(w=>w.Role).Where(w => w.RankCategoryId == 2).ToListAsync();


                decimal totalDays = _dayUtility.CountDays(data.StartDate, data.EndDate, activityType.IsMandatoryLeave);

                var credits = await _personnelRepository.GetPersonnelCreditsAsync(
                   personnelId: data.PersonnelId ?? 0,
                   activityTypeId: data.ActivityTypeId ?? 0,
                   date: data.StartDate
                );

                // Calculate the new balance
                decimal currentBalance = credits?.FirstOrDefault()?.RemainingCredits ?? 0;
                decimal remainingCredits = currentBalance - totalDays;
                
//            await _emailSenderUtility.SendEmailAsync(personnel?.Email,
//  isApproved ? $"Request Approved - {activityType?.ActivityTypeName}" : $"Request Disapproved - {activityType?.ActivityTypeName}",
//  $@"<div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #f0f0f0; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.05);"">
    
//    <div style=""background-color: {(isApproved ? "#52c41a" : "#ff4d4f")}; padding: 30px; text-align: center;"">
//        <div style=""background: white; width: 60px; height: 60px; border-radius: 50%; display: inline-block; line-height: 60px; margin-bottom: 15px;"">
//            <span style=""color: {(isApproved ? "#52c41a" : "#ff4d4f")}; font-size: 30px;"">{(isApproved ? "✔" : "✘")}</span>
//        </div>
//        <h2 style=""margin: 0; color: #ffffff; font-size: 22px; letter-spacing: 0.5px;"">Request {(isApproved ? "Approved Successfully" : "Disapproved")}</h2>
//    </div>

//    <div style=""padding: 30px; background-color: #ffffff;"">
//        <p style=""font-size: 16px; color: #333;"">Hello <strong>{personnel?.Rank?.RankCode} {personnel?.FirstName} {personnel?.LastName}</strong>,</p>
//        <p style=""font-size: 15px; color: #666; line-height: 1.5;"">
//            Your leave request for <strong>{activityType?.ActivityTypeName}</strong> has been <strong>{(isApproved ? "approved" : "disapproved")}</strong>.
//        </p>

//        <div style=""margin: 30px 0; text-align: center; background-color: #fafafa; padding: 20px; border-radius: 10px;"">
//            <h4 style=""margin: 0 0 15px 0; color: #8c8c8c; font-size: 10px; text-transform: uppercase; letter-spacing: 1px;"">Approval Journey Status</h4>
//            <table style=""width: 100%; border-collapse: collapse;"">
//                <tr>
//                    {GenerateStepHtml(1, nonOfficerWorkSteps.FirstOrDefault(n=>n.StepNumber == 1), data.ApprovalProccess.StageOneIsApprove, data.ApprovalProccess.CurrentStage)}
//                    {GenerateStepHtml(2, nonOfficerWorkSteps.FirstOrDefault(n=>n.StepNumber == 2), data.ApprovalProccess.StageTwoIsApprove, data.ApprovalProccess.CurrentStage)}
//                    {GenerateStepHtml(3, nonOfficerWorkSteps.FirstOrDefault(n=>n.StepNumber == 3), data.ApprovalProccess.StageThreeIsApprove, data.ApprovalProccess.CurrentStage)}
//                    {GenerateStepHtml(4, nonOfficerWorkSteps.FirstOrDefault(n=>n.StepNumber == 4), data.ApprovalProccess.StageFourIsApprove, data.ApprovalProccess.CurrentStage)}
//                </tr>
//            </table>
//        </div>

//        <div style=""margin: 25px 0; border-top: 2px solid #f0f0f0; border-bottom: 2px solid #f0f0f0; padding: 15px 0;"">
//            <h4 style=""margin: 0 0 10px 0; color: #595959; text-transform: uppercase; font-size: 12px; letter-spacing: 1px;"">Final Summary</h4>
//            <table style=""width: 100%; font-size: 15px; color: #444;"">
//                 <tr>
//                    <td style=""padding: 5px 0;""><strong>Title:</strong></td>
//                    <td style=""padding: 5px 0; text-align: right;"">{data?.Title ?? "N/A"}</td>
//                </tr>
//                <tr>
//                    <td style=""padding: 5px 0;""><strong>Period:</strong></td>
//                    <td style=""padding: 5px 0; text-align: right;"">{data.StartDate?.ToString("dd MMM yyyy").ToUpper()} — {data.EndDate?.ToString("dd MMM yyyy").ToUpper()}</td>
//                </tr>
//                <tr>
//                    <td style=""padding: 5px 0;""><strong>Duration:</strong></td>
//                    <td style=""padding: 5px 0; text-align: right;"">{totalDays} Day(s)</td>
//                </tr>
//                <tr>
//                     <td style=""padding: 5px 0; vertical-align: top;""><strong>Remarks:</strong></td>
//                      <td style=""padding: 5px 0; text-align: right; color: {(isApproved ? "#237804" : "#cf1322")}; font-weight: 600;"">{remarks ?? "No remarks provided."}</td>
//                </tr>
//            </table>
//        </div>

//        {(isApproved ? $@"
//        <div style=""background-color: #f0f5ff; border: 1px solid #adc6ff; padding: 15px; border-radius: 8px; margin-bottom: 20px;"">
//            <table style=""width: 100%;"">
//                <tr>
//                    <td style=""font-size: 14px; color: #1d39c4;""><strong>Updated Leave Balance:</strong></td>
//                    <td style=""font-size: 18px; color: #2f54eb; font-weight: bold; text-align: right;"">{remainingCredits} Day(s)</td>
//                </tr>
//            </table>
//        </div>" : "")}

//        <p style=""font-size: 14px; color: #8c8c8c; background-color: #f5f5f5; padding: 15px; border-radius: 6px; text-align: center;"">
//            Thank you for using the <strong>RTC Aurora E-Monitoring System</strong>.
//        </p>
//    </div>
//</div>"
//);
            

            //data.Remarks = remarks;
            //switch (data.ApprovalProccess.CurrentStage)
                //{
                //    case 4:
                //        data.ApprovalProccess.StageFourRemarks = remarks;
                //        data.ApprovalProccess.StageFourIsApprove = true;
                //        data.ApprovalProccess.StageFourId = approver.PersonnelId; // Link the personnel

                //        // Final Stage Logic
                //        data.IsFullyApproved = true;
                //        data.Status = _personnelActivityRepository.GetActivityStatus(data.StartDate, data.EndDate, data.Status);
                //        break;

                //    case 3:
                //        data.ApprovalProccess.StageThreeRemarks = remarks;
                //        data.ApprovalProccess.StageThreeIsApprove = true;
                //        data.ApprovalProccess.StageThreeId = approver.PersonnelId;

                //        data.ApprovalProccess.CurrentStage += 1;
                //        data.Status = "Pending Approval";
                //        break;

                //    case 2:
                //        data.ApprovalProccess.StageTwoRemarks = remarks;
                //        data.ApprovalProccess.StageTwoIsApprove = true;
                //        data.ApprovalProccess.StageTwoId = approver.PersonnelId;

                //        data.ApprovalProccess.CurrentStage += 1;
                //        data.Status = "Pending Approval";
                //        break;

                //    case 1:
                //        data.ApprovalProccess.StageOneRemarks = remarks;
                //        data.ApprovalProccess.StageOneIsApprove = true;
                //        data.ApprovalProccess.StageOneId = approver.PersonnelId;

                //        data.ApprovalProccess.CurrentStage += 1;
                //        data.Status = "Pending Approval";
                //        break;

                //    default:
                //        data.IsFullyApproved = true;
                //        data.Status = _personnelActivityRepository.GetActivityStatus(data.StartDate, data.EndDate, data.Status);
                //        break;
                //}

                return await _personnelActivityRepository.UpdateAsync(data);
            }
     
        public async Task<PersonnelActivity?> DeclineAsync(int? personnelActivityId, string? remarks, int? disapproverId = null)
        {
            var data = await _context.PersonnelActivity
                .Include(pa => pa.ApprovalProccess)
                .FirstOrDefaultAsync(pa => pa.PersonnelActivityId == personnelActivityId);

            if (data == null) throw new Exception("No Activity found");

            // Await the disapprover finding
            var disapprover = await _context.Personnel
                .Include(p => p.Rank)
                .FirstOrDefaultAsync(d => d.PersonnelId == disapproverId);

            var personnel = await _personnelRepository.GetByIdAsync(data.PersonnelId ?? 0);
            var activityType = await _activityTypeRepository.GetByIdAsync(data.ActivityTypeId ?? 0);

            decimal totalDays = _dayUtility.CountDays(data.StartDate, data.EndDate, activityType.IsMandatoryLeave);

            // Get the Disapprover's Full Name and Rank
            string disapproverName = disapprover != null
                ? $"{disapprover.Rank?.RankCode} {disapprover.FirstName} {disapprover.MiddleName} {disapprover.LastName} {disapprover.SerialNumber}"
                : "Approving Authority";

            // Link to your frontend application for the appeal
            string appealUrl = $"https://your-app-url.com/activities/view/{data.PersonnelActivityId}";

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
                <p style=""font-size: 15px; color: #666; line-height: 1.5;"">We regret to inform you that your leave request for <strong>{activityType?.ActivityTypeName}</strong> has been <strong>Declined</strong>.</p>
                
                <div style=""background-color: #fff2f0; border: 1px solid #ffccc7; padding: 12px; border-radius: 8px; margin: 20px 0; text-align: center;"">
                    <span style=""font-size: 12px; color: #ff4d4f; text-transform: uppercase; font-weight: bold;"">Declined By:</span><br/>
                    <strong style=""font-size: 15px; color: #434343;"">{disapproverName}</strong>
                </div>

                <div style=""margin: 25px 0; border-top: 1px solid #f0f0f0; border-bottom: 1px solid #f0f0f0; padding: 15px 0;"">
                    <table style=""width: 100%; font-size: 14px; color: #444;"">
                        <tr>
                            <td style=""padding: 5px 0;""><strong>Period:</strong></td>
                            <td style=""padding: 5px 0; text-align: right;"">{data.StartDate?.ToString("dd MMM yyyy").ToUpper()} — {data.EndDate?.ToString("dd MMM yyyy").ToUpper()}</td>
                        </tr>
                        <tr>
                            <td style=""padding: 5px 0; vertical-align: top;""><strong>Reason for Decline:</strong></td>
                            <td style=""padding: 5px 0; text-align: right; color: #cf1322; font-weight: 600;"">{remarks ?? "No specific reason provided."}</td>
                        </tr>
                    </table>
                </div>

                <div style=""text-align: center; margin: 30px 0;"">
                    <a href=""{appealUrl}"" style=""background-color: #1890ff; color: white; padding: 12px 25px; text-decoration: none; font-weight: bold; border-radius: 6px; display: inline-block; box-shadow: 0 2px 0 rgba(0,0,0,0.045);"">
                        File an Appeal
                    </a>
                </div>

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
        public async Task<PersonnelActivity?> UpdateAsync(PersonnelActivity data)
        {
            if (data.Status != "Pending Approval" && data.Status == "Suspended")
            {
                data.Status = _personnelActivityRepository.GetActivityStatus(data.StartDate, data.EndDate, data.Status);
            }

            return await _personnelActivityRepository.UpdateAsync(data);
        }

        public async Task<IEnumerable<PersonnelActivityDTO>> GetPending(PendingActivity pendingActivity)
        {
            var query = _context.PersonnelActivity
                .Include(p => p.Personnel)
                    .ThenInclude(p => p.Department) // Includes the person's department
                .Include(p => p.Personnel)
                    .ThenInclude(p => p.Rank)
                        .ThenInclude(a => a.RankCategory)
                .Include(p => p.ActivityType)
                .Include(p => p.ApprovalProccess)
                .AsQueryable();

            // Base Stage Filter
            if (pendingActivity.CurrentStage > 0)
            {
                query = query.Where(p => p.ApprovalProccess.CurrentStage == pendingActivity.CurrentStage);
            }

            // Stage 2 Logic: Filter by OIC's managed departments
            if (pendingActivity.CurrentStage == 2 && pendingActivity.OicPersonnelId.HasValue)
            {
                // Find IDs of all departments managed by this OIC
                var managedDepartmentIds = await _context.Department
                    .Where(d => d.OicId == pendingActivity.OicPersonnelId)
                    .Select(d => d.DepartmentId)
                    .ToListAsync();

                // Only show activities where the Personnel belongs to one of those departments
                query = query.Where(p => p.Personnel.DepartmentId.HasValue &&
                                         managedDepartmentIds.Contains(p.Personnel.DepartmentId.Value));
            }

            var personnelActivities = await query.ToListAsync();

            // Sorting Logic
            var sortedData = personnelActivities
                .OrderBy(x => x.Status != "Pending Approval" && x.Status != "Appeal")
                .ThenByDescending(x => x.EndDate)
                .ThenBy(x => x.Personnel?.Rank?.RankLevel);

            return _mapper.MapList<PersonnelActivity, PersonnelActivityDTO>(sortedData);
        }
        public async Task<IEnumerable<PersonnelActivityDTO>> GetAllAsync(PersonnelActivity? filter = null)
        {

            IEnumerable<PersonnelActivity> personnelActivities = await _personnelActivityRepository
                .GetAllAsync(filter);

            var sortedData = personnelActivities
                .OrderBy(x => x.Status != "Pending Approval" && x.Status != "Appeal")
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

        public Task<PersonnelActivity?> ApproveAsync(int? personnelActivityId, string? remarks, int? approverId = null)
        {
            throw new NotImplementedException();
        }

        public Task<PersonnelActivity?> DeclineAsync(int? personnelActivityId, string? remarks)
        {
            throw new NotImplementedException();
        }

        public async Task<PersonnelActivity?> InsertSchoolingAsync(PersonnelActivity data)
        {
            var personnel = await _personnelRepository.GetByIdAsync(data.PersonnelId ?? 0);
            if (personnel == null) throw new Exception("No Personnel found");

            var activityType = await _activityTypeRepository.GetByIdAsync(data.ActivityTypeId ?? 0);

            decimal totalDays = _dayUtility.CountDays(data.StartDate, data.EndDate, activityType?.IsMandatoryLeave);

            await _emailSenderUtility.SendEmailAsync(personnel?.Email, $"Schooling Order - {data?.Title ?? ""}",
                $@"<div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #f0f0f0; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.05);"">
            <div style=""background-color: #52c41a; padding: 30px; text-align: center;"">
                <div style=""background: white; width: 60px; height: 60px; border-radius: 50%; display: inline-block; line-height: 60px; margin-bottom: 15px;"">
                    <span style=""color: #52c41a; font-size: 30px;"">📋</span>
                </div>
                <h2 style=""margin: 0; color: #ffffff; font-size: 22px; letter-spacing: 0.5px;"">Schooling Order Approved</h2>
            </div>
            <div style=""padding: 30px; background-color: #ffffff;"">
                <p style=""font-size: 16px; color: #333;"">Hello <strong>{personnel?.Rank?.RankCode} {personnel?.FirstName} {personnel?.MiddleName} {personnel?.LastName} {personnel?.SerialNumber}</strong>,</p>
                <p style=""font-size: 15px; color: #666; line-height: 1.5;"">This is to officially notify you that your deployment for <strong>{activityType?.ActivityTypeName}</strong> has been **approved and officially ordered**. Please see the training details below:</p>
                
                <div style=""margin: 25px 0; border-top: 2px solid #f6ffed; border-bottom: 2px solid #f6ffed; padding: 15px 0;"">
                    <h4 style=""margin: 0 0 10px 0; color: #389e0d; text-transform: uppercase; font-size: 12px; letter-spacing: 1px;"">Course & Schooling Details</h4>
                    <table style=""width: 100%; font-size: 15px; color: #444;"">
                        <tr>
                            <td style=""padding: 5px 0;""><strong>Course Title:</strong></td>
                            <td style=""padding: 5px 0; text-align: right;"">{data?.Title ?? "N/A"}</td>
                        </tr>
                        <tr>
                            <td style=""padding: 5px 0; vertical-align: top;""><strong>Remarks: </strong></td>
                            <td style=""padding: 5px 0; text-align: right; line-height: 1.6; color: #444;"">
                                {data?.Remarks?.Replace("\n", "<br />")}
                            </td>
                        </tr>
                        <tr>
                            <td style=""padding: 5px 0;""><strong>Inclusive Dates:</strong></td>
                            <td style=""padding: 5px 0; text-align: right;"">{data?.StartDate?.ToString("dd MMMM yyyy").ToUpper()} — {data?.EndDate?.ToString("dd MMMM yyyy").ToUpper()}</td>
                        </tr>
                        <tr>
                            <td style=""padding: 5px 0;""><strong>Total Duration:</strong></td>
                            <td style=""padding: 5px 0; text-align: right;"">{totalDays} Day(s)</td>
                        </tr>
                    </table>
                </div>

            </div>
            <div style=""background-color: #fafafa; padding: 20px; text-align: center; border-top: 1px solid #f0f0f0;"">
                <p style=""margin: 0; font-size: 12px; color: #999;"">Thank you for using the <strong>RTC Aurora E-Monitoring</strong>.</p>
                <p style=""margin: 5px 0 0 0; font-size: 11px; color: #bbb;"">© 2026 RTC Aurora E-Monitoring. All rights reserved.</p>
            </div>
        </div>"
            );

            //var approvalProccess = new ApprovalProccess
            //{
            //    CurrentStage = personnel?.Rank?.RankCategory?.Name == "Officer" ? 3 : 1,
            //};

            //var savedApproval = await _approvalProccessRepository.InsertAsync(approvalProccess);

            //data.ApprovalProccessId = savedApproval.Id;
            data.IsFullyApproved = true;
            data.Days = totalDays;
            data.PersonnelType = personnel?.Rank?.RankCategory?.Name ?? "Non-Officer";
            data.Status = _personnelActivityRepository.GetActivityStatus(data.StartDate, data.EndDate, data.Status);

            // 3. Save the main Activity
            var activity = await _personnelActivityRepository.InsertAsync(data);

            return activity;
        }
    }
}