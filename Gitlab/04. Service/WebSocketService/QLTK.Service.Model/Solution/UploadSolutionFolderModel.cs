using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Solution
{
    public class UploadSolutionFolderModel
    {
        public string ApiUrl { get; set; }
        public string FileApiUrl { get; set; }
        public string SolutionId { get; set; }
        public string Token { get; set; }
        public string Path { get; set; }
        public int DesignType { get; set; }
        public int CurentVersion { get; set; }
    }
}
