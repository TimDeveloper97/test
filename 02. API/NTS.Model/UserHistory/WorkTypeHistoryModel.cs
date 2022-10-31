using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.UserHistory
{
    public class WorkTypeHistoryModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Quantity { get; set; }
        public string DepartmentId { get; set; }
        public string SBUId { get; set; }
        public string FlowStageId { get; set; }
        public string SalaryLevelMinId { get; set; }
        public string SalaryLevelMaxId { get; set; }
        public Nullable<decimal> Value { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
