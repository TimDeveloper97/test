using NTS.Model.DesignDocuments;
using NTS.Model.Materials;
using NTS.Model.ModuleMaterials;
using NTS.Model.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleDesignDocument
{
    public class ImportFileModuleModel
    {
        public string ModuleId { get; set; }
        public List<ModuleMaterialModel> Materials { get; set; }
        public List<FolderUploadModel> DesignDocuments { get; set; }
        public List<ModuleDesignerModel> Designers { get; set; }
        public List<ProductModuleModel> ListData { get; set; }
        public string CreateBy { get; set; }
        public int DesignType { get; set; }
        public bool SoftwareExist { get; set; }
        public bool HMIExist { get; set; }
        public bool PLCExist { get; set; }
        public bool FilmExist { get; set; }
        public ImportFileModuleModel()
        {
            Materials = new List<ModuleMaterialModel>();
            DesignDocuments = new List<FolderUploadModel>();
            Designers = new List<ModuleDesignerModel>();
        }
    }

    public class ModuleDesignDocumentModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string ServerPath { get; set; }
        public decimal FileSize { get; set; }
        public string FileType { get; set; }
        public int DesignType { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int TotalFile { get; set; }
        public string  HashValue { get; set; }
    }

    public class TreeListDesignDocumentModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<TreeListDesignDocumentModel> ListFolder { get; set; }
        public List<TreeListDesignDocumentModel> ListFile { get; set; }
        public string Path { get; set; }
        public TreeListDesignDocumentModel()
        {
            ListFolder = new List<TreeListDesignDocumentModel>();
            ListFile = new List<TreeListDesignDocumentModel>();
        }
    }
}
