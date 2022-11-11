using NTS.Model.MaterialGroup;
using NTS.Model.MaterialParameter;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Materials
{
    [RoutePrefix("api/ConfigMaterial")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F000303")]
    public class ConfigMaterialController : BaseController
    {
        private readonly ConfigMaterialBusiness _business = new ConfigMaterialBusiness();

        [Route("SaveConfig")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000301")]
        public HttpResponseMessage SaveConfig(UpdateMaterialGroupModel model)
        {
            try
            {
                model.CreateBy = GetUserIdByRequest();
                var result = _business.SaveConfig(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetMaterialGroup")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F000302")]
        public HttpResponseMessage GetMaterialGroup()
        {
            try
            {
                var result = _business.GetMaterialGroup();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListMaterialGroup")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F000300")]
        public HttpResponseMessage GetListMaterialGroup()
        {
            try
            {
                var result = _business.GetListMaterialGroup();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
