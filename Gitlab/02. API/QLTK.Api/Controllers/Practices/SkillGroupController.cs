using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NTS.Model.SkillGroup;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;

namespace QLTK.Api.Controllers.SkillGroup
{

    [RoutePrefix("api/SkillGroup")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F040404")]
    public class SkillGroupController : BaseController
    {
        private readonly Business.SkillGroup.SkillGroup _skillGroup = new Business.SkillGroup.SkillGroup();

        [Route("SearchListSkilltGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040401;F040402")]
        public HttpResponseMessage SearchListSkilltGroup(SkillGroupSearchModel modelSearch)
        {
            var result = _skillGroup.GetListSkillGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //[Route("SearchSkillGroup")]
        //[HttpPost]
        //public HttpResponseMessage SearchSkilGroup(SkillGroupSearchModel modelSearch)
        //{
        //    var result = _skillGroup.SearchSkillGroup(modelSearch);
        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}

        [Route("GetSkillGroup")]
        [HttpPost]
        public HttpResponseMessage GetSkilGroup(SkillGroupModel model)
        {
            var result = _skillGroup.GetByIdSkillGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetSkillGroupInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040402")]
        public HttpResponseMessage GetSkillGroupInfo(SkillGroupModel model)
        {
            var result = _skillGroup.GetSkillGroupInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddSkillGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040401")]
        public HttpResponseMessage AddSkilGroup(SkillGroupModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _skillGroup.AddSkilGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, value: string.Empty);
        }

        [Route("UpdateSkillGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040402")]
        public HttpResponseMessage UpdateSkilGroup(SkillGroupModel model)
        {

            model.UpdateBy = GetUserIdByRequest();
            _skillGroup.UpdateSkillGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteSkilGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040403")]
        public HttpResponseMessage DeleteSkilGroup(SkillGroupModel model)
        {
            _skillGroup.DeleteSkillGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("SearchSkillGroupExpect")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040402")]
        public HttpResponseMessage SearchSkillGroupExcepted(SkillGroupSearchModel modelSearch)
        {
            var result = _skillGroup.GetSkillGroupExcepted(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
