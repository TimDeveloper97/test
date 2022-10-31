using NTS.Model;
using NTS.Model.ReportQualityPresent;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ReportQualityPresent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ReportQualityPresent
{
    [RoutePrefix("api/ReportEmployees")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F100013")]
    public class ReportQualityPresentController : BaseController
    {
        private readonly ReportQualityPresentBussiness errorEmployees = new ReportQualityPresentBussiness();

        [Route("GetErrorEmployees")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100008")]
        public HttpResponseMessage SearchReport(ReportQualityPresentSearchModel modelSearch)
        {
            var result = errorEmployees.searchEmployees(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ErrorWithLineProduct")]
        [HttpPost]
        public HttpResponseMessage ErrorWithLineProduct(ReportQualityPresentSearchModel modelSearch)
        {
            var deparmentId = GetDepartmentIdByRequest();
            var result = errorEmployees.ErrorWithLineProduct(modelSearch, deparmentId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ErrorGroup")]
        [HttpPost]
        public HttpResponseMessage ErrorGroup(SearchCommonModel modelSearch)
        {
            var result = errorEmployees.ErrorGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ErrorRatio")]
        [HttpPost]
        public HttpResponseMessage ErrorRatio(ReportQualityPresentSearchModel modelSearch)
        {
            var result = errorEmployees.ErrorRatio(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
