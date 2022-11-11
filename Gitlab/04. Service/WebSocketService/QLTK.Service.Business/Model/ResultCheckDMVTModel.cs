using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class ResultCheckDMVTModel
    {
        public List<ListDMVTResultModel> ListResult { get; set; }
        public string ListManuError { get; set; }
        public string listPartError { get; set; }
        public string listPartManuError { get; set; }
        public bool IsOK { get; set; }

        public List<ListDMVTResultModel> ListMaterialNotDB { get; set; }

    }
}
