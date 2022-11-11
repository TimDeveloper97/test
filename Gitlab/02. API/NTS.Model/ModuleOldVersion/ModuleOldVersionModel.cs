using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleOldVersion
{
    public class ModuleOldVersionModel:BaseModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string Content { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string CreateByName { get; set; }
    }
}
