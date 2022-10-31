using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model
{
    public class SolutionAttachModel : BaseModel
    {
        public string Id { get; set; }
        public string SolutionId { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string Path { get; set; }
        public int Type { get; set; }
    }
}
