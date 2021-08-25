﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public interface ICaseTrait
    {
        Boolean MailTypeStore(string[] to, List<dynamic> dependencies, int typeMail);
        String Random();
        public Boolean MailTypeUpdate(string[] to, List<dynamic> dependencies, int typeMail);
        public Boolean mailTypeDelete();
        public Boolean mailTypeSolution(string[] to, List<dynamic> dependencies, int typeMail);
        public Boolean mailTypeeExpiration();
        public Boolean mailTypeReminder();
    }
}
