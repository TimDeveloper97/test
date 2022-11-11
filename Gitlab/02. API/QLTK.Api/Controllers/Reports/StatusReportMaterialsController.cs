using NTS.Model.ReportStatusMaterial;
using QLTK.Api.Attributes;
using QLTK.Business.StatusReportMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.StatusReportMaterial
{
    [RoutePrefix("api/StatusReportMaterial")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F100013")]
    public class StatusReportMaterialsController : ApiController
    {
        private readonly StatusReportMaterialBussiness _business = new StatusReportMaterialBussiness();

        [Route("GetStatusReportMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100009")]
        public HttpResponseMessage GetStatusReportMaterial(ReportStatusMaterialSearchModel model)
        {
            var result = _business.GetStatusReportMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ReportModuleMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100009")]
        public HttpResponseMessage ReportModuleMaterial(ReportStatusMaterialSearchModel model)
        {
            var result = _business.ReportModuleMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ModuleMaterialCheck3D")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100009")]
        public HttpResponseMessage ModuleMaterialCheck3D()
        {
            var result = _business.ModuleMaterialCheck3D();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcel")]
        [HttpPost]
        public HttpResponseMessage ExportExcel(ReportStatusMaterialSearchModel model)
        {
            var result = _business.Excel(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
