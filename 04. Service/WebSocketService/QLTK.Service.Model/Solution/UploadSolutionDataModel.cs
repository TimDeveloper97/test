using QLTK.Service.Model.Definitions;
using QLTK.Service.Model.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Solution
{
    public class UploadSolutionDataModel
    {
        public SolutionModel Solution { get; set; }
        public List<FolderDefinitionModel> FolderDefinitions { get; set; }
        public List<FileDefinitionModel> FileDefinitions { get; set; }
        public List<DataCheckModuleModel> Modules { get; set; }
        public UploadSolutionDataModel()
        {
            FolderDefinitions = new List<FolderDefinitionModel>();
            FileDefinitions = new List<FileDefinitionModel>();
            Modules = new List<DataCheckModuleModel>();
        }
    }
}
