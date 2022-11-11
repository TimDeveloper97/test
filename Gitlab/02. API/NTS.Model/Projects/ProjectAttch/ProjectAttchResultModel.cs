using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectAttch
{
    public class ProjectAttchResultModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public decimal? FileSize { get; set; }
        public string Path { get; set; }
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByName { get; set; }
        public int Type { get; set; }
        public int PromulgateType { get; set; }
        public string PromulgateName { get; set; }
        public string PromulgateCode { get; set; }
        public string GroupName { get; set; }
        public DateTime? PromulgateDate { get; set; }
        public string CustomerId { get; set; }
        public string SupplierId { get; set; }
        public bool IsRequired { get; set; }
        public string ParentId { get; set; }
    }
}
