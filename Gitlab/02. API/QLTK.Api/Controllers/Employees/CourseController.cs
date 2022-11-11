using NTS.Model.Course;
using NTS.Model.WorldSkill;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Courses
{
    /// <summary>
    /// Quản lý khóa học
    /// </summary>
    [RoutePrefix("api/Course")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F080207")]
    public class CourseController : BaseController
    {
        private readonly CourseBussiness _business = new CourseBussiness();
        [Route("SearchCourse")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080200")]
        public HttpResponseMessage SearchCourse(CourseSearchModel modelSearch)
        {
            var result = _business.SearchCourse(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchCourseSkill")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080204;F080201;F080202")]
        //[NTSAuthorize(AllowFeature = "F080200")]
        public HttpResponseMessage SearchCourseSkill(WorkSkillModel modelSearch)
        {
            var result = _business.SearchCourseSkill(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CreateCourse")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080201")]
        public HttpResponseMessage CreateCourse(CourseModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateCourse(model, this.GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateCourse")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080202")]
        public HttpResponseMessage UpdateCourse(CourseModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateCourse(model, this.GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteCourse")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080203")]
        public HttpResponseMessage DeleteCourse(CourseModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteCourse(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetInfoCourse")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080202;F080204")]
        public HttpResponseMessage GetInfoCourse(CourseModel model)
        {
            var result = _business.GetInfoCourse(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetParentCourse")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F080202;F080204")]
        public HttpResponseMessage GetListParentCourse()
        {
            var result = _business.GetListParentCourse();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
