using NTS.Model.ReportStatusProduct;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ReportStatusProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ReportStatusProduct
{
    [RoutePrefix("api/ReportStatusProduct")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F100013")]
    public class ReportStatusProductController : BaseController
    {
        private readonly ReportStatusProductBussiness _bussiness = new ReportStatusProductBussiness();

        [Route("GetProduct")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F100010")]
        public HttpResponseMessage SearchReport(ReportStatusProductSearchModel searchModel)
        {
            var result = _bussiness.searchProduct(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("ExportExcel")]
        public HttpResponseMessage ExportExcel(ReportStatusProductSearchModel model)
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
        [Route("ExportExcelProduct")]
        public HttpResponseMessage ExportExcelProduct(ReportStatusProductSearchModel model)
        {
            try
            {
                string path = _bussiness.ExportExcelProduct(model);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
