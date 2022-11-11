using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.HistoryVersion
{
    public class FolderDefinitionHistoryModel
    {
        public string FolderDefinitionId { get; set; }
        public int TypeDefinitionId { get; set; }
        public string FolderDefinitionManageId { get; set; }
        public string FolderDefinitionFirst { get; set; }
        public int FolderDefinitionBetween { get; set; }
        public string FolderDefinitionLast { get; set; }
        public int StatusCheckFile { get; set; }
        public int FolderType { get; set; }
        public int FolderDefinitionBetweenIndex { get; set; }
        public int StatusCheckFolder { get; set; }
        public string ExtensionFile { get; set; }
        public int ObjectType { get; set; }
        public string DepartmentId { get; set; }
    }
}
