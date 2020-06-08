using Microsoft.AspNetCore.Mvc.Rendering;
using Refosus.Web.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Refosus.Web.Models
{
    public class MessageViewModel : MessageEntity
    {
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una compañia.")]
        public int TypeId { get; set; }
        [Display(Name = "Estado")]
        public int StateId { get; set; }
        public int StateIdOld { get; set; }
        [Display(Name = "Usuario")]
        public string CreateUser { get; set; }

        [Display(Name = "Estado de Factura")]
        public int StateBillId { get; set; }

        [Display(Name = "Usuario que Autoriza")]
        public string AutUser { get; set; }

        [Display(Name = "Usuario que Proceso")]
        public string ProUser { get; set; }


        public IEnumerable<SelectListItem> MessageType { get; set; }
        public IEnumerable<SelectListItem> MessageState { get; set; }

        public IEnumerable<SelectListItem> MessageBillState { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }
        public MessagetransactionEntity Transaction { get; set; }

    }
}
