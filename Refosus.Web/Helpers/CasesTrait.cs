using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Refosus.Web.Helpers;

namespace Refosus.Web.Helpers
{
    public class CasesTrait : ICaseTrait
    {

        IMailHelper mailer;

        public CasesTrait(IMailHelper _mailer)
        {
            mailer = _mailer;
        }
        public Boolean MailTypeStore(string[] to, List<dynamic>dependencies, int typeMail)
        {
            string subject = "";
            string body = "";

            foreach (var item in dependencies)
            {
                subject = "Nuevo Caso No. " + item.CaseId;
               if (typeMail == 1)
               {
                   body = "<strong>Hola</strong>,<br/><br/>Hemos registrado tu solicitud con el código <strong>" + item.CaseCode + "</strong>"+ 
                   "<br/>pronto recibirás un correo en el que se asignará una persona para atenderlo"+
                   "<br/>esta respuesta debe llegar antes de <strong>" + item.CaseDeadline + "</strong>"+
                   "<br/>recuerda estar pediente del estado de tu caso."+
                   "<br/><br />Atentamente,<br/>" + "Equipo de Soporte - Refocosta.<br/>";
               }

               if (typeMail == 2)
               {
                    body = "<strong>Hola</strong>,<br/><br/>Se ha registrado el caso con el codigo <strong>" + item.CaseCode + "</strong>" +
                   "<br/>por favor asigna el caso a la persona que debe solucionarlo antes de <strong>" + item.CaseDeadline + "</strong>" +
                   "<br/><br />Atentamente,<br/>" + "Equipo de Soporte - Refocosta.<br/>";
                }
            }

            return(mailer.sendMail(to, subject, body));
        }
        
        public Boolean MailTypeUpdate(string[] to, List<dynamic> dependencies, int typeMail)
        {
            string subject = "";
            string body = "";
            foreach (var item in dependencies)
            {
                subject = "Actualización del caso No. " + item.CaseId;
                if (typeMail == 1)
                {
                    body = "<strong>Hola</strong>," +
                    "<br/><br/>Tu caso <strong>" + item.CaseCode + "</strong>" + " ha sido asignado a " + item.CaseResponsable +
                    "<br/>la fecha estimada de solución es <strong>" + item.CaseDeadline + "</strong>" +
                    "<br/>recuerda que puedes hacer seguimiento en https://nativa.refocosta.com/Cases ingresando con tu nombre de usuario y contraseña." +
                    "<br/>si en la fecha estimada de solución no has recibido una respuesta, puedes ingresar a tu cuenta y hacer un <strong>“llamado de atención”</strong> para que este sea agilizado." +
                    "<br /><br/>Atentamente,<br/>" + "Equipo de Soporte - Refocosta.<br/>";
                }

                if (typeMail == 2)
                {
                    body = "<strong>Hola</strong>," +
                    "<br/><br/>El caso <strong>" + item.CaseCode + "</strong> te ha sido asignado" +
                    "<br/>por favor solucionalo antes de <strong>" + item.CaseDeadline + "</strong>" +
                    "<br /><br/>Atentamente,<br/>" + "Equipo de Soporte - Refocosta.<br/>";
                }
            }
            return (mailer.sendMail(to, subject, body));
        }

        public Boolean mailTypeDelete(string[] to, List<dynamic> dependencies, int typeMail)
        {
            string subject = "";
            string body = "";
            foreach (var item in dependencies)
            {
                subject = "Actualización del caso No. " + item.CaseId;
                if (typeMail == 1)
                {
                    body = "<strong>Hola</strong>," +
                    "<br/><br/>Tu caso <strong>" + item.CaseCode + "</strong>" + " ha sido deshabilitado"+
                    "<br/>por favor comunicate con el responsable de tu caso <strong>" + item.CaseResponsable + "</strong> para verificar las razones" +
                    "<br/>recuerda que puedes hacer seguimiento en https://nativa.refocosta.com/Cases ingresando con tu nombre de usuario y contraseña." +
                    "<br /><br/>Atentamente,<br/>" + "Equipo de Soporte - Refocosta.<br/>";
                }

                if (typeMail == 2)
                {
                    body = "<strong>Hola</strong>," +
                    "<br/><br/>El caso <strong>" + item.CaseCode + "</strong> ha sido deshabilitado" +
                    "<br/>por favor verifcia las razones" +
                    "<br /><br/>Atentamente,<br/>" + "Equipo de Soporte - Refocosta.<br/>";
                }
            }
            return (mailer.sendMail(to, subject, body));
        }

        public Boolean mailTypeSolution(string[] to, List<dynamic> dependencies, int typeMail)
        {
            string subject = "";
            string body = "";
            foreach (var item in dependencies)
            {
                subject = "Solución del caso No. " + item.CaseId;
                if (typeMail == 1)
                {
                    body = "<strong>Hola</strong>," +
                    "<br/><br/>Tu caso <strong>"+ item.CaseCode + "</strong> ha sido cerrado el <strong>" + item.CaseClosingDate + "</strong>" +
                    "<br/>el responsable de solucionar tu solicitud a sido " + item.CaseResponsable + "</strong>" +
                    "<br/>recuerda que puedes hacer seguimiento en https://nativa.refocosta.com/Cases ingresando con tu nombre de usuario y contraseña." +
                    "<br /><br/>Atentamente,<br/>" + "Equipo de Soporte - Refocosta.<br/>";
                }

                if (typeMail == 2)
                {
                    body = "<strong>Hola</strong>," +
                    "<br/>Se ha solucionado el caso <strong>" + item.CaseCode + "</strong>, ha sido cerrado el <strong>" + item.CaseClosingDate + "</strong>" +
                    "<br/>el responsable de solucionar el solicitud a sido " + item.CaseResponsable + "</strong>" +
                    "<br /><br/>Atentamente,<br/>" + "Equipo de Soporte - Refocosta.<br/>";
                }
            }
            return (mailer.sendMail(to, subject, body));
        }

        public Boolean mailTypeExpiration(string[] to, List<dynamic> dependencies, int typeMail)
        {
            string subject = "";
            string body = "";
            foreach (var item in dependencies)
            {
                subject = "Vencimiento del caso No. " + item.CaseId;
                if (typeMail == 1)
                {
                    body = "<strong>Hola</strong>," +
                    "<br/><br/>Tu caso <strong>" + item.CaseCode + "</strong> se ha vencido el <strong>" + item.CaseClosingDate + "</strong>" +
                    "<br/>el responsable de solucionar tu solicitud a sido " + item.CaseResponsable + "</strong>" +
                    "<br/>recuerda que puedes hacer un <strong>“llamado de atención”</strong> para que este sea agilizado." +
                    "<br /><br/>Atentamente,<br/>" + "Equipo de Soporte - Refocosta.<br/>";
                }

                if (typeMail == 2)
                {
                    body = "<strong>Hola</strong>," +
                    "<br/>El caso <strong>" + item.CaseCode + "</strong>, se ha vencido el <strong>" + item.CaseClosingDate + "</strong>" +
                    "<br/>el responsable de solucionar el solicitud a sido " + item.CaseResponsable + "</strong>" +
                    "<br /><br/>Atentamente,<br/>" + "Equipo de Soporte - Refocosta.<br/>";
                }
            }
            return (mailer.sendMail(to, subject, body));
        }

        public Boolean mailTypeReminder()
        {
            return true;
        }

        public String Random()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[6];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            return finalString;
        }
    }
}
