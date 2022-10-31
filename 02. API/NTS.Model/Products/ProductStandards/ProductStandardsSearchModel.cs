using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductStandards
{
    public class ProductStandardsSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ProductStandardGroupId { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CreateBy { get; set; }
        public string CreateByName { get; set; }
        public int? DataType { get; set; }
        public bool IsExport { get; set; }

        public List<string> ListIdSelect { get; set; }
        public List<string> ListIdChecked { get; set; }
    }
}
