using LibraryManagmentSystem.Data;
using System;

namespace LibraryManagementSystem.API.Services
{
    public class CleanupService : BackgroundService
    {
        private readonly ILogger<CleanupService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        public CleanupService(ILogger<CleanupService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();

                    var cutoff = DateTime.UtcNow.AddDays(-15);
                    var oldReturns = context.ReturnedRentals
                        .Where(r => r.ReturnedAt < cutoff);

                    context.ReturnedRentals.RemoveRange(oldReturns);
                    await context.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // run daily
            }
        }
    }
}
