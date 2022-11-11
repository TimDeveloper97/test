using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ConfigScanFile
{
    public class ConfigScanFileModel:BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Nullable<int> Type { get; set; }
        public string PathFolderC { get; set; }
        public Nullable<bool> IsDisplay { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
