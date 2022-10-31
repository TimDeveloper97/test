using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.PracticeOldVersion
{
    public class PracticeOldVersionModel : BaseModel
    {
        public string Id { get; set; }
        public string PracticeId { get; set; }
        public string Content { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }
        public string CreateByName { get; set; }
    }
}
