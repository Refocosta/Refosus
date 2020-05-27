using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Refosus.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Models
{
    public class MessageViewModel :MessageEntity
    {
        public int TypeId { get; set; }
        public int StateId { get; set; }
        public String CreateUser { get; set; }

        public IEnumerable<SelectListItem> MessageType { get; set; }
        public IEnumerable<SelectListItem> MessageState { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }
        public MessagetransactionEntity Transaction { get; set; }
        
    }
}
