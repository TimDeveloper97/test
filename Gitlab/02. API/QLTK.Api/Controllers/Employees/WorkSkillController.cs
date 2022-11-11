using NTS.Model.WorkSkill;
using NTS.Model.WorldSkill;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.WorkSkills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.WorkSkill
{
    [RoutePrefix("api/WorkSkill")]
    [ApiHandleExceptionSystemAttribute]
    [NTSIPAuthorize(AllowFeature = "F080304;F080405")]
    public class WorkSkillController : BaseController
    {
        private readonly WorkSkillBussiess _business = new WorkSkillBussiess();

        [Route("SearchWorkSkill")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080400")]
        public HttpResponseMessage SearchWorkSkill(WorkSkillSearchModel modelSearch)
        {
            var result = _business.SearchWorkSkill(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteWorkSkill")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080403")]
        public HttpResponseMessage DeleteWorkSkill(WorkSkillModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteWorkSkill(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("AddWorkSkill")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080401")]
        public HttpResponseMessage AddWorkSkill(WorkSkillModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddWorkSkill(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateWorkSkill")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080402")]
        public HttpResponseMessage UpdateWorkSkill(WorkSkillModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateWorkSkill(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetWorkSkillInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080402;F080404")]
        public HttpResponseMessage GetWorkSkillInfo(WorkSkillModel model)
        {
            var result = _business.GetWorkSkillInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchWorkSkillGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080400")]
        public HttpResponseMessage SearchWorkSkillGroup(WorkSkillGroupModel modelSearch)
        {
            var result = _business.SearchWorkSkillGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddWorkSkillGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080301")]
        public HttpResponseMessage AddWorkSkillGroup(WorkSkillGroupModel model)
        {
            _business.AddWorkSkillGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetWorkSkillGroupInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080302")]
        public HttpResponseMessage GetWorkSkillGroupInfo(WorkSkillGroupModel model)
        {
            var result = _business.GetWorkSkillGroupInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateWorkSkillGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080302")]
        public HttpResponseMessage UpdateManufacture(WorkSkillGroupModel model)
        {
            _business.UpdateWWorkSkillGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteWorkSkillGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080303")]
        public HttpResponseMessage DeleteWorkSkillGroup(WorkSkillGroupModel model)
        {
            _business.DeleteWorkSkillGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("SearchSelectWorkSkill")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F080303")]
        public HttpResponseMessage SearchSelectWorkSkill(WorkSkillModel model)
        {
            var result = _business.SearchSelectWorkSkill(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
