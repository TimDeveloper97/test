using NTS.Model.Holiday;
using NTS.Model.ImportProfileDocumentConfigs;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Holidays;
using QLTK.Business.ImportProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ImportProfiles
{
    [RoutePrefix("api/ImportProfileReport")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F120653;F120663")]
    public class ImportProfileReportController : BaseController
    {
        ImportProfileReportBussiness _importProfileReportBussiness = new ImportProfileReportBussiness();

        [Route("Ongoing")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120651")]
        public HttpResponseMessage ReporOngoing(ImportProfileSearchModel searchModel)
        {
            var result = _importProfileReportBussiness.ReporOngoing(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("OngoingExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120652")]
        public HttpResponseMessage OngoingExportExcel(ImportProfileSearchModel searchModel)
        {
            var result = _importProfileReportBussiness.OngoingExportExcel(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("Summary")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120661")]
        public HttpResponseMessage ReporSummary(ImportProfileSearchModel searchModel)
        {
            var result = _importProfileReportBussiness.ReporSummary(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SummaryExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120662")]
        public HttpResponseMessage SummaryExportExcel(ImportProfileSearchModel searchModel)
        {
            var result = _importProfileReportBussiness.SummaryExportExcel(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
