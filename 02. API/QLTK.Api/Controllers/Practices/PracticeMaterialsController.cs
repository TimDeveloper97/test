using NTS.Model.Materials;
using NTS.Model.PracticeMaterial;
using QLTK.Api.Attributes;
using QLTK.Business.PracticeMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.PracticeMaterial
{
    [RoutePrefix("api/PracticeMaterial")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F040730")]
    public class PracticeMaterialsController : ApiController
    {
        private readonly PracticeMaterialBussiness _bussiness = new PracticeMaterialBussiness();

        [Route("SearchModuleInPractice")]
        [HttpPost]
        public HttpResponseMessage SearchModuleInPractice(PracticeMaterialSearchModel modelSearch)
        {
            var result = _bussiness.SearchModuleInPractice(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchPracticeMaterial")]
        [HttpPost]
        public HttpResponseMessage SearchPracticeSubMaterial(PracticeMaterialSearchModel modelSearch)
        {
            var result = _bussiness.SearchPracticeMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchMaterial")]
        [HttpPost]
        public HttpResponseMessage SearchMaterial(MaterialSearchModel modelSearch)
        {
            var result = _bussiness.SearchMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddPracticeMaterial")]
        [HttpPost]
        public HttpResponseMessage AddPracticeMaterial(PracticeMaterialModel model)
        {
            _bussiness.AddPracticeMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ExportExcelPracticeMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040708")]
        public HttpResponseMessage ExportExcelPracticeMaterial(PracticeMaterialSearchModel modelSearch)
        {
            var result = _bussiness.ExportExcelPracticeMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
