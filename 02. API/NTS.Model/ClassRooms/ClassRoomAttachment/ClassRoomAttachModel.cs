using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoomAttach
{
    public class ClassRoomAttachModel : BaseModel
    {
        public string Id { get; set; }
        public string ClassRoomId { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public string Path { get; set; }
    }
}
