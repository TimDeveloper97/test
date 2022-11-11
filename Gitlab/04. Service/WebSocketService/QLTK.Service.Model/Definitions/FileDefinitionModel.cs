using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Definitions
{
    public class FileDefinitionModel
    {
        public string FileDefinitionId { get; set; }
        public string FolderDefinitionId { get; set; }
        public string FileDefinitionNameFirst { get; set; }
        public int FileDefinitionNameBetween { get; set; }
        public string FileDefinitionNameLast { get; set; }
        public int FileDefinitionNameBetweenIndex { get; set; }
        public int FileType { get; set; }
        public int TypeDefinitionId { get; set; }
    }
}
