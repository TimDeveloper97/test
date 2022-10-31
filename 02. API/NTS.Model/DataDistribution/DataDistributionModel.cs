using NTS.Model.DataDistributionFile;
using NTS.Model.QLTKMODULE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DataDistribution
{
    public class DataDistributionModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string DepartmentId { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string Description { get; set; }
        public int? Type { get; set; }
        public string Path { get; set; }
        public bool IsCreateUpdate { get; set; }
        public bool IsExportMaterial { get; set; }
        public List<ModuleModel> ListSelect { get; set; }
        public List<DataDistributionFileModel> ListFile { get; set; }
        public DataDistributionModel()
        {
            ListFile = new List<DataDistributionFileModel>();
        }

    }
}
