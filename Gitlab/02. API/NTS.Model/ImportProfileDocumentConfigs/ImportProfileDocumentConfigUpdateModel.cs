using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Model.ImportProfileDocumentConfigs
{
    public class ImportProfileDocumentConfigUpdateModel
    {
        public int Step { get; set; }
        public List<ImportProfileDocumentConfigModel> Documents { get; set; }
    }
}