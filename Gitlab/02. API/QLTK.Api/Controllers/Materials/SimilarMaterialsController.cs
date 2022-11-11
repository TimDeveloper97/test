using NTS.Model.SimilarMaterial;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.SimilarMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.SimilarMaterial
{
    [RoutePrefix("api/SimilarMaterial")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F000908")]
    public class SimilarMaterialsController : BaseController
    {
        private readonly SimilarMaterialBusiness _business = new SimilarMaterialBusiness();

        [Route("SearchSimilarMaterial")]
        [HttpPost]
        public HttpResponseMessage SearchSimilarMaterial(SimilarMaterialSearchModel modelSearch)
        {
            var result = _business.SearchSimilarMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchMaterialInSimilarMaterial")]
        [HttpPost]
        public HttpResponseMessage SearchMaterialInSimilarMaterial(SimilarMaterialSearchModel modelSearch)
        {
            var result = _business.SearchMaterialInSimilarMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddSimilarMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000901")]
        public HttpResponseMessage AddSimilarMaterial(SimilarMaterialModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddSimilarMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateSimilarMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000902")]
        public HttpResponseMessage UpdateSimilarMaterial(SimilarMaterialModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateSimilarMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteSimilarMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000903")]
        public HttpResponseMessage DeleteSimilarMaterial(SimilarMaterialModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeletesimilarMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetSimilarMaterialInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000902")]
        public HttpResponseMessage GetSimilarMaterialInfo(SimilarMaterialModel model)
        {
            var result = _business.GetSimilarMaterialInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
