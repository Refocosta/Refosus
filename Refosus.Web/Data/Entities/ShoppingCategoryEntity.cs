using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class ShoppingCategoryEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ShoppingCategoryEntity SubCategory { get; set; }
        public UserEntity Responsable { get; set; }
    }
}
