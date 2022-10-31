using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ImportProfile
{
    public class StepImportProfileDocumentModel
    {
        public int Step { get; set; }
        public List<ImportProfileDocumentModel> ListDocument { get; set; }
        public StepImportProfileDocumentModel()
        {
            ListDocument = new List<ImportProfileDocumentModel>();
        }
    }
}
