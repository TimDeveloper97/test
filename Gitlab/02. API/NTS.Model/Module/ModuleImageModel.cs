using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Module
{
    public class ModuleImageModel : BaseModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ThumbnailPath { get; set; }
        public string Note { get; set; }
    }
}
