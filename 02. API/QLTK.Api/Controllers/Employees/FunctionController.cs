using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NTS.Business;
using NTS.Model.Function;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;

namespace QLTK.Api.Controllers.Function
{
    [RoutePrefix("api/Function")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F020705")]
    public class FunctionController : BaseController
    {
        private readonly QLTK.Business.Function.Function _function = new Business.Function.Function();

        [Route("SearchFunction")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020700")]
        public HttpResponseMessage SearchFunction(FunctionSearchModel modelSearch)
        {
            var result = _function.SearchFunction(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddFunction")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020701")]
        public HttpResponseMessage AddFunction(FunctionModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _function.AddFunction(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("getFunction")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020702;F020704")]
        public HttpResponseMessage GetFunction(FunctionModel model)
        {
            var result = _function.GetIdFunction(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateFunction")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020702")]
        public HttpResponseMessage UpdateFunction(FunctionModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _function.UpdateFunction(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteFunction")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020703")]
        public HttpResponseMessage DeleteFunction(FunctionModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _function.DeleteFunction(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
