using NTS.Model.SolutionGroup;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.SolutionGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers
{
    [RoutePrefix("api/SolutionGroup")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F070113")]
    public class SolutionGroupsController : BaseController
    {
        private readonly SolutionGroupBussiness _business = new SolutionGroupBussiness();

        [Route("SearchSolutionGroup")]
        [HttpPost]
        public HttpResponseMessage SearchSolutionGroup(SolutionGroupModel modelSearch)
        {
            var result = _business.SearchSolutionGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddSolutionGroup")]
        [HttpPost]
        public HttpResponseMessage AddSolutionGroup(SolutionGroupModel model)
        {
            _business.AddSolutionGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateSolutionGroup")]
        [HttpPost]
        public HttpResponseMessage UpdateSolutionGroup(SolutionGroupModel model)
        {
            _business.UpdateSolutionGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteSolutionGroup")]
        [HttpPost]
        public HttpResponseMessage DeleteSolutionGroup(SolutionGroupModel model)
        {
            _business.DeleteSolutionGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetSolutionGroupInfo")]
        [HttpPost]
        public HttpResponseMessage GetSolutionGroupInfo(SolutionGroupModel model)
        {
            var result = _business.GetSolutionGroupInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
