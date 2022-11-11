using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTSFile.Api.Utilities
{
    public class UploadModel
    {
        public string FolderName { get; set; }
        public string FileName { get; set; }
        public string KeyAuthorize { get; set; }
    }

    public class DownloadModel
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public string KeyAuthorize { get; set; }
    }


    public class ImageResult
    {
        public string FileUrlThum { get; set; }
        public string FileUrl { get; set; }
        public string FilePDFUrl { get; set; }
        public decimal FileSize { get; set; }
        public string FileName { get; set; }
        public string FullFileUrl { get; set; }
    }

    public class ResultDownload
    {
        public string PathZip { get; set; }
        public string Error { get; set; }
    }

    public class ListDataModel
    {
        public string Name { get; set; }
        public List<DownloadModel> ListDatashet { get; set; }
    }
}