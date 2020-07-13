using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class MessageFileEntity
    {
        public int Id { get; set; }
        public MessageEntity message { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Archivo")]
        public string FilePath { get; set; }
        [Display(Name = "Ext")]
        public string Ext { get; set; }
    }
}
