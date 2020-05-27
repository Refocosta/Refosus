using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class MessagetransactionEntity
    {
        public int Id { get; set; }

        public MessageEntity Message { get; set; }

        [Display(Name = "Usuario Anterior")]
        public UserEntity UserCreate { get; set; }

        [Display(Name = "Usuario Nuevo")]
        public UserEntity UserUpdate { get; set; }

        [Display(Name = "Estado Anterior")]
        public MessageStateEntity StateCreate { get; set; }

        [Display(Name = "Estado Nuevo")]
        public MessageStateEntity StateUpdate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Registro")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime UpdateDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Registro")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime UpdateDateLocal => UpdateDate.ToLocalTime();

        [Display(Name = "Observaciones")]
        public string Observation { get; set; }
    }
}
