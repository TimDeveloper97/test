using NTS.Model.Practice;
using NTS.Model.PracticeProduct;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.PracticeProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.PracticeInProduct
{

    [RoutePrefix("api/PracticeInProduct")]
    [ApiHandleExceptionSystemAttribute]
    [Authorize]

    public class PracticeInProductController : BaseController
    {
        private readonly PracticeProductBussiness _bussiness = new PracticeProductBussiness();

        [Route("SearchPracticeSubMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040706;")]
        public HttpResponseMessage SearchPracticeSubMaterial(PracticeProductSearchModel modelSearch)
        {
            var result = _bussiness.SearchPracticeProduct(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchPracticeModule")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040706;")]
        public HttpResponseMessage SearchPracticeModule(PracticeProductSearchModel modelSearch)
        {
            var result = _bussiness.SearchPracticeModule(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcelPracticeProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040706")]
        public HttpResponseMessage ExportExcelPracticeProduct(PracticeProductSearchModel modelSearch)
        {
            var result = _bussiness.ExportExcelPracticeProduct(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("AddModuleInPractice")]
        [HttpPost]
        public HttpResponseMessage AddModuleInPractice(PracticeModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _bussiness.AddModuleInPratice(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
