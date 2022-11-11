﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleManualDocument
{
    public class ModuleManualDocumentModel : BaseModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public string Note { get; set; }
        public int FileType { get; set; }
        public bool IsDocument { get; set; }
    }
}
