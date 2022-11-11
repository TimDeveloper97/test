using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Downloads
{
    public class DownloadFolderModel
    {
        public string Id { get; set; }
        public string ObjectId { get; set; }
        public string Name { get; set; }
        public string DownloadPath { get; set; }
        public string ServerPath { get; set; }
        public string LocalPath { get; set; }
        public string ApiUrl { get; set; }
        public string Token { get; set; }
    }

    public class DownloadFileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ServerPath { get; set; }
        public string DownloadPath { get; set; }
        public string ApiUrl { get; set; }
        public string Token { get; set; }
    }
}