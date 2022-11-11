using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Product
{
    public class SearchErrorProductModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ObjectId { get; set; }
        public string DepartmentId { get; set; }
        public string ModuleErrorVisualId { get; set; }
        public string Code { get; set; }
        public string Subject { get; set; }

        public int? Status { get; set; }
        public string CreateDate { get; set; }
        public int Year { set; get; }
        public int Quarter { set; get; }
        public int Month { set; get; }
        public string TimeType { set; get; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}
