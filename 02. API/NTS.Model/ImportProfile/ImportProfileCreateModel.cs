using NTS.Model.ImportPR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ImportProfile
{
    public class ImportProfileCreateModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime DueDatePR { get; set; }
        public string ManufactureCode { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string PRCode { get; set; }
        public int Index { get; set; }
        public DateTime SupplierExpectedDate { get; set; }
        public DateTime ContractExpectedDate { get; set; }
        public DateTime PayExpectedDate { get; set; }
        public DateTime ProductionExpectedDate { get; set; }
        public DateTime TransportExpectedDate { get; set; }
        public DateTime CustomExpectedDate { get; set; }
        public DateTime WarehouseExpectedDate { get; set; }
        public List<ImportPRModel> ListMaterial { get; set; }
        public List<string> ListFileId { get; set; }
    }
}
