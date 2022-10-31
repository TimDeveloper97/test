using QLTK.Service.Model.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class ListDMVTResultModel : MaterialModel
    {
        public List<int> ListIndexPart { get; set; }
        public List<string> ListEror { get; set; }
        public ListDMVTResultModel()
        {
            ListIndexPart = new List<int>();
            ListEror = new List<string>();
        }
    }
}
