using NTS.Model.Skills;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.PacticeSkillBussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.PracticeSkill
{
    [RoutePrefix("api/PracticeSkill")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F040506")]
    public class PracticeSkillController : BaseController
    {
        private readonly PracticeSkillBussiness _bussiness = new PracticeSkillBussiness();
        // getInfor của Kỹ năng theo bài thực hành
        [Route("GetskillInPractice")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040727")]
        public HttpResponseMessage GetSkillInPractice(SkillsSearchModel modelSearch)
        {
            var result = _bussiness.GetSkillInPractice(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        // chọn kỹ năng trong bài thực hành
        [Route("SearchSkillInPractice")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040725")]
        public HttpResponseMessage SearchSkillInPractice(SkillsSearchModel modelSearch)
        {
            var result = _bussiness.SearchSkillInPractice(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        // khi lưu 
        [Route("AddSkillInPractice")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040725")]
        public HttpResponseMessage AddSkillInPractice(SkillsModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _bussiness.AddSkillInPractice(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
