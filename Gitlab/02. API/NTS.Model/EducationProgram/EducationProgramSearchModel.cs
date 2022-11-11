using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.EducationProgram
{
    public class EducationProgramSearchModel : SearchCommonModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string JobId { get; set; }
        public string JobName { get; set; }

        public bool IsExport { get; set; }

        public List<string> ListIdSelect { get; set; }

        public EducationProgramSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
