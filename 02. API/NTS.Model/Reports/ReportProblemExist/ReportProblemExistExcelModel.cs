using NTS.Model.ImportProfileProblemExist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportProblemExist
{
    public class ReportProblemExistExcelModel
    {
        public int TotalItem { get; set; }
        public int TotalProcessed { get; set; }
        public int TotalNoProcessed { get; set; }
        public int SupplierQuantity { get; set; }
        public int ContractQuantity { get; set; }
        public int PayQuantity { get; set; }
        public int ProductionQuantity { get; set; }
        public int TranportQuantity { get; set; }
        public int CustomsQuantity { get; set; }
        public int WarehouseQuantity { get; set; }
        public List<ReportProblemExistModel> ListEmployee { get; set; }
        public List<ReportProblemExistModel> ListSupplier { get; set; }
        public List<ReportProblemExistModel> ListReportProjectCode { get; set; }
        public List<ImportProfileProblemExistModel> ListResult { get; set; }
        public ReportProblemExistExcelModel()
        {
            ListEmployee = new List<ReportProblemExistModel>();
            ListSupplier = new List<ReportProblemExistModel>();
            ListReportProjectCode = new List<ReportProblemExistModel>();
            ListResult = new List<ImportProfileProblemExistModel>();
        }
    }
}
