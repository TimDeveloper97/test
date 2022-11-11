using NTS.Model.Practice;
using NTS.Model.QLTKMODULE;
using NTS.Model.Skills;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Practices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.Practice
{
    [RoutePrefix("api/Practice")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F040730")]
    public class PracticesController : BaseController
    {
        private readonly PracticeBussiness _bussiness = new PracticeBussiness();

        [Route("SearchPractice")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040700")]
        public HttpResponseMessage SearchPractice(PracticeSearchModel modelSearch)
        {
            var result = _bussiness.SearchPractice(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchSkill")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040700")]
        public HttpResponseMessage SearchSkill(SkillsSearchModel modelSearch)
        {
            var result = _bussiness.SearchSkills(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddPractice")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040701")]
        public HttpResponseMessage AddPractice(PracticeModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            var result = _bussiness.AddPractice(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdatePractice")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040702;F040704;F040723")]
        public HttpResponseMessage UpdatePractice(PracticeModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            var result = _bussiness.UpdatePractice(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeletePractice")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040702;F040703")]
        public HttpResponseMessage DeletePractice(PracticeModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _bussiness.DeletePractice(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetPracticeInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040704;F040702")]

        public HttpResponseMessage GetPracticeInfo(PracticeModel model)
        {
            var result = _bussiness.GetPracticeInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040705")]
        public HttpResponseMessage ExportExcel(PracticeSearchModel model)
        {
            var path = _bussiness.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [HttpPost]
        [Route("ImportProductModule")]
        public HttpResponseMessage ImportProductModule()
        {
            var productId = HttpContext.Current.Request.Form["PracticeId"];
            //var projectId = JsonConvert.DeserializeObject<string>(modelJson);
            var userId = GetUserIdByRequest();
            List<ModuleModel> result = new List<ModuleModel>();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                result = _bussiness.ImportModuleProductSketches(userId, hfc[0], productId);
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetContentPractice")]
        [HttpPost]
        public HttpResponseMessage GetContentPractice(string practiceId)
        {
            var data = _bussiness.GetContentPractice(practiceId);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("UpdateContent")]
        [HttpPost]
        public HttpResponseMessage UpdateContent(PracticeContentModel model)
        {
            _bussiness.UpdateContent(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
