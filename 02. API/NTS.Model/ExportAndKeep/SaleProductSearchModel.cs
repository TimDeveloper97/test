using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ExportAndKeep
{
    public class SaleProductSearchModel : SearchCommonModel
    {
        public string NameCode { get; set; }
        public string SaleProductTypeId { get; set; }
        public int? Inventory { get; set; }
        public int InventoryType { get; set; }
        public int? AvailableQuantity { get; set; }
        public int AvailableQuantityType { get; set; }
        public int? EXWTPAPrice { get; set; }
        public int EXWTPAPriceType { get; set; }
        public string CountryName { get; set; }
        public string ManufactureId { get; set; }
        public List<string> ListIdSelect { get; set; }

        public List<string> SaleGroupIdRequests { get; set; }
        public SaleProductSearchModel()
        {
            ListIdSelect = new List<string>();
            SaleGroupIdRequests = new List<string>();
        }
    }
}
