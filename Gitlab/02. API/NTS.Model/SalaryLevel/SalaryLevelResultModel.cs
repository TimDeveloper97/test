using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SalaryLevel
{
    public class SalaryLevelResultModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public string SalaryGroupId { get; set; }
        public string SalaryGroupName { get; set; }
        public string SalaryTypeId { get; set; }
        public string SalaryTypeName { get; set; }
        public string Note { get; set; }
    }
}
