using NTS.Model.Datasheet;
using NTS.Model.ProductMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleMaterials
{
    public class ModuleMaterialUploadFileModel : BaseModel
    {
        public string Id { get; set; }
        public List<DatasheetModel> ListFileDataSheet { get; set; }
        public List<FileSetUpModel> ListFileSetup { get; set; }
        public string MaterialId { get; set; }
        public string ModuleId { get; set; }
        public string ManufactureId { get; set; }

        public ModuleMaterialUploadFileModel()
        {
            ListFileDataSheet = new List<DatasheetModel>();
            ListFileSetup = new List<FileSetUpModel>();
        }
    }

    public class FileSetUpModel 
    {
        public string Id { get; set; }
        public string ModuleMaterialId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public decimal? Size { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
