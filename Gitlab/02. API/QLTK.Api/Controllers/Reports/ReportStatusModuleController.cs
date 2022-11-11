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

namespace QLTK.Api.Controllers.ReportStatusModule
{
    [RoutePrefix("api/ReportStatusModule")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F100013")]
    public class ReportStatusModuleController : BaseController
    {
        private readonly ReportStatusModuleBussiness _bussiness = new ReportStatusModuleBussiness();

        [Route("GetModule")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100010")]
        public HttpResponseMessage SearchReport(ReportStatusModuleSearchModel searchModel)
        {
            var result = _bussiness.searchModule(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("ExportExcel")]
        public HttpResponseMessage ExportExcel(ReportStatusModuleSearchModel model)
        {
            try
            {
                string path = _bussiness.ExportExcel(model);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("ExportExcelModule")]
        public HttpResponseMessage ExportExcelModule(ReportStatusModuleSearchModel model)
        {
            try
            {
                string path = _bussiness.ExportExcelModule(model);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
