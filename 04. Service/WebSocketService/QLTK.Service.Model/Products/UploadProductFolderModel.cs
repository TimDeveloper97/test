using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Products
{
   public class UploadProductFolderModel
    {
        public string ApiUrl { get; set; }
        public string ProductId { get; set; }
        public string Token { get; set; }
        public string Path { get; set; }
        public int DesignType { get; set; }
    }
}
