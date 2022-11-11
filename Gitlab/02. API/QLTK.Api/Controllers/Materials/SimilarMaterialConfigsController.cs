using NTS.Model.Materials;
using NTS.Model.SimilarMaterialConfig;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.SimilarMaterialConfigs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.SimilarMaterialConfig
{
    [RoutePrefix("api/SimilarMaterialConfig")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F000017")]
    public class SimilarMaterialConfigsController : BaseController
    {
        private readonly SimilarMaterialConfigBusiness _business = new SimilarMaterialConfigBusiness();

        [Route("SearchSimilarMaterialConfig")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000900")]
        public HttpResponseMessage SearchSimilarMaterialConfig(SimilarMaterialConfigSearchModel modelSearch)
        {
            var result = _business.SearchSimilarMaterialConfig(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000004")]
        public HttpResponseMessage SearchMaterial(MaterialSearchModel modelSearch)
        {
            var result = _business.SearchMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddSimilarMaterialConfig")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000904")]

        public HttpResponseMessage AddSimilarMaterialConfig(SimilarMaterialConfigModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddSimilarMaterialConfig(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateSimilarMaterialConfig")]
        [HttpPost]
        public HttpResponseMessage UpdateSimilarMaterialConfig(SimilarMaterialConfigModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateSimilarMaterialConfig(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteSimilarMaterialConfig")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000905")]
        public HttpResponseMessage DeleteSimilarMaterialConfig(SimilarMaterialConfigModel model)
        {
            _business.DeleteSimilarMaterialConfig(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetMaterialInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000906")]
        public HttpResponseMessage GetMaterialInfo(MaterialModel model)
        {
            var result = _business.GetMaterialInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000907")]
        public HttpResponseMessage ExportExcel(SimilarMaterialConfigSearchModel model)
        {
            var result = _business.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
