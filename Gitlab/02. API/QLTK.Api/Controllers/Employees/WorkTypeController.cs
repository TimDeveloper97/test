using NTS.Model.WorkType;
using NTS.Model.WorldSkill;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.WorkTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.WorkType
{
    [RoutePrefix("api/WorkType")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F080506")]
    public class WorkTypeController : BaseController
    {
        private readonly WorkTypeBussiess _business = new WorkTypeBussiess();

        [Route("SearchWorkType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080500")]
        public HttpResponseMessage SearchWorkType(WorkTypeSearchModel modelSearch)
        {
            var result = _business.SearchWorkType(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteWorkType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080503")]
        public HttpResponseMessage DeleteWorkType(WorkTypeModel model)
        {
            string userId = GetUserIdByRequest();
            _business.DeleteWorkType(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("AddWorkType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080501")]
        public HttpResponseMessage AddWorkType(WorkTypeModel model)
        {
            string userId = GetUserIdByRequest();
            _business.AddWorkType(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateWorkType")]
        [NTSAuthorize(AllowFeature = "F080502")]
        [HttpPost]
        public HttpResponseMessage UpdateWorkType(WorkTypeModel model)
        {
            string userId = GetUserIdByRequest();
            _business.UpdateWorkType(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetWorkTypeInfo")]
        [NTSAuthorize(AllowFeature = "F080502;F080504")]
        [HttpPost]
        public HttpResponseMessage GetWorkTypeInfo(WorkTypeModel model)
        {
            var result = _business.GetWorkTypeInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchWorkSkill")]
        [NTSAuthorize(AllowFeature = "F080501;F080502")]
        [HttpPost]
        public HttpResponseMessage SearchWorkSkill(WorkSkillModel modelSearch)
        {
            var result = _business.SearchWorkSkill(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchEmployeeWorkSkill")]
        [NTSAuthorize(AllowFeature = "F080501;F080502")]
        [HttpPost]
        public HttpResponseMessage SearchEmployeeWorkSkill(WorkSkillModel modelSearch)
        {
            var result = _business.SearchEmployeeWorkSkill(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
