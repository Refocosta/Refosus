using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class TP_Shopping_ArticleEntity
    {
        public int Id { get; set; }
        [Display(Name="Codigo SAP")]
        public string CodSAP { get; set; }
        [Display(Name = "Categoria")]
        public ShoppingCategoryEntity Category { get; set; }
        [Display(Name = "Sub Categoria")]
        public ShoppingCategoryEntity SubCategory { get; set; }
        [Display(Name = "Unidades")]
        public ShoppingUnitEntity Unit { get; set; }
        [Display(Name = "Medida")]
        public ShoppingMeasureEntity Measure { get; set; }
        [Display(Name = "Descripcion")]
        public string Description { get; set; }
        [Display(Name = "Referencia")]
        public string Refence { get; set; }
        [Display(Name = "Marca")]
        public string Mark { get; set; }
        [Display(Name = "Orden Interna")]
        public string InternalOrder { get; set; }
        [Display(Name = "Numero de Orden Interna")]
        public string NumInternalOrder { get; set; }
    }
}
