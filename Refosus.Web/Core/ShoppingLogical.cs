using Refosus.Web.Data.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Core
{
    public class ShoppingLogical
    {

        public void UpdShopingToNull(ShopingToNull obj,string cade)
        {
            Data.Connection.Data data = new Data.Connection.Data();
            data.updShopingToNull(obj, cade);
        }

    }
}
