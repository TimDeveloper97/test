using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SubjectsAttach
{
    public class SubjectsAttachModel : BaseModel
    {
        public string Id { get; set; }
        public string SubjectsId { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public string Path { get; set; }
    }
}
