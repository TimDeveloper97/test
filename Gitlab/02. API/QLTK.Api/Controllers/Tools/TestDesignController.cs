using NTS.Model.TestDesign;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.TestDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.TestDesign
{
    [RoutePrefix("api/TestDesign")]
    [ApiHandleExceptionSystemAttribute]
    public class TestDesignController : BaseController
    {
        private readonly TestDesignBusiness business = new TestDesignBusiness();
        [HttpPost]
        [Route("Excel")]
        public HttpResponseMessage Excel(ReportTestDesignModel model)
        {
            string path = business.Excel(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);

        }

        [HttpPost]
        [Route("ExportReportDMVT")]
        public HttpResponseMessage ExportReportDMVT(ReportDMVTModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = business.ExportReportDMVT(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);

        }

        [HttpPost]
        [Route("ExportReportTestDesignStruture")]
        public HttpResponseMessage ExportReportTestDesignStruture(ReportTestDesignModel model)
        {
            string path = business.ExportReportTestDesignStructure(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);

        }

        [HttpPost]
        [Route("ExportResultDMVT")]
        public HttpResponseMessage ExportResultDMVT(ReportTestDesignModel model)
        {
            model.Designer = GetUserIdByRequest();
            string path = business.ExportResultDMVT(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);

        }

    }
}
