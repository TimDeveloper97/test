using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ImportProfile
{
    public class ImportCodeModel
    {
        public int Index { get; set; }
        public string Code { get; set; }
        public List<ListFileConfigModel> ListFile { get; set; }
        public ImportCodeModel()
        {
            ListFile = new List<ListFileConfigModel>();
        }
    }
}
