using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class MaterialFromDBModel
    {
        public string Id { get; set; }
        public string Index { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string ManufactureId { get; set; }
        public string ManufactureCode { get; set; }
        public decimal? Pricing { get; set; }
        public string UnitName { get; set; }
        public DateTime CreateDate { get; set; }
        public string MaterialGroupId { get; set; }
        public string Note { get; set; }
    }
}
