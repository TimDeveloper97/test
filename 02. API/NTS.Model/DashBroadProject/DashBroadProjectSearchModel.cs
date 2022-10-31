using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DashBroadProject
{
    public class DashBroadProjectSearchModel
    {
        public string ProjCode { get; set; }
        public string SBUId { get; set; }
        public string ProjectCode { get; set; }
        public string DepartmentId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string ModuleId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerTypeId { get; set; }
        public string DepartermentUserId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string search { get; set; }
        public string ProjectName { get; set; }
        public int? DocumentStatus { get; set; }
        public int? Type { get; set; }
        public int? ErrorStatus { get; set; }
    }
}
