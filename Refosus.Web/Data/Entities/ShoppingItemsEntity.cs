using System;

namespace Refosus.Web.Data.Entities
{
    public class ShoppingItemsEntity
    {
        public int Id { get; set; }
        public ShoppingEntity Shoping { get; set; }
        public string CodSAP { get; set; }
        public ShoppingCategoryEntity Category { get; set; }
        public ShoppingCategoryEntity SubCategory { get; set; }
        public ShoppingUnitEntity Unit { get; set; }
        public ShoppingMeasureEntity Measure { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Refence { get; set; }
        public string Mark { get; set; }
        public string InternalOrder { get; set; }
        public string NumInternalOrder { get; set; }
        public double UnitCost { get; set; }
        public int QuantityDelivered { get; set; }
        public TP_Shoping_Item_StateEntity State { get; set; }
        public double FullCost { get; set; }
    }
}
