using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SolutionOldVersion
{
    public class SolutionOldVersionModel
    {
        public string Id { get; set; }
        public string SolutionId { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }
        public string CreateByName { get; set; }
        public DateTime CreateDate { get; set; }
        public List<DesignModel> ListDesign { get; set; }
        public SolutionOldVersionModel()
        {
            ListDesign = new List<DesignModel>();
        }
    }

    public class DesignModel
    {
        public string Name { get; set; }
        public string FolderName { get; set; }
        public bool IsDownload { get; set; }
        public string FolderId { get; set; }
        public int DesignType { get; set; }
    }
}
