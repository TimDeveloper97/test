using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.PracticeFile
{
    public class PracticeFileModel : BaseModel
    {
        public string Id { get; set; }
        public string PracticeId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        public string CreateByName { get; set; }
        public bool IsDocument { get; set; }
        public List<PracticeFileModel> ListFile { get; set; }
    }
}
