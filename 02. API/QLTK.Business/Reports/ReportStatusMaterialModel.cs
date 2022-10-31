using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ReportStatusModule
{
    public class ReportStatusMaterialModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public Decimal Price { get; set; }
        public Decimal TotalMaterial { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
