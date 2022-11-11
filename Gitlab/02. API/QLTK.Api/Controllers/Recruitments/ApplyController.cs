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
    [RoutePrefix("api/apply")]
    [ApiHandleExceptionSystem]
    //[NTSIPAuthorize(AllowFeature = "F120005")]
    public class ApplyController : BaseController
    {
        private readonly ApplyBusiness _applyBusiness = new ApplyBusiness();

        /// <summary>
        /// Tìm kiếm ứng viên
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120000")]
        public HttpResponseMessage SearchApply(ApplySearchModel searchModel)
        {
            var result = _applyBusiness.SearchApplys(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("search-by-recruitment-request-id/{id}")]
        [HttpGet]
        public HttpResponseMessage SearchApplyByRecruitmentRequestId(string id)
        {
            var result = _applyBusiness.SearchApplysByRecruitmentRequestId(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Kiểm tra ứng viên
        /// </summary>
        /// <returns></returns>
        [Route("check-candidate")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120001")]
        public HttpResponseMessage CheckCandidate(ApplyCheckCandidateModel checkCandidateModel)
        {
            var candidates = _applyBusiness.CheckCandidate(checkCandidateModel);
            return Request.CreateResponse(HttpStatusCode.OK, candidates);
        }

        /// <summary>
        /// Thêm ứng viên
        /// </summary>
        /// <returns></returns>
        [Route("create-apply")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120001")]
        public HttpResponseMessage CreateApply(ApplyCreateModel candidateCreateModel)
        {
           var candidates = _applyBusiness.CreateApply(candidateCreateModel, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, candidates);
        }

        /// <summary>
        /// Thêm ứng tuyển
        /// </summary>
        /// <returns></returns>
        [Route("create-candidateapply")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120001")]
        public HttpResponseMessage CreateCandidateApply(CandidateApplyModel candidateapplyCreateModel)
        {
            _applyBusiness.CreateCandidateApply(candidateapplyCreateModel, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Sửa ứng viên
        /// </summary>
        /// <returns></returns>
        [Route("update-apply/{id}")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120002")]
        public HttpResponseMessage UpdateApply(string id, ApplyCreateModel model)
        {
            _applyBusiness.UpdateApply(id, model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("get-apply-by-id/{id}")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F121652")]
        public HttpResponseMessage GetApplyById(string id)
        {
            var result = _applyBusiness.GetApplyById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("get-interviews/{id}")]
        [HttpGet]
        //[NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage GetInterviews(string id)
        {
            var result = _applyBusiness.GetInterviews(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("get-salarylevel-by-id/{id}")]
        [HttpGet]
        //[NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage GetSalaryLevelById(string id)
        {
            var result = _applyBusiness.GetSalaryLevelById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("delete-apply")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120003")]
        public HttpResponseMessage DeleteApply(ApplyCreateModel model)
        {
            _applyBusiness.DeleteApply(model.Id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ExportExcel")]
        [HttpPost]
        public HttpResponseMessage ExportExcel(ApplySearchModel searchModel)
        {
            var result = _applyBusiness.ExportExcel(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddMoreInterviews")]
        [HttpPost]
        public HttpResponseMessage AddMoreInterviews(InterviewSearchResultsModel model)
        {
            _applyBusiness.AddMoreInterviews(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Xóa phỏng vấn
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete/{id}")]
        [HttpPost]
        public HttpResponseMessage Delete(string id)
        {
            _applyBusiness.Delete(id, GetUserIdByRequest());
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("updateMoreInterviews/{id}")]
        [HttpPost]
        public HttpResponseMessage UpdateMoreInterviews(string id, InterviewSearchResultsModel model)
        {
            _applyBusiness.UpdateMoreInterviews(id, model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("getInforMoreInterviews/{id}")]
        [HttpGet]
        //[NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage GetInforMoreInterviews(string id)
        {
            var result = _applyBusiness.GetInforMoreInterviews(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("get-candidate-apply-by-id/{id}")]
        [HttpGet]
        public HttpResponseMessage GetCandidateApplyById(string id)
        {
            var result = _applyBusiness.GetCandidateApplyById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Sửa ứng viên
        /// </summary>
        /// <returns></returns>
        [Route("update-candidate-apply/{id}")]
        [HttpPost]
        public HttpResponseMessage UpdateCandidateApply(string id, ApplyCreateModel model)
        {
            _applyBusiness.UpdateCandidateApply(id, model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// xuất kết quả phỏng ấn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("InterviewExport/{id}")]
        [HttpPost]
        public HttpResponseMessage InterviewExport(string id)
        {
            string path = _applyBusiness.InterviewExport(id);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        /// <summary>
        /// xuất hồ sơ phỏng vấn
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ApplyExport")]
        [HttpPost]
        public HttpResponseMessage ApplyExport(List<ApplySearchResultsModel> model, string idCheck)
        {
            string path = _applyBusiness.ApplyExport(model, idCheck);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

    }
}
