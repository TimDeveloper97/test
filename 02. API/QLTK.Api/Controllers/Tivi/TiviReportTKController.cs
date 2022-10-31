using NTS.Model.DashBroadProject;
using NTS.Model.Plans;
using NTS.Model.Report;
using NTS.Model.ReportStatusModule;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.DashBroadProject;
using QLTK.Business.Plans;
using QLTK.Business.ReportStatusModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Tivi
{
    [RoutePrefix("api/TiviReportTK")]
    [ApiHandleExceptionSystem]
    public class TiviReportTKController : BaseController
    {
        private readonly PlanBussiness _plan = new PlanBussiness();
        private readonly TiviReportTKBussiness tiviReportTKBussiness = new TiviReportTKBussiness();

        [Route("Search")]
        [HttpPost]
        public HttpResponseMessage SearchWorkingTime(WorkingTimeSearchModel modelSearch)
        {
            var result = _plan.SearchWorkingTime(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchProjects")]
        [HttpPost]
        public HttpResponseMessage SearchListEmployee(DashBroadProjectSearchModel model)
        {
            var result = tiviReportTKBussiness.GetDashBroadProject(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

    }
}