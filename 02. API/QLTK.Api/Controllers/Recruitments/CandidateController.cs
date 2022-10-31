using NTS.Model.Candidates;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Recruitments;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Sale
{
    [RoutePrefix("api/candidate")]
    [ApiHandleExceptionSystem]
    //[NTSIPAuthorize(AllowFeature = "F120005")]
    public class CandidateController : BaseController
    {
        private readonly CandidateBusiness _candidateBusiness = new CandidateBusiness();

        /// <summary>
        /// Tìm kiếm ứng viên
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120000")]
        public HttpResponseMessage SearchCandidate(CandidateSearchModel searchModel)
        {
            var result = _candidateBusiness.SearchCandidates(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("search-by-recruitment-request-id/{id}")]
        [HttpGet]
        //[NTSAuthorize(AllowFeature = "F120000")]
        public HttpResponseMessage SearchCandidatesByRecruitmentRequestId(string id)
        {
            var result = _candidateBusiness.SearchCandidatesByRecruitmentRequestId(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Tự động tạo mã 
        /// </summary>
        /// <returns></returns>
        [Route("GenerateCode")]
        [HttpGet]
        public HttpResponseMessage GenerateCode()
        {
            var result = _candidateBusiness.GenerateCode();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Thêm ứng viên
        /// </summary>
        /// <returns></returns>
        [Route("create-candidate")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120001")]
        public HttpResponseMessage CreateCandidate(CandidateCreateModel candidateCreateModel)
        {
            _candidateBusiness.CreateCandidate(candidateCreateModel, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Sửa ứng viên
        /// </summary>
        /// <returns></returns>
        [Route("update-candidate/{id}")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120002")]
        public HttpResponseMessage UpdateCandidate(string id, CandidateCreateModel model)
        {
            _candidateBusiness.UpdateCandidate(id, model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Cập thin thông tin follow
        /// </summary>
        /// <returns></returns>
        [Route("update-follow/{id}")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120002")]
        public HttpResponseMessage UpdateFollow(string id, List<CandidateFollowModel> candidateFollows)
        {
            _candidateBusiness.UpdateFollow(id, candidateFollows);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("get-candidate-by-id/{id}")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F121632")]
        public HttpResponseMessage GetCandidateById(string id)
        {
            var result = _candidateBusiness.GetCandidateById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("get-follow/{id}")]
        [HttpGet]
        //[NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage GetFollows(string id)
        {
            var result = _candidateBusiness.GetFollow(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("get-applys/{id}")]
        [HttpGet]
        //[NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage GetApplys(string id)
        {
            var result = _candidateBusiness.GetApplys(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("delete-candidate")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120003")]
        public HttpResponseMessage DeleteCandidate(CandidateCreateModel candidateModel)
        {
            _candidateBusiness.DeleteCandidate(candidateModel.Id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ExportExcel")]
        [HttpPost]
        public HttpResponseMessage ExportExcel(CandidateSearchModel searchModel)
        {
            var result = _candidateBusiness.ExportExcel(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("cancel-candidate/{id}")]
        [HttpPost]
        public HttpResponseMessage ExportExcel(string id)
        {
            _candidateBusiness.CancelCandidate(id);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
