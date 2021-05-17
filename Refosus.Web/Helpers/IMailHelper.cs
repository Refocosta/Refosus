using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public interface IMailHelper
    {
        bool sendMail(string[] to, string subject, string body);
        bool sendMailHTML(string Modulo, string Body, string Asunto, List<string> to);
    }
}
