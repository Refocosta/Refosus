using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class MessageCheckEntity
    {
        public int Id { get; set; }
        public MessageEntity message { get; set; }

        [Display(Name = "Usuario")]
        public UserEntity User { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Autorizacion")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime DateAut { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Autorizacion")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}", ApplyFormatInEditMode = false)]
        public DateTime DateAutLocal => DateAut.ToLocalTime();
    }
}
