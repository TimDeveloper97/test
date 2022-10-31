using NTS.Model.MaterialGroupTPA;
using QLTK.Business.MaterialGroupTPAs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;

namespace QLTK.Api.Controllers.MaterialGroupTPA
{
    [RoutePrefix("api/MaterialGroupTPA")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F000105")]
    public class MaterialGroupTPAController : BaseController
    {
        private readonly MaterialGroupTPABusiness _materialGroupTPABusiness = new MaterialGroupTPABusiness();

        [Route("SearchRawMaterialGroupTPA")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000200")]
        public HttpResponseMessage SearchMaterialGroupTPA(MaterialGroupTPASearchModel modelSearch)
        {
            var result = _materialGroupTPABusiness.SearchMaterialGroupTPA(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteMaterialGroupTPA")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000203")]
        public HttpResponseMessage DeleteMaterialGroupTPA(MaterialGroupTPAModel model)
        {
            _materialGroupTPABusiness.DeleteMaterialGroupTPA(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetMaterialGroupTPAInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000204;F000202")]
        public HttpResponseMessage GetMaterialGroupTPAInfo(MaterialGroupTPAModel model)
        {
            var result = _materialGroupTPABusiness.GetMaterialGroupTPAInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddMaterialGroupTPA")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000201")]
        public HttpResponseMessage AddMaterialGroupTPA(MaterialGroupTPAModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _materialGroupTPABusiness.AddMaterialGroupTPA(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateMaterialGroupTPA")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000202")]
        public HttpResponseMessage UpdateMaterialGroupTPA(MaterialGroupTPAModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _materialGroupTPABusiness.UpdateMaterialGroupTPA(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }


    }
}
