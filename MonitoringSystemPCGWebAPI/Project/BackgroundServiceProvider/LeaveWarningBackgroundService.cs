using ApplicationContexts;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace MonitoringSystemPCGWebAPI.Project.BackgroundServiceProvider
{
    public class LeaveWarningBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<LeaveWarningBackgroundService> _logger;

        public LeaveWarningBackgroundService(IServiceScopeFactory scopeFactory, ILogger<LeaveWarningBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Leave Warning Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                        var personnelService = scope.ServiceProvider.GetRequiredService<IPersonnelActivityService>();

                        var now = DateTime.Now;
                        var warningWindow = now.AddHours(24);

                        var upcomingExpirations = await context.PersonnelActivity
                            .Include(pa => pa.Personnel)
                                .ThenInclude(p => p.Rank)
                            .Include(pa => pa.ActivityType)
                            .Where(pa => pa.EndDate <= warningWindow 
                                      && pa.EndDate >= now         
                                      && pa.IsWarningSent != true
                                      && pa.IsFullyApproved == true)
                            .ToListAsync(stoppingToken);

                        if (upcomingExpirations.Any())
                        {
                            foreach (var activity in upcomingExpirations)
                            {
                                try
                                {
                                    await personnelService.SendWarningEmailAsync(activity);

                                    activity.IsWarningSent = true;

                                    context.PersonnelActivity.Update(activity);
                                    await context.SaveChangesAsync(stoppingToken);

                                    _logger.LogInformation($"Warning sent to {activity.Personnel?.LastName} (ID: {activity.PersonnelActivityId})");
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, $"Failed to process Activity ID: {activity.PersonnelActivityId}");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Database connection error in LeaveWarningBackgroundService. Retrying in 30 minutes.");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }

            _logger.LogWarning("Leave Warning Background Service has been requested to stop.");
        }
    }
}