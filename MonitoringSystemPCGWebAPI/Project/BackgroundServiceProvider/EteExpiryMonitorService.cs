using Models.NonTables;
using Repositories.Interfaces;
using System.Text.Json;
using Utilities.Interfaces;

namespace MonitoringSystemPCGWebAPI.Project.BackgroundServiceProvider
{
    public class EteExpiryMonitorService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public EteExpiryMonitorService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                var now = DateTime.Now;
                var nextRunTime = now.Date.AddDays(1);
                var delay = nextRunTime - now;
                //var delay = TimeSpan.FromMinutes(1);
                try
                {
                    await Task.Delay(delay, stoppingToken);

                    using var scope = _serviceProvider.CreateScope();
                    var personnelRepo = scope.ServiceProvider.GetRequiredService<IPersonnelRepository>();

                    var enlistedEteRecords = await personnelRepo.GetEnlismentETE();
                    var filteredRecords = enlistedEteRecords.Where(c => c.Remarks != "ALREADY SUBMITTED" && c.Remarks != "ACTIVE");

                    foreach (var record in filteredRecords)
                    {
                        double days = record?.ETEDaysRemaining ?? 0;
                        string name = $"{record.Rank?.RankCode} {record.LastName}, {record.FirstName}";

                        if (days < 100 && days > 0 && (days % 25 == 0))
                        {
                            await SendEteNotification(record);
                            Console.WriteLine($"[INFO]: {name} is at {days} days remaining.");
                        }
                    }
                }
                catch (OperationCanceledException) { /* Handle shutdown */ }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in ETE Monitor Service: {ex.Message}");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
        }
        private async Task SendEteNotification(EnlistedPersonnelETE record)
        {
            using var scope = _serviceProvider.CreateScope();
            var _emailSenderUtility = scope.ServiceProvider.GetRequiredService<IEmailSenderUtility>();

            // 1. Dynamic Variables & Calculations
            string fullName = $"{record.Rank?.RankCode} {record.FirstName} {record.MiddleName} {record.LastName} {record.SerialNumber}";
            string serialNumber = record.SerialNumber ?? "N/A";
            double daysLeft = record.ETEDaysRemaining ?? 0;
            string expiryDate = record.NextETE?.ToString("dd MMMM yyyy").ToUpper() ?? "N/A";
            string latestRecord = record.DateOfLatestReEnlistment?.ToString("dd MMMM yyyy").ToUpper() ?? "N/A";
            string yearsService = record.YearsInService?.ToString() ?? "0";
            string adminRemarks = record.Remarks ?? "No specific remarks.";
            string currentDate = DateTime.Now.ToString("dd MMMM yyyy").ToUpper();

            // 2. Term Progress Calculation (Assuming 3-year term = 1095 days)
            double totalTermDays = 1095;
            double percentageUsed = Math.Clamp(Math.Round(((totalTermDays - daysLeft) / totalTermDays) * 100), 0, 100);

            // 3. Dynamic Theme Coloring
            string headerColor = "#059669"; // Default Success (Green)
            if (daysLeft <= 25) headerColor = "#dc2626";      // Critical (Red)
            else if (daysLeft <= 50) headerColor = "#f59e0b"; // Warning (Amber)
            else if (daysLeft <= 100) headerColor = "#2563eb"; // Info (Blue)

            // 4. Enhanced HTML Body with Variables
            string htmlBody = $@"
    <div style=""font-family: 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; max-width: 650px; margin: 20px auto; border: 1px solid #d1d5db; border-radius: 20px; overflow: hidden; box-shadow: 0 20px 40px rgba(0,0,0,0.12); background-color: #ffffff;"">
        
        <div style=""background-color: {headerColor}; padding: 12px 30px; display: flex; align-items: center; justify-content: space-between;"">
            <span style=""color: rgba(255,255,255,0.8); font-size: 11px; font-weight: bold; letter-spacing: 1.5px; text-transform: uppercase;"">RTC Aurora E-Monitoring</span>
        </div>

        <div style=""padding: 40px;"">
            <div style=""margin-bottom: 30px; text-align: center;"">
                <p style=""font-size: 18px; color: #111827; margin: 0; font-weight: 700;"">{fullName}</p>
                <p style=""font-size: 14px; color: #6b7280; margin-top: 5px; text-transform: uppercase; letter-spacing: 1px;"">End of Term of Enlistment (ETE) Notification</p>
            </div>

            <div style=""background-color: #f9fafb; border: 2px solid {headerColor}; border-radius: 24px; padding: 40px 20px; text-align: center; margin-bottom: 35px; position: relative;"">
                <div style=""position: absolute; top: -14px; left: 50%; transform: translateX(-50%); background-color: {headerColor}; color: white; padding: 4px 20px; border-radius: 20px; font-size: 11px; font-weight: bold; text-transform: uppercase; letter-spacing: 1px;"">
                    {(daysLeft <= 25 ? "Immediate Action" : "Critical Deadline")}
                </div>

                <span style=""display: block; font-size: 13px; color: #4b5563; font-weight: bold; text-transform: uppercase; letter-spacing: 2px; margin-bottom: 10px;"">Submission In:</span>
                
                <div style=""margin-bottom: 10px;"">
                    <span style=""font-size: 64px; font-weight: 900; color: {headerColor}; line-height: 1; display: block;"">{daysLeft}</span>
                    <span style=""font-size: 18px; font-weight: 800; color: {headerColor}; text-transform: uppercase; letter-spacing: 4px; display: block; margin-top: -5px;"">Days Remaining</span>
                </div>

                <p style=""font-size: 14px; color: #374151; font-weight: 600; margin: 20px 0 0 0;"">
                    Final date to submit re-enlistment: <span style=""color: #dc2626;"">{expiryDate}</span>
                </p>

                <div style=""margin-top: 25px; width: 80%; margin-left: auto; margin-right: auto;"">
                    <div style=""display: flex; justify-content: space-between; margin-bottom: 8px; font-size: 11px; font-weight: bold; color: #9ca3af;"">
                        <span>TERM PROGRESS </span>
                        <span>{percentageUsed}%</span>
                    </div>
                    <div style=""width: 100%; background-color: #e5e7eb; height: 10px; border-radius: 5px; overflow: hidden;"">
                        <div style=""width: {percentageUsed}%; background-color: {headerColor}; height: 100%; border-radius: 5px;""></div>
                    </div>
                </div>
            </div>

            <div style=""display: grid; grid-template-columns: 1fr 1fr; gap: 15px; margin-bottom: 35px;"">
                <div style=""padding: 15px; border-radius: 12px; background: #ffffff; border: 1px solid #f3f4f6; box-shadow: 0 2px 4px rgba(0,0,0,0.02);"">
                    <span style=""display: block; font-size: 10px; color: #9ca3af; font-weight: bold; text-transform: uppercase;"">Latest Re-Enlistment</span>
                    <span style=""font-size: 14px; color: #111827; font-weight: 600;"">{latestRecord}</span>
                </div>
                <div style=""padding: 15px; border-radius: 12px; background: #ffffff; border: 1px solid #f3f4f6; box-shadow: 0 2px 4px rgba(0,0,0,0.02);"">
                    <span style=""display: block; font-size: 10px; color: #9ca3af; font-weight: bold; text-transform: uppercase;"">Service Length</span>
                    <span style=""font-size: 14px; color: #111827; font-weight: 600;"">{yearsService} Years</span>
                </div>
                <div style=""padding: 15px; grid-column: span 2; border-radius: 12px; background: #fff1f0; border: 1px solid #ffa39e;"">
                    <span style=""display: block; font-size: 10px; color: #cf1322; font-weight: bold; text-transform: uppercase;"">Administrative Remarks</span>
                    <span style=""font-size: 14px; color: #111827; font-weight: 600;"">{adminRemarks}</span>
                </div>
            </div>
        </div>

        <div style=""background-color: #111827; padding: 30px; text-align: center; color: #9ca3af;"">
            <p style=""margin: 0; font-size: 13px; color: #ffffff; font-weight: bold;"">RTC AURORA E-MONITORING</p>
            <div style=""border-top: 1px solid #374151; padding-top: 15px; font-size: 9px;"">
                This is an automated official notification. Generated on {currentDate}.
            </div>
        </div>
    </div>";

            await _emailSenderUtility.SendEmailAsync(
                record.Email,
                $"ETE Notification: {daysLeft} Days Remaining",
                htmlBody
            );
        }
    }
}
