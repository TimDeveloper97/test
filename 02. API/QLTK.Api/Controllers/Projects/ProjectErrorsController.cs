using NTS.Model.Error;
using QLTK.Api.Attributes;
using QLTK.Business.ProjectErrors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ProjectError
{
    [RoutePrefix("api/ProjectError")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F060036")]
    public class ProjectErrorsController : ApiController
    {
        private readonly ProjectErrorBusiness _business = new ProjectErrorBusiness();

        [Route("SearchModuleErrors")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060004")]
        public HttpResponseMessage SearchModuleErrors(ErrorSearchModel model)
        {
            var result = _business.SearchModuleErrors(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetErrorInfo")]
        [HttpPost]
        public HttpResponseMessage GetErrorInfo(ErrorModel model)
        {
            var result = _business.GetErrorInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        /// <summary>
        /// Xuất báo cáo vấn đề tồn đọng
        /// </summary>
        [Route("ExportExcelError/{Id}")]
        [HttpPost]
        public HttpResponseMessage ExportExcelError(string id)
        {
            var result = _business.ExportExcelError(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
