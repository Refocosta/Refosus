using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class MessageEntity
    {
        public int Id { get; set; }

        [Display(Name = "Tipo")]
        public MessageTypeEntity Type { get; set; }

        [Display(Name = "Remitente")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Sender { get; set; }

        [Display(Name = "Referencia")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Reference { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Radicación")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime CreateDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Radicación")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime CreateDateLocal => CreateDate.ToLocalTime();

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Actualización")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime UpdateDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Actualización")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}", ApplyFormatInEditMode = false)]
        public DateTime UpdateDateLocal => UpdateDate.ToLocalTime();

        [Display(Name = "Estado")]
        public MessageStateEntity State { get; set; }

        [Display(Name = "Usuario Asignado")]
        public UserEntity User { get; set; }
        public ICollection<MessagetransactionEntity> Transaction { get; set; }

    }
}
