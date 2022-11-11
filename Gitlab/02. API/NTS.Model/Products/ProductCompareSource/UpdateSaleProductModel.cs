using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductCompareSource
{
    public class UpdateSaleProductModel
    {
        public List<string> ListIdSaleProduct { get; set; }

        public UpdateSaleProductModel()
        {
            ListIdSaleProduct = new List<string>();
        }
    }
}
