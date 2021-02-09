using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class MessageEntity
    {
        [Display(Name = "Radicado")]
        public int Id { get; set; }

        [Display(Name = "Compañia")]
        public CompanyEntity Company { get; set; }

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
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime UpdateDateLocal => UpdateDate.ToLocalTime();

        [Display(Name = "Creado Por")]
        public UserEntity UserCreate { get; set; }

        [Display(Name = "Enviado Por")]
        public UserEntity UserSender { get; set; }

        [Display(Name = "Enviado A")]
        public UserEntity User { get; set; }

        [Display(Name = "Estado")]
        public MessageStateEntity State { get; set; }

        [Display(Name = "Estado de Factura")]
        public MessageBillStateEntity StateBill { get; set; }

        [Display(Name = "Usuario que Autoriza")]
        public UserEntity UserAut { get; set; }

        [Display(Name = "Usuario que Procesa")]
        public UserEntity UserPros { get; set; }

        [Display(Name = "Consecutivo de Factura")]
        public string NumberBill { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Autorizacion")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime DateAut { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Autorizacion")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}", ApplyFormatInEditMode = false)]
        public DateTime DateAutLocal => DateAut.ToLocalTime();

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha del Proceso")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime DateProcess { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha del Proceso")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}", ApplyFormatInEditMode = false)]
        public DateTime DateProcessLocal => DateProcess.ToLocalTime();

        public ICollection<MessagetransactionEntity> Transaction { get; set; }
        public ICollection<MessageCheckEntity> Checks { get; set; }
        public ICollection<MessageFileEntity> MessageFiles { get; set; }

        [Display(Name = "Centro de Costos")]
        public CeCoEntity Ceco { get; set; }

        [Display(Name = "Observación")]
        public string Observation { get; set; }

        [Display(Name = "Valor")]
        public int Value { get; set; }

        public static implicit operator MessageEntity(List<MessageEntity> v)
        {
            throw new NotImplementedException();
        }
    }
}
