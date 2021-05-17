using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Refosus.Web.Data.Entities
{
    public class ShoppingItemsEntity
    {
        [Display(Name = "Consecutivo")]
        public int Id { get; set; }

        [Display(Name = "Codigo SAP")] public ShoppingEntity Shoping { get; set; }
        public string CodSAP { get; set; }
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        [Display(Name = "Categoria")]
        public ShoppingCategoryEntity Category { get; set; }
        [Display(Name = "Sub Categoria")]
        public ShoppingCategoryEntity SubCategory { get; set; }
        [Display(Name = "Unidades")]
        public ShoppingUnitEntity Unit { get; set; }
        [Display(Name = "Medida")]
        public ShoppingMeasureEntity Measure { get; set; }
        [Display(Name = "Cantidad")]
        public int Quantity { get; set; }
        [Display(Name = "Referencia")]
        public string Reference { get; set; }
        [Display(Name = "Marca")]
        public string Mark { get; set; }
        [Display(Name = "Ordern Interna")]
        public string InternalOrder { get; set; }
        [Display(Name = "Numero Orden Interna")]
        public string NumInternalOrder { get; set; }
        [Display(Name = "Observaciones")]
        public string Observation { get; set; }
        [Display(Name = "Cantidad Entregada")]
        public int QuantityDelivered { get; set; }
        [Display(Name = "Valor Unidad")]
        public decimal ValorUnidad { get; set; }
        [Display(Name = "CostoTotal")]
        public decimal ValorTotal { get; set; }
        [Display(Name = "Estado")]
        public TP_Shopping_Item_StateEntity State { get; set; }
        [Display(Name = "Asignado A")]
        public UserEntity UserAssigned { get; set; }
        [Display(Name = "Proveedores")]
        public List<TP_Shopping_ItemProvedorEntity> Proveedores { get; set; }
    }
}
