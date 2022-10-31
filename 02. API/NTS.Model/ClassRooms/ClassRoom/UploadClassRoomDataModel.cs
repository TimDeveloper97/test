using NTS.Model.DataCheckModuleUpload;
using NTS.Model.FileDefinition;
using NTS.Model.FolderDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoom
{
    public class UploadClassRoomDataModel
    {
        public UploadClassRoomModel ClassRoom { get; set; }
        public List<FolderDefinitionModel> FolderDefinitions { get; set; }
        public List<FileDefinitionModel> FileDefinitions { get; set; }
        public List<DataCheckModuleModel> Modules { get; set; }

        public UploadClassRoomDataModel()
        {
            FolderDefinitions = new List<FolderDefinitionModel>();
            FileDefinitions = new List<FileDefinitionModel>();
            Modules = new List<DataCheckModuleModel>();
        }
    }
}
