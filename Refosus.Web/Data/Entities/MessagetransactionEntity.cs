using System;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Estado Anterior de Factura")]
        public MessageBillStateEntity StateBillCreate { get; set; }

        [Display(Name = "Estado Nuevo de Factura")]
        public MessageBillStateEntity StateBillUpdate { get; set; }

        [Display(Name = "Usuario Que Aprobo")]
        public UserEntity UserBillAutho { get; set; }

        [Display(Name = "Usuario Que Tramito")]
        public UserEntity UserBillFinished { get; set; }

        [Display(Name = "Descripcion")]
        [MinLength(1, ErrorMessage = "El campo {0} no puede estar vacio.")]
        public string Description { get; set; }
    }
}
