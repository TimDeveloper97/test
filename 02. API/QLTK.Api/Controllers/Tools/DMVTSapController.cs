using NTS.Model.DMVTImportSAP;
using NTS.Model.QLTKMODULE;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.DMVTimportSAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.DownloadDMVTSap
{
    [RoutePrefix("api/downloadDMVTSAP")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F091102")]
    public class DMVTSapController : BaseController
    {
        private readonly DMVTImportSAPBussiness _business = new DMVTImportSAPBussiness();

        [Route("GetModuleInProjectProductByProjectId")]
        [HttpPost]
        public HttpResponseMessage GetModuleInProjectProductByProjectId(DesignModuleInfoModel model)
        {
            var result = _business.GetModuleInProjectProductByProjectId(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GenerateMaterialSAP")]
        [HttpPost]
        public HttpResponseMessage GenerateMaterialSAP(DesignModuleInfoModel model)
        {
            
            var result = _business.GenerateMaterialSAP(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("ImportModule")]
        [HttpPost]
        public HttpResponseMessage ImportModule()
        {
            string userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            var results = _business.ImportModule(userId, hfc[0]);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
    }
}
