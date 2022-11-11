using NTS.Model.Practice;
using NTS.Model.PracticeSkill;
using NTS.Model.Skills;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Skills
{
    [RoutePrefix("api/Skills")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F080020")]
    public class SkillController : BaseController
    {
        private readonly SkillsBussiness _business = new SkillsBussiness();

        [Route("SearchSkills")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040500")]
        public HttpResponseMessage SearchSkills(SkillsSearchModel modelSearch)
        {
            var result = _business.SearchSkills(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchPractice")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040502;F040501;F030407")]
        public HttpResponseMessage SearchPractice(PracticeSearchModel modelSearch)
        {
            var result = _business.SearchPractice(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddSkills")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040501")]
        public HttpResponseMessage AddSkills(SkillsModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddSkills(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("AddPracticeSkill")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040502;F040501")]
        public HttpResponseMessage AddPracticeSkill(PracticeModel model)
        {
            _business.AddPracticeSkill(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateSkills")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040502")]
        public HttpResponseMessage UpdateSkills(SkillsModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateSkills(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteSkills")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040503")]
        public HttpResponseMessage DeleteSkills(SkillsModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteSkill(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeletePracticeSkill")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040502;F040501")]
        public HttpResponseMessage DeletePracticeSkill(PracticeSkillModel model)
        {
            _business.DeletePracticeSkill(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetSkillsInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040504;F040502")]
        public HttpResponseMessage GetSkillsInfo(SkillsModel model)
        {
            var result = _business.GetSkillInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetPracticeSkillInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040504;F040502")]
        public HttpResponseMessage GetPracticeSkillInfo(PracticeSkillModel model)
        {
            var result = _business.GetPracticeSkillInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040505")]
        public HttpResponseMessage ExportExcel(SkillsModel model)
        {
            try
            {
                var path = _business.ExportExcel(model);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
