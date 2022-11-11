using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.HistoryVersion
{
    public class FileDefinitionHistoryModel
    {
        public string FileDefinitionId { get; set; }
        public int TypeDefinitionId { get; set; }
        public string FolderDefinitionId { get; set; }
        public string FileDefinitionNameFirst { get; set; }
        public int FileDefinitionNameBetween { get; set; }
        public string FileDefinitionNameLast { get; set; }
        public int FileDefinitionNameBetweenIndex { get; set; }
        public int FileType { get; set; }
        public int ObjectType { get; set; }
        public string DepartmentId { get; set; }
    }
}
