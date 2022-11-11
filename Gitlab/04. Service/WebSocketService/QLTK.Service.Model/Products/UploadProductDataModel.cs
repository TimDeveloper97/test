using QLTK.Service.Model.Definitions;
using QLTK.Service.Model.Modules;
using QLTK.Service.Model.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class UploadProductDataModel
    {
        public ProductModel Product { get; set; }
        public List<FolderDefinitionModel> FolderDefinitions { get; set; }
        public List<FileDefinitionModel> FileDefinitions { get; set; }
        public List<DataCheckModuleModel> Modules { get; set; }
        public UploadProductDataModel()
        {
            FolderDefinitions = new List<FolderDefinitionModel>();
            FileDefinitions = new List<FileDefinitionModel>();
            Modules = new List<DataCheckModuleModel>();
        }
    }
}
