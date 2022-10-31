using QLTK.Business.ModuleError;
using NTS.Model.Combobox;
using NTS.Model.ModuleError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Api;

namespace NTS.Api.Controllers.ModuleError
{
    [RoutePrefix("api/ModuleError")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F020129")]
    public class ModuleErrorController : BaseController
    {
        private readonly ModuleErrorBusiness _business = new ModuleErrorBusiness();

        [Route("SearchModuleErrors")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020128")]
        public HttpResponseMessage SearchModuleErrors(ModuleErrorSearchModel model)
        {
            model.DepartmentId = GetDepartmentIdByRequest();
            var result = _business.SearchModuleErrors(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetErrorInfo")]
        [HttpPost]
        public HttpResponseMessage GetErrorInfo(ModuleErrorModel model)
        {
            var result = _business.GetErrorInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}