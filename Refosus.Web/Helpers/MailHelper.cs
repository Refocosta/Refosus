using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public class MailHelper : IMailHelper
    {

        public bool sendMail(string[] to, string subject, string body)
        {

            string EmailOrigen = "nativa@refocosta.com";
            string Password = "Nativ@123";

            MailMessage mail = new MailMessage();

            int count = 0;
            foreach (string item in to)
            {

                mail.To.Add(new MailAddress(to[count]));
                count++;
            }

            mail.From = new MailAddress(EmailOrigen);
            mail.Subject = subject;
            mail.Body = body;

            mail.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("mail.refocosta.com")
            {
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Host = "mail.refocosta.com",
                Port = 587,
                Credentials = new System.Net.NetworkCredential(EmailOrigen, Password)
            };
            client.Send(mail);
            return true;
        }


        public bool sendMailHTML(string Modulo, string Body, string Asunto, List<string> to)
        {
            return enviar(getHTML(Modulo, Body), Asunto, to);
        }
        protected string getHTML(string Modulo, string Body)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                $"wwwroot\\HTML\\",
                "formato MAIL.html");
                using (FileStream fs = File.OpenRead(path))
                {
                    fs.CopyTo(ms);
                }

                string s = Encoding.ASCII.GetString(ms.ToArray());
                s = s.Replace("{NombreModulo}", Modulo);
                s = s.Replace("{CuerpoMensaje}", Body);
                s = s.Remove(0, 3);
                return s;
            }
        }
        protected bool enviar(string body, string subject, List<string> mail)
        {
            bool result = false;


            string correoOrigen = "nativa@refocosta.com";
            string usuarioCorreo = "nativa@refocosta.com";
            string passCorreo = "Nativ@123";
            string servidorSmtp = "mail.refocosta.com";
            string puerto = "587";
            bool usaSsl = true;


            using (MailMessage mensajeMail = new MailMessage())
            {
                try
                {
                    mensajeMail.From = new System.Net.Mail.MailAddress(correoOrigen, "SOPORTE NATIVA");
                    foreach (var item in mail)
                    {
                        mensajeMail.To.Add(item);
                    }
                    mensajeMail.Subject = subject;
                    mensajeMail.SubjectEncoding = System.Text.Encoding.UTF8;


                    mensajeMail.Body = body;
                    mensajeMail.BodyEncoding = System.Text.Encoding.UTF8;
                    mensajeMail.IsBodyHtml = true;
                    mensajeMail.Priority = System.Net.Mail.MailPriority.Normal;
                    using (SmtpClient client = new SmtpClient())
                    {
                        client.Credentials = new System.Net.NetworkCredential(usuarioCorreo, passCorreo);
                        client.Port = int.Parse(puerto);
                        client.Host = servidorSmtp;
                        client.EnableSsl = false;
                        client.Timeout = 30000;
                        client.Send(mensajeMail);
                    }
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
