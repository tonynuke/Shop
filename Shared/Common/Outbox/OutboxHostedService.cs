using System;
using System.Threading;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Common.Outbox
{
    public class OutboxHostedService : BackgroundService
    {
        /// <summary>
        /// Delay.
        /// </summary>
        private const int MillisecondsDelay = 5 * 1000;

        private readonly ILogger<OutboxHostedService> _logger;
        private readonly DbContext _dbContext;

        public OutboxHostedService(ILogger<OutboxHostedService> logger, DbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox hosted service running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoWork(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Fatal error");
                }

                await Task.Delay(MillisecondsDelay, stoppingToken);
            }
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            //using (var scope = _serviceProvider.CreateScope())
            //{
            //    var scheduler = scope.ServiceProvider.GetRequiredService<ISynchronizationsScheduler>();
            //    await scheduler.QueueSynchronizations();
            //}
        }
    }
}