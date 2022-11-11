using NTS.Model.ClassRoom;
using NTS.Model.Skills;
using NTS.Model.Subjects;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Subjects
{
    [RoutePrefix(prefix: "api/Subjects")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F050006")]
    public class SubjectsController : BaseController
    {
        private readonly SubjectsBussiness _subjects = new SubjectsBussiness();
        private readonly SubjectsBussiness _classRoom = new SubjectsBussiness();


        [Route("SearchSubjects")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050000")]
        public HttpResponseMessage SearchSubjects(SubjectsSearchModel modelSearch)
        {
            var result = _subjects.SearchSubjects(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchClassRoom")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050001;F050002")]
        public HttpResponseMessage SearchClassRoom(ClassRoomSearchModel modelSearch)
        {
            var result = _classRoom.SearchClassRoom(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchSkill")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050001;F050002")]
        public HttpResponseMessage SearchSkill(SkillsSearchModel modelSearch)
        {
            var result = _classRoom.SearchSkill(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddSubjects")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050001")]
        public HttpResponseMessage AddSubjects(SubjectsModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _subjects.AddSubjects(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetSubjects")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050002;F050004")]
        public HttpResponseMessage GetSubjects(SubjectsModel model)
        {
            var result = _subjects.GetIdSubjects(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateSubjects")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050002")]
        public HttpResponseMessage UpdateSubjects(SubjectsModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _subjects.UpdateSubject(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteSubjects")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050003")]
        public HttpResponseMessage DeleteSubjects(SubjectsModel model)
        {
            _subjects.DeleteSubjects(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050005")]
        public HttpResponseMessage ExportExcel(SubjectsSearchModel model)
        {
            try
            {
                string path = _subjects.ExportExcel(model);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
