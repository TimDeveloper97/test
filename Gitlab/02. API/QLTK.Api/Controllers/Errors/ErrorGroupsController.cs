using NTS.Model.ErrorGroup;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ErrorGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ErrorGroup
{
    [RoutePrefix("api/ErrorGroup")]
    [ApiHandleExceptionSystemAttribute]

    public class ErrorGroupsController : BaseController
    {
        private readonly ErrorGroupBusiness _business = new ErrorGroupBusiness();

        [Route("SearchErrorGroup")]
        [HttpPost]
        public HttpResponseMessage SearchErrorGroup(ErrorGroupModel modelSearch)
        {
            var result = _business.SearchErrorGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddErrorGroup")]
        [HttpPost]
        public HttpResponseMessage AddErrorGroup(ErrorGroupModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddErrorGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateErrorGroup")]
        [HttpPost]
        public HttpResponseMessage UpdateErrorGroup(ErrorGroupModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateErrorGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteErrorGroup")]
        [HttpPost]
        public HttpResponseMessage DeleteCustomerType(ErrorGroupModel model)
        {
            _business.DeleteErrorGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetErrorGroupInfo")]
        [HttpPost]
        public HttpResponseMessage GetErrorGroupInfo(ErrorGroupModel model)
        {
            var result = _business.GetErrorGroupInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
