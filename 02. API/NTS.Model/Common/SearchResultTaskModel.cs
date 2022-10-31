using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Common
{
    public class SearchResultTaskModel<T>
    {
        public int TotalItem { get; set; }
        public List<T> ListResult = new List<T>();
        public List<T> ListResult1 = new List<T>();
        public int Status1 { get; set; }
        public int Status2 { get; set; }
        public int Status3 { get; set; }
        public int Status4 { get; set; }
        public int TotalError { get; set; }
        public decimal MaxPricing { get; set; }
        public int MaxDeliveryDay { get; set; }
    }
}
