using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SaleGroups
{
    public class SaleProductModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public string SaleProductTypeId { get; set; }
        public string SaleProductTypeName { get; set; }
        public bool IsChoose { get; set; }
    }
}
