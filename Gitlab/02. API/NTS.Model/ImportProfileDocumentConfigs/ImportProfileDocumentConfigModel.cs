using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Model.ImportProfileDocumentConfigs
{
    public class ImportProfileDocumentConfigModel
    {
        public int Step { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsRequired { get; set; }
    }
}