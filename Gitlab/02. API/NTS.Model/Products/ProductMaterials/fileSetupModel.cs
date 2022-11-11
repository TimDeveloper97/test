using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductMaterials
{
    public class FileSetupModel
    {
        public string Id { get; set; }
        public string ModuleMaterialId { get; set; }
        public string ProductId { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public decimal? Size { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
