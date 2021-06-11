using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class TP_Shopping_ItemProvedorEntity
    {
        public int Id { get; set; }
        [DisplayName("Nombre")]
        public ShoppingItemsEntity Item { get; set; }

        [DisplayName("Nombre")]
        public string Name { get; set; }
        [DisplayName("Telefono")]
        public string Telefono { get; set; }
        [DisplayName("Cantidad")]
        public double Cantidad { get; set; }
        [DisplayName("Precio Unidad")]
        public double PrecioUnidad { get; set; }
        [DisplayName("Precio Total")]
        public double PrecioTotal { get; set; }
        [DisplayName("Nombre")]
        public string FileName { get; set; }

        [DisplayName("Archivo")]
        public string FilePath { get; set; }

    }
}
