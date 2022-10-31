using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.HistoryVersion
{
    public class DocumentHistoryModel
    {
        public string Id { get; set; }
        public string DocumentGroupId { get; set; }
        public string DocumentTypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Version { get; set; }
        public int CompilationType { get; set; }
        public string CompilationEmployeeId { get; set; }
        public string CompilationSuppliserId { get; set; }
        public Nullable<System.DateTime> PromulgateDate { get; set; }
        public Nullable<System.DateTime> PromulgateLastDate { get; set; }
        public string DepartmentId { get; set; }
        public string EmployeeId { get; set; }
        public Nullable<System.DateTime> ReviewDateFrom { get; set; }
        public Nullable<System.DateTime> ReviewDateTo { get; set; }
        public decimal Price { get; set; }
        public string Keyword { get; set; }
        public string Description { get; set; }
        public string ApproveWorkTypeId { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public int Status { get; set; }
    }
}
