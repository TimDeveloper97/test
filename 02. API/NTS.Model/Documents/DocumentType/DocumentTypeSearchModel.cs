using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DocumentType
{
    public class DocumentTypeSearchModel: SearchCommonModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
