﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public class MailHelper : IMailHelper
    {

        public bool sendMail(string[] to, string subject, string body)
        {

            string EmailOrigen = "nativa@refocosta.com";
            string Password = "Nativ@123";
            //string body = "<strong>Favor, NO responda a este mensaje, el envió es generado de forma automática. </strong>";

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
    }
}
