using System;
using System.Threading;
using System.Threading.Tasks;
using BadGateway.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BadGateway.HostedServices
{
    public class MigratorHostedService : IHostedService
    {
        private readonly ILogger<MigratorHostedService> logger;
        private readonly IServiceProvider services;

        public MigratorHostedService(ILogger<MigratorHostedService> logger, IServiceProvider services)
        {
            this.logger = logger;
            this.services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Db Migrator started...");

            using (var scope = services.CreateScope())
            {
                var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await appDbContext.Database.MigrateAsync(cancellationToken);
            }

            this.logger.LogInformation("Db Migrator finished...");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
