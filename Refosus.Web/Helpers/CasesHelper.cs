using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Refosus.Web.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refosus.Web.Helpers;

namespace Refosus.Web.Helpers
{
    public class CasesHelper : IHostedService, IDisposable
    {
        private ILogger<CasesHelper> logger;
        private Timer timer;
        private Timer reminder;
        public IServiceScopeFactory serviceScopeFactory;
        public MailHelper helper;

        public CasesHelper(ILogger<CasesHelper> logger, IServiceScopeFactory _serviceScopeFactory, MailHelper _helper)
        {
            this.logger = logger;
            this.serviceScopeFactory = _serviceScopeFactory;
            this.helper = _helper;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {

            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(12));
            reminder = new Timer(Reminder, null, TimeSpan.Zero, TimeSpan.FromHours(4));
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
                        var dependencies = new List<dynamic>();
                        dependencies.Add(new
                        {
                            CaseId = item.Id,
                            CaseCode = item.Code,
                            CaseDeadline = item.DeadLine,
                            CaseResponsable = item.Responsable
                        });
                        string[] sender = new string[1];
                        sender[0] = item.Sender;
                        string[] responsable = new string[1];
                        responsable[0] = item.Responsable;
                        string subject = "Vencimiento del caso No. " + item.Id;
                        string bodySender = "<strong>Hola</strong>," +
                        "<br/><br/>Tu caso <strong>" + item.Code + "</strong> se ha vencido el <strong>" + item.DeadLine + "</strong>" +
                        "<br/>el responsable de solucionar tu solicitud a sido " + item.Responsable + "</strong>" +
                        "<br/>recuerda que puedes hacer un <strong>“llamado de atención”</strong> para que este sea agilizado." +
                        "<br /><br/>Atentamente,<br/>" + "Equipo de Soporte - Refocosta.<br/>";
                        string bodyResponsable = "<strong>Hola</strong>," +
                        "<br/>El caso <strong>" + item.Code + "</strong>, se ha vencido el <strong>" + item.DeadLine + "</strong>" +
                        "<br/>el responsable de solucionar el solicitud a sido " + item.Responsable + "</strong>" +
                        "<br /><br/>Atentamente,<br/>" + "Equipo de Soporte - Refocosta.<br/>";
                        helper.sendMail(sender, subject, bodySender);
                        helper.sendMail(responsable, subject, bodyResponsable);
                    }
                }
            }
        }

        private void Reminder(object state)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var now = System.DateTime.Now.ToUniversalTime();
                var ctx = scope.ServiceProvider.GetRequiredService<DataContext>();
                var cases = ctx.CaseEntity.Where(x => now > x.DeadLine && x.Status == 3).ToList();
                if (cases != null)
                {
                    foreach (var item in cases)
                    {
                        string[] responsable = new string[1];
                        responsable[0] = item.Responsable;
                        string subject = "Recordatorio del caso No. " + item.Id;
                        string body = "<strong>Hola</strong>," +
                        "<br/>El caso <strong>" + item.Code + "</strong>, está vencido desde <strong>" + item.DeadLine + "</strong>" +
                        "<br/>por favor soluciona el caso" +
                        "<br /><br/>Atentamente,<br/>" + "Equipo de Soporte - Refocosta.<br/>";
                        helper.sendMail(responsable, subject, body);
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            reminder?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
