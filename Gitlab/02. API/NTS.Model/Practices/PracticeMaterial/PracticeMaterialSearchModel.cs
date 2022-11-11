using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.PracticeMaterial
{
    public class PracticeMaterialSearchModel : SearchCommonModel
    {
        public string PracticeId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public decimal? Pricing { get; set; }
        public int? DeliveryDay { get; set; }
        public int Operators { get; set; }
        public int MaterialPriceType { get; set; }
    }
}
