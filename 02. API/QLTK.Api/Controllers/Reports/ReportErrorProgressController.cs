using NTS.Model.Report;
using NTS.Model.Reports.ReportErrorProgress;
using NTS.Model.ReportStatusModule;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ReportStatusModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Reports
{
    [RoutePrefix("api/ReportErrorProgress")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F100013")]
    public class ReportErrorProgressController : BaseController
    {
        private readonly ReportErrorProgressBussiness _bussiness = new ReportErrorProgressBussiness();

        [Route("Report")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100015")]
        public HttpResponseMessage Report(ReportErrorProgressSearchModel searchModel)
        {
            var result = _bussiness.Report(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("reportErrorChangePlan")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100015")]
        public HttpResponseMessage reportErrorChangePlan(StatisticErrorChangePlanedModel searchModel)
        {
            var resultECP = _bussiness.reportErrorChangePlan(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, resultECP);
        }

        [Route("GetWork")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100015")]
        public HttpResponseMessage GetWork(ReportErrorProgressSearchModel searchModel)
        {
            var result = _bussiness.GetWork(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //2203
        [Route("GetWorkChangePlan")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100015")]
        public HttpResponseMessage GetWorkChangePlan(ReportErrorProgressSearchModelWorkChangePlanModel searchModel)
        {
            var result = _bussiness.GetWorkChangePlan(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        
        //

        [HttpPost]
        [Route("ExportExcel")]
        [NTSAuthorize(AllowFeature = "F100015")]
        public HttpResponseMessage ExportExcel(ReportErrorSearchModel searchModel)
        {
            try
            {
                string path = _bussiness.ExportExcel(searchModel);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
