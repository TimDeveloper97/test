using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleError
{
    public class ModuleErrorResultModel
    {
        public string Id { get; set; }
        public string ModuleName { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentProcessName { get; set; }
        public string ModuleErrorVisual { get; set; }
        public string ModuleErrorReason { get; set; }
        public string ModuleErrorCost { get; set; }
        public string Code { get; set; }
        public string AuthorName { get; set; }
        public string CreateBy { get; set; }
        public List<string> PersonError { get; set; }
        public string ProjecName { get; set; }
        public string QuickSolution { get; set; }
        public string Solution { get; set; }
        public int Status { get; set; }
        public Nullable<System.DateTime> ActualFinishDate { get; set; }
        public Nullable<int> IsKCSConfirmed { get; set; }
        public Nullable<int> IsManagerConfirmed { get; set; }
        public Nullable<int> TempComfirmed { get; set; }
        public Nullable<System.DateTime> DesignCompleteTime { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string PersonConfirmmail { get; set; }
        public string PersonProcessmail { get; set; }
        public int? IsAllowDowload { get; set; }
        

    }
}
