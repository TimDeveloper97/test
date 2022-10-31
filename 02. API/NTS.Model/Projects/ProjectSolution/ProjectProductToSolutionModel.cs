using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectSolution
{
    public class ProjectProductToSolutionModel
    {
        public string Id { get; set; }
        public string ContractName { get; set; }
        public string ContractCode { get; set; }
        public string ProjectId { get; set; }
        public string ParentId { get; set; }
        public string Note { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public bool Checked { get; set; }
        public int DataType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ProductId { get; set; }
        public string ModuleId { get; set; }
    }
}
