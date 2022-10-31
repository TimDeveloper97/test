using QLTK.Service.Model.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class ScanFileModel
    {
        public string ModuleCode { get; set; }
        public string ApiUrl { get; set; }
        public string Token { get; set; }

        public int FileId { get; set; }
        public bool CheckDelete { get; set; }
        public List<FileModel> ListFile { get; set; }
        public List<FolderScanModel> ListFolderScan { get; set; }
        public List<ConfigScanFileModel> ListFileScan { get; set; }
        public ScanFileModel()
        {
            ListFile = new List<FileModel>();
            ListFolderScan = new List<FolderScanModel>();
            ListFileScan = new List<ConfigScanFileModel>();
        }
    }
   
    public class FolderScanModel
    {
        public string FileName { get; set; }
        public int ParentID { get; set; }
        public int ID { get; set; }
        public string FilePath { get; set; }
    }

    public class ConfigScanFileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Nullable<int> Type { get; set; }
        public string PathFolderC { get; set; }
        public Nullable<bool> IsDisplay { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<int> Status { get; set; }
    }

    public class MoveFileModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string PathForder { get; set; }
    }
}
