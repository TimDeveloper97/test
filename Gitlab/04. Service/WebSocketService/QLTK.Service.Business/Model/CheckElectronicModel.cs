using QLTK.Service.Business.Common;
using QLTK.Service.Model.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class CheckElectronicModel
    {
        public List<CheckUploadEntity> ListError { get; set; }
        public List<string> ListFolder { get; set; }
        public List<string> LstError { get; set; }
        public ResultCheckDMVTModel resultCheckDMVTModel { get; set; }
        public CheckElectronicModel()
        {
            ListError = new List<CheckUploadEntity>();
            LstError = new List<string>();
            ListFolder = new List<string>();
            resultCheckDMVTModel = new ResultCheckDMVTModel();
        }
    }
}
