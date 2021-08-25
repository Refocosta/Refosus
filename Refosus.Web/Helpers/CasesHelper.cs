using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Refosus.Web.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Refosus.Web.Helpers
{
    public class CasesHelper : IHostedService, IDisposable
    {
        private ILogger<CasesHelper> logger;
        private Timer timer;

        public IServiceScopeFactory serviceScopeFactory;

        public CasesHelper(ILogger<CasesHelper> logger, IServiceScopeFactory _serviceScopeFactory)
        {
            this.logger = logger;
            this.serviceScopeFactory = _serviceScopeFactory;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(12));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                DayOfWeek day = DateTime.Now.DayOfWeek;
                var now = System.DateTime.Now.ToUniversalTime();
                var ctx = scope.ServiceProvider.GetRequiredService<DataContext>();
                var cases = ctx.CaseEntity.Where(x => x.Status == 1 && now > x.DeadLine).ToList();
                if (cases != null)
                {
                    foreach (var item in cases)
                    {
                       item.Status = 3;
                       ctx.SaveChanges();
                       logger.LogInformation("Cambios efectuados");
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
