using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportStatusModule
{
    public class ReportStatusModuleModel
    {
        public string Id { get; set; }
        public string ModuleName { get; set; }
        public string ModuleId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string ModuleCode { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string ProjectId { get; set; }
        public decimal Quatity { get; set; }
        public int TotalModule { get; set; }
        public decimal TotalModuleInProject { get; set; }
        public List<ReportStatusModuleModel> ListModuleUse { get; set; }
        public List<ReportStatusModuleModel> ListModule { get; set; }
        public ReportStatusModuleModel()
        {
            ListModuleUse = new List<ReportStatusModuleModel>();
            ListModule = new List<ReportStatusModuleModel>();
        }
    }

}
