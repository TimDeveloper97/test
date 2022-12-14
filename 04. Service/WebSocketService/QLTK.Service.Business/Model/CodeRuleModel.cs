using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class CodeRuleModel
    {
        public string Id { get; set; }
        public string MaterialGroupId { get; set; }
        public string MaterialGroupName { get; set; }
        public string MaterialGroupTPAId { get; set; }
        public string MaterialGroupTPAName { get; set; }
        public string Code { get; set; }
        public int Length { get; set; }
        public string UnitId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
