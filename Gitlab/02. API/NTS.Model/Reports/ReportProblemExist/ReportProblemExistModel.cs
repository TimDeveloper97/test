using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportProblemExist
{
    public class ReportProblemExistModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Total { get; set; }
        public int SupplierQuantity { get; set; }
        public int ContractQuantity { get; set; }
        public int PayQuantity { get; set; }
        public int ProductionQuantity { get; set; }
        public int TranportQuantity { get; set; }
        public int CustomsQuantity { get; set; }
        public int WarehouseQuantity { get; set; }
        public decimal AmountVND { get; set; }
    }
}
