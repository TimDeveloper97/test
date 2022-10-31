using NTS.Model.ManufactureGroup;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ManufactureGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ManufactureGroup
{
    [RoutePrefix("api/ManufactureGroup")]
    [ApiHandleExceptionSystemAttribute]

    public class ManufactureGroupsController : BaseController
    {
        private readonly ManufactureGroupBusiness _business = new ManufactureGroupBusiness();

        [Route("SearchManufactureGroup")]
        [HttpPost]
        public HttpResponseMessage SearchManufactureGroup(ManufactureGroupModel modelSearch)
        {
            var result = _business.SearchManufactureGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddManufactureGroup")]
        [HttpPost]
        public HttpResponseMessage AddManufactureGroup(ManufactureGroupModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddManufactureGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateManufactureGroup")]
        [HttpPost]
        public HttpResponseMessage UpdateManufactureGroup(ManufactureGroupModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateManufactureGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteManufactureGroup")]
        [HttpPost]
        public HttpResponseMessage DeleteManufactureGroup(ManufactureGroupModel model)
        {
            _business.DeleteManufactureGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetManufactureGroupInfo")]
        [HttpPost]
        public HttpResponseMessage GetManufactureGroupInfo(ManufactureGroupModel model)
        {
            var result = _business.GetManufactureGroupInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
