using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Materials
{
    public class DataSheetModel : BaseModel
    {
        public string Id { get; set; }
        public string ManufactureId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int Size { get; set; }

        public string CreateByName { get; set; }
      
    }
}
