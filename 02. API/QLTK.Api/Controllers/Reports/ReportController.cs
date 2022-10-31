using NTS.Model.Reports.ReportProgressProjetct;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Report
{
    [RoutePrefix("api/ReportProgressProject")]
    [ApiHandleExceptionSystem]
    public class ReportController : ApiController
    {
        private readonly ReportProgressProjectBusssiness reportProgressProjectBusssiness = new ReportProgressProjectBusssiness();

        [Route("GetReportProcessProject")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F100011")]
        public HttpResponseMessage GetReportApplicationPresent(ReportProgressProjectSearch model)
        {
            var result = reportProgressProjectBusssiness.ReportProgressProjects(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
