using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductStandardHistory
{
    public class ProductStandardLogHistoryModel
    {
        public string Id { get; set; }
        public string ProductStandardGroupId { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Target { get; set; }
        public string Note { get; set; }
        public string Version { get; set; }
        public string EditContent { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
