using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Solution
{
    public class UploadFolderSolutionResultModel
    {
        public string CreateBy { get; set; }
        public List<SolutionDesignDocumentModel> ListResult { get; set; }
        public bool IsUploadSuccess { get; set; }
        public UploadFolderSolutionResultModel()
        {
            ListResult = new List<SolutionDesignDocumentModel>();
        }
    }
}
