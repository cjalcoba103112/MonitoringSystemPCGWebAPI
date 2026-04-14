using Microsoft.Extensions.Hosting;
using Repositories.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringSystemPCGWebAPI.Project.BackgroundServiceProvider
{
    public class ActivityStatusUpdater : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval;

        public ActivityStatusUpdater(IServiceProvider serviceProvider, TimeSpan interval)
        {
            _serviceProvider = serviceProvider;
            _interval = interval;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var personnelActivityRepo = scope.ServiceProvider.GetRequiredService<IPersonnelActivityRepository>();

                    await UpdateActivityStatuses(personnelActivityRepo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating activity statuses: {ex.Message}");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task UpdateActivityStatuses(IPersonnelActivityRepository _personnelActivityRepository)
        {
            var today = DateTime.UtcNow.Date;

            var activities = await _personnelActivityRepository.GetAllAsync();


            var result = activities.ToList().Where(c => c.Status != "Suspended" && c.Status != "Inactive" && c.Status != "Declined" && c.Status !="Pending Approval");

            foreach (var r in result.ToList())
            {
                if (r.Status == "Suspended" || r.Status == "Declined" || r.Status == "Pending Approval") return;

                if (r.EndDate.HasValue && today > r.EndDate.Value.Date)
                    r.Status = "Inactive";
                else if (r.StartDate.HasValue && r.StartDate.Value.Date <= today &&
                         r.EndDate.HasValue && today <= r.EndDate.Value.Date)
                    r.Status = "Ongoing";
                else
                    r.Status = "Scheduled";
                await _personnelActivityRepository.UpdateAsync(r);
            }


        }
    }
}