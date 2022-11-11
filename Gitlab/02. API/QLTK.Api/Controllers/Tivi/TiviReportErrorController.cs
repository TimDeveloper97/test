using NTS.Model.Report;
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

namespace QLTK.Api.Controllers.Tivi
{
    [RoutePrefix("api/TiviReportError")]
    [ApiHandleExceptionSystem]
    public class TiviReportErrorController : BaseController
    {
        private readonly TiviReportErrorBussiness _bussiness = new TiviReportErrorBussiness();

        [Route("Report")]
        [HttpPost]
        public HttpResponseMessage Report(ReportErrorSearchModel searchModel)
        {
            var result = _bussiness.Report(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetWork")]
        [HttpPost]
        public HttpResponseMessage GetWork(ReportErrorSearchModel searchModel)
        {
            var result = _bussiness.GetWork(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("ExportExcel")]
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
