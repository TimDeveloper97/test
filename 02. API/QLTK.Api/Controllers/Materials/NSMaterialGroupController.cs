using Newtonsoft.Json;
using NTS.Model.NSMaterialGroup;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.NSMaterialGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.NSMaterialGroup
{
    [RoutePrefix("api/NSMaterialGroup")]
    [NTSIPAuthorize(AllowFeature = "F000406")]
    [ApiHandleExceptionSystem]
    public class NSMaterialGroupController : BaseController
    {
        private readonly NSMaterialGroupBusiness business = new NSMaterialGroupBusiness();

        [Route("SearchNSMaterialGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000400")]
        public HttpResponseMessage SearchNSMaterialGroup(NSMaterialGroupSearchModel modelSearch)
        {
            var result = business.SearchNSMaterialGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetNSMaterialGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000400")]
        public HttpResponseMessage GetNSMaterialGroup(NSMaterialGroupSearchModel modelSearch)
        {
            var result = business.GetNSMaterialGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetById")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000404;F000402")]
        public HttpResponseMessage GetById(NSMaterialGroupModel model)
        {
            var result = business.GetById(model.Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CreateNSMaterialGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000401")]
        public HttpResponseMessage CreateNSMaterialGroup(NSMaterialGroupModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            var result = business.CreateNSMaterialGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateNSMaterialGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000402;F000405")]
        public HttpResponseMessage UpdateNSMaterialGroup(NSMaterialGroupModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            var result = business.UpdateNSMaterialGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteNSMaterialGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000403")]
        public HttpResponseMessage DeleteNSMaterialGroup(NSMaterialGroupModel model)
        {
            var result = business.Delete(model.Id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
