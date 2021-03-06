﻿using Microsoft.AspNetCore.Http;
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
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de mensaje.")]
        public int TypeId { get; set; }

        [Display(Name = "Compañia")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Compañia.")]
        public int CompanyId { get; set; }

        [Display(Name = "Estado")]
        public int StateId { get; set; }
        public int StateIdOld { get; set; }

        [Display(Name = "Usuario que crea")]
        public string CreateUser { get; set; }

        [Display(Name = "Usuario que Envia")]
        public string UserTrn { get; set; }

        [Display(Name = "Usuario que Recibe")]
        public string UserRec { get; set; }

        [Display(Name = "Estado de Factura")]
        public int StateBillId { get; set; }

        [Display(Name = "Centro de Costos")]
        public int CecoId { get; set; }

        [Display(Name = "Usuario que Autoriza")]
        public string AutUser { get; set; }

        [Display(Name = "Usuario que Proceso")]
        public string ProUser { get; set; }



        public IEnumerable<SelectListItem> MessageType { get; set; }
        public IEnumerable<SelectListItem> MessageState { get; set; }

        public IEnumerable<SelectListItem> MessageBillState { get; set; }
        public IEnumerable<SelectListItem> Cecos { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }

        public new MessagetransactionEntity Transaction { get; set; }
        public new MessageCheckEntity Checks { get; set; }

        [Display(Name = "Adjunto")]
        public IEnumerable<IFormFile> File { get; set; }
        public string Archivo { get; internal set; }
        public int Operation { get; set; }
    }
}
