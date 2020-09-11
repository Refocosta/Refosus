using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public interface IMailHelper
    {
        Boolean sendMail(string [] to,string subject, string body);
    }
}
