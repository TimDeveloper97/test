using NTS.Model.RawMaterial;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.RawMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.RawMaterial
{
    [RoutePrefix("api/RawMaterial")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F000705")]
    public class RawMaterialController : BaseController
    {
        private readonly RawMaterialBusiness _rawMaterialBusiness = new RawMaterialBusiness();
        [Route("SearchRawMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000700")]
        public HttpResponseMessage SearchRawMaterial(RawMaterialSearchModel modelSearch)
        {
            var result = _rawMaterialBusiness.SearchRawMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteRawMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000703")]
        public HttpResponseMessage DeleteRawMaterial(RawMaterialModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _rawMaterialBusiness.DeleteRawMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetRawMaterialInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000704;F000702")]
        public HttpResponseMessage GetRawMaterialInfo(RawMaterialModel model)
        {
            var result = _rawMaterialBusiness.GetRawMaterialInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddRawMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000701")]
        public HttpResponseMessage AddRawMaterial(RawMaterialModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _rawMaterialBusiness.AddRawMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000702")]
        public HttpResponseMessage UpdateRawMaterial(RawMaterialModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _rawMaterialBusiness.UpdateRawMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetListRawMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000704")]
        public HttpResponseMessage GetListRawMaterial()
        {
            var rs = _rawMaterialBusiness.GetListRawMaterial();
            return Request.CreateResponse(HttpStatusCode.OK, rs);
        }
    }
}
