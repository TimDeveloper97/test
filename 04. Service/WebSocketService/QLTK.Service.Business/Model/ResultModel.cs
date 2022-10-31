using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class ResultModel
    {
        public object Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public const int StatusCodeSuccess = 1;
        public const int StatusCodeError = 2;
    }

    public class ListMaterialResult
    {
        public List<ModuleMaterialModel> ListModuleMaterial { get; set; }
        public List<MaterialFromDBModel> ListMaterial { get; set; }
        public string MessageError { get; set; }
        public ListMaterialResult()
        {
            ListModuleMaterial = new List<ModuleMaterialModel>();
            ListMaterial = new List<MaterialFromDBModel>();
        }
    }

    public class CheckFolderResult
    {
        public bool Status { get; set; }
        public ListMaterialResult Result { get; set; }
    }
}
