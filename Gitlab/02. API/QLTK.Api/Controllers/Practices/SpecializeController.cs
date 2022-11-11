using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NTS.Model.Specialize;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;

namespace QLTK.Api.Controllers.Specialize
{
    [RoutePrefix(prefix: "api/Specialize")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F040105")]
    public class SpecializeController : BaseController
    {
        private readonly Business.Specialize.Specialize _specialize = new Business.Specialize.Specialize();

        [Route("SearchSpecialize")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040100")]
        public HttpResponseMessage SearchSpecialize(SpecializeSearchModel modelSearch)
        {
            var result = _specialize.SearchSpecialize(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetSpecialize")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040102;F040104")]
        public HttpResponseMessage GetSpecialize(SpecializeModel model)
        {
            var result = _specialize.GetByIdSpecialize(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddSpecialize")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040101")]
        public HttpResponseMessage AddSpecialize(SpecializeModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            var result = _specialize.AddSpecialize(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateSpecialize")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040102")]
        public HttpResponseMessage UpdateSpecialize(SpecializeModel model)
        {
            
            model.UpdateBy = GetUserIdByRequest();
            _specialize.UpdateSpecialize(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteSpecialize")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040103")]
        public HttpResponseMessage DeleteSpecialize(SpecializeModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _specialize.DeleteSpecialize(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
