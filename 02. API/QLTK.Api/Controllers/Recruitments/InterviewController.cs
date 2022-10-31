using NTS.Model.Applys;
using NTS.Model.Candidates;
using NTS.Model.Recruitments.Interviews;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Recruitments;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Sale
{
    [RoutePrefix("api/interview")]
    [ApiHandleExceptionSystem]
    //[NTSIPAuthorize(AllowFeature = "F120005")]
    public class InterviewController : BaseController
    {
        private readonly InterviewBusiness _interviewBusiness = new InterviewBusiness();

        /// <summary>
        /// Tìm kiếm phỏng vấn
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120000")]
        public HttpResponseMessage SearchInterviews(InterviewSearchModel searchModel)
        {
            var result = _interviewBusiness.SearchInterviews(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Thêm phỏng vấn
        /// </summary>
        /// <returns></returns>
        [Route("create-interview")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120001")]
        public HttpResponseMessage CreateInterview(InterviewModel interviewModel)
        {
            //interviewModel.SBUId = GetSBUIdByRequest();
            //interviewModel.DepartmentId = GetDepartmentIdByRequest();
            //interviewModel.EmployeeId = GetEmployeeIdByRequest();

            _interviewBusiness.CreateInterview(interviewModel, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("create-interview-question")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120001")]
        public HttpResponseMessage CreateInterviewQuestion(InterviewQuestionCreateModel model)
        {
            _interviewBusiness.CreateInterviewQuestion(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin phỏng vấn
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("get-interviewinfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121684")]
        public HttpResponseMessage GetInterviewInfo(InterviewGetInfoModel model)
        {
            var result = _interviewBusiness.GetInterviewInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("get-interview-by-id/{id}")]
        [HttpGet]
        //[NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage GetById(string id)
        {
            var result = _interviewBusiness.GetInterviewById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Xóa phỏng vấn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delete-interview/{id}")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120003")]
        public HttpResponseMessage DeleteInterview(string id)
        {
            _interviewBusiness.DeleteInterview(id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ExportExcel")]
        [HttpPost]
        public HttpResponseMessage ExportExcel(InterviewSearchModel searchModel)
        {
            var result = _interviewBusiness.ExportExcel(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetInfoInterviewByRecruitmentRequestId/{id}")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120000")]
        public HttpResponseMessage GetInfoInterviewByRecruitmentRequestId(string Id)
        {
            var result = _interviewBusiness.GetInfoInterviewByRecruitmentRequestId(Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListQuestions/{id}")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120000")]
        public HttpResponseMessage GetListQuestions(string Id)
        {
            var result = _interviewBusiness.GetListQuestions(Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("delete/{id}")]
        [HttpPost]
        public HttpResponseMessage Delete(string id)
        {
            _interviewBusiness.Delete(id, GetUserIdByRequest());
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("getEmployee/{id}")]
        [HttpGet]
        public HttpResponseMessage getEmployee(string id)
        {
            var result = _interviewBusiness.getEmployee(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Sửa người phỏng vấn
        /// </summary>
        /// <returns></returns>
        [Route("updateEmployee/{id}")]
        [HttpPost]
        public HttpResponseMessage UpdateEmployee(string id, List<InterviewUserInfoModel> model)
        {
            _interviewBusiness.UpdateEmployee(id, model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Sửa câu hỏi phỏng vấn
        /// </summary>
        /// <returns></returns>
        [Route("updateQuestions/{id}")]
        [HttpPost]
        public HttpResponseMessage UpdateQuestions(string id, List<InterviewQuestionModel> model)
        {
            _interviewBusiness.UpdateQuestions(id, model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
