using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.HistoryVersion
{
    public class WorkDiaryHistoryModel
    {
        public string Id { get; set; }
        public string DepartmentId { get; set; }
        public string SBUId { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public string ObjectId { get; set; }
        public System.DateTime WorkDate { get; set; }
        public decimal TotalTime { get; set; }
        public string Address { get; set; }
        public int Done { get; set; }
        public int ObjectType { get; set; }
        public string Note { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
