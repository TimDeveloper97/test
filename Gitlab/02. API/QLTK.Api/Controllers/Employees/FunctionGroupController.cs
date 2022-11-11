using NTS.Model.FunctionGroup;
using QLTK.Api.Attributes;
using QLTK.Business.FunctionGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.FunctionGroups
{
    [RoutePrefix("api/FunctionGroups")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F020604")]
    public class FunctionGroupController : ApiController
    {
        private readonly FunctionGroupsBussiness _bussiness = new FunctionGroupsBussiness();

        [Route("SearchFunctionGroups")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020700")]
        public HttpResponseMessage SearchFunctionGroups(FunctionGroupSearchModel modelSearch)
        {
            var result = _bussiness.SearchFunctionGroups(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddFunctionGroups")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020601")]
        public HttpResponseMessage AddFunctionGroups(FunctionGroupModel model)
        {

            _bussiness.AddFunctionGroups(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateFunctionGroups")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020602")]
        public HttpResponseMessage UpdateFunctionGroups(FunctionGroupModel model)
        {
            _bussiness.UpdateFunctionGroups(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteFunctionGroups")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020603")]
        public HttpResponseMessage DeleteFunctionGroups(FunctionGroupModel model)
        {
            _bussiness.DeleteFunctionGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetFunctionGroupsInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020602")]
        public HttpResponseMessage GetFunctionGroupsInfo(FunctionGroupModel model)
        {
            var result = _bussiness.GetFunctionGroupInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
