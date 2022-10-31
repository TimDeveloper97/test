using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.HistoryVersion
{
    public class DataDistributionHistoryModel
    {
        public string Id { get; set; }
        public string DepartmentId { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string Description { get; set; }
        public Nullable<int> Type { get; set; }
        public string Path { get; set; }
        public bool IsExportMaterial { get; set; }
    }
}
