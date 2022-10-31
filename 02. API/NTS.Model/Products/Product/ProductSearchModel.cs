using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Product
{
    public class ProductSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ProductGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int? IsEnought { get; set; }
        public bool isEnought1 { get; set; }
        public string DepartmentId { get; set; }
        public string SBUId { get; set; }
        public bool IsSendSale { get; set; }
        public int Publications { get; set; }
        public string TypeCatalogs { get; set; }
        public string TypeGuidePractice { get; set; }
        public string TypeDMBTH { get; set; }
        public string TypeGuideMaintenance { get; set; }
    }
}
