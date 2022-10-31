using NTS.Model.Course;
using NTS.Model.Employees;
using NTS.Model.EmployeeTraining;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.EmployeeTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.EmployeeTraining
{
    [RoutePrefix("api/EmployeeTraining")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F080605")]
    public class EmployeeTrainingsController : BaseController
    {
        private readonly EmployeeTrainingBusiness _business = new EmployeeTrainingBusiness();

        [Route("SearchEmployeeTraining")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080600")]
        public HttpResponseMessage SearchEmployeeTraining(EmployeeTrainingSearchModel model)
        {
            var result = _business.SearchEmployeeTraining(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchCourse")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080601;F080602")]
        public HttpResponseMessage SearchCourse(CourseSearchModel model)
        {
            var result = _business.SearchCourse(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetEmployeeByCourseId")]
        [HttpPost]
        public HttpResponseMessage GetEmployeeByCourseId(string  courseId)
        {
            var result = _business.GetEmployeeByCouserId(courseId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchEmployee")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080601;F080602")]
        public HttpResponseMessage SearchEmployee(EmployeeSearchModel model)
        {
            var result = _business.SearchEmployee(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddEmployeeTraining")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080601")]
        public HttpResponseMessage AddEmployeeTraining(EmployeeTrainingModel model)
        {

            _business.AddEmployeeTraining(model,GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateEmployeeTraining")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080602")]
        public HttpResponseMessage UpdateEmployeeTraining(EmployeeTrainingModel model)
        {
            _business.UpdateEmployeeTraining(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteEmployeeTraining")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080603")]
        public HttpResponseMessage DeleteEmployeeTraining(EmployeeTrainingModel model)
        {
            _business.DeleteEmployeeTraining(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetEmployeeTrainingInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080602;F080604")]
        public HttpResponseMessage GetEmployeeTrainingInfo(EmployeeTrainingModel model)
        {
            var result = _business.GetEmployeeTrainingInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetWorkKillEndEmployee")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080601;F080602")]
        public HttpResponseMessage GetWorkKillEndEmployee(EmployeeTrainingWorkSkillSearchModel model)
        {
            var result = _business.GetWorkKillEndEmployee(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdatePointEmployee")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080601;F080602")]
        public HttpResponseMessage UpdatePointEmployee(EmployeeTrainingUpdatePointModel model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _business.UpdatePointEmployee(model));
        }

        [Route("GetEmployeeByCourse")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080601;F080602")]
        public HttpResponseMessage GetEmployeeByCourse(List<string> courseIds)
        {
            
            return Request.CreateResponse(HttpStatusCode.OK, _business.GetEmployeeByCourse(courseIds));
        }
    }
}
