using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Model.ImportProfileDocumentConfigs
{
    public class ImportProfileKanbanReusltModel
    {
        public int Step { get; set; }
        public bool IsShow { get; set; }
        public string Name { get; set; }
        public int TotalItems { get; set; }
        public List<ImportProfileReusltModel> ImportProfiles { get; set; }

        public ImportProfileKanbanReusltModel()
        {
            ImportProfiles = new List<ImportProfileReusltModel>();
        }
    }
}