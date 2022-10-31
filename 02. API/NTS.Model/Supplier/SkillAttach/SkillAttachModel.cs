﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SkillAttach
{
    public class SkillAttachModel:BaseModel
    {
        public string Id { get; set; }
        public string SkillId { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Note { get; set; }
        public decimal FileSize { get; set; }
    }
}
