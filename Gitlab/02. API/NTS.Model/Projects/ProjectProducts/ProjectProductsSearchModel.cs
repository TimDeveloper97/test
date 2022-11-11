using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Combobox;

namespace NTS.Model.ProjectProducts
{
    public class ProjectProductsSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string ModuleId { get; set; }
        public string Note { get; set; }
        public string ContractCode { get; set; }
        public string ContractName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int DataType { get; set; }
        public int ModuleStatus { get; set; }
        public int DesignStatus { get; set; }
        public int? CostType { get; set; }
        public string MaterialName { get; set; }
        public string QCStatus { get; set; }


    }
}
