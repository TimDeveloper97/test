﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectProductDocument
{
    public class DownloadProjectProductModel
    {
        public string ProjectId { get; set; }
        public string ApiUrl { get; set; }
        public string DownloadPath { get; set; }
        public string Token { get; set; }
    }
}
