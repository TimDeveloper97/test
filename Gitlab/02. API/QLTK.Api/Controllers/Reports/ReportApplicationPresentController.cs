using NTS.Model.ReportApplicationPresent;
using QLTK.Api.Attributes;
using QLTK.Business.ReportApplicationPresent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ReportApplication
{
    [RoutePrefix("api/Reports")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F100013")]
    public class ReportApplicationPresentController : ApiController
    {
        private readonly ReportApplicationPresentBussiness reportApplicationPresent = new ReportApplicationPresentBussiness();

        [Route("GetReportApplicationPresent")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100011")]
        public HttpResponseMessage GetReportApplicationPresent(ReportApplicationPresentSearchModel searchModel)
        {
            var result = reportApplicationPresent.ReportApplicationPresent(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
