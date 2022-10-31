using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.HistoryVersion
{
    public class DesignStructureHistoryModel
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string ParentPath { get; set; }
        public string Path { get; set; }
        public bool IsCheckContent { get; set; }
        public bool IsOpen { get; set; }
        public string Extension { get; set; }
        public string Contain { get; set; }
        public string Description { get; set; }
        public string DepartmentId { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
