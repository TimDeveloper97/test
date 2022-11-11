using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class SelectForderModel
    {
        public List<FolderModel> ListForder { get; set; }
        public string Path { get; set; }
        public SelectForderModel()
        {
            ListForder = new List<FolderModel>();
        }
    }
}
