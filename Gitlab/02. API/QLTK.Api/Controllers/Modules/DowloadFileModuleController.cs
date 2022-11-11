using NTS.Model.DataDistribution;
using NTS.Model.ProjectProductDocument;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.DowloadFileModule
{
    [RoutePrefix("api/DowloadFileModule")]
    [ApiHandleExceptionSystemAttribute]

    public class DowloadFileModuleController : BaseController
    {
        private readonly DowloadFileModuleBussiness _business = new DowloadFileModuleBussiness();

        [Route("ListFileDowload")]
        [HttpPost]
        public HttpResponseMessage ListFileDowload(DataDistributionModel model)
        {
            model.DepartmentId = GetDepartmentIdByRequest();
            var result = _business.GetDataDistributions(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListModuleDesignDocument")]
        [HttpPost]
        public HttpResponseMessage GetListModuleDesignDocument()
        {
            var result = _business.GetListModuleDesignDocument();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListModuleMaterial")]
        [HttpPost]
        public HttpResponseMessage GetListModuleMaterial(DataDistributionModel model)
        {
            var result = _business.GetListModuleMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListModuleMaterials")]
        [HttpPost]
        public HttpResponseMessage GetListModuleMaterials(DataDistributionModel model)
        {
            var result = _business.GetListModuleMaterials(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("GetListFileByModuleId")]
        [HttpPost]
        public HttpResponseMessage GetListFileByModuleId(DataDistributionModel model)
        {
            model.DepartmentId = GetDepartmentIdByRequest();
            var result = _business.GetListFileByModuleId(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

    }
}
