using NTS.Model.RecruitmentRequest;
using NTS.Model.Recruitments.Applys;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Recruitments;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Recruitments
{
    [RoutePrefix("api/recruitment-request")]
    [ApiHandleExceptionSystem]
    public class RecruitmentRequestController : BaseController
    {
        private readonly RecruitmentRequestBussiness _business = new RecruitmentRequestBussiness();

        /// <summary>
        /// Tìm kiếm yêu cầu tuyển dụng
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        public HttpResponseMessage SearchChannel(RecruitmentRequestSearchModel searchModel)
        {
            var result = _business.SearchRecruitmentRequest(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Tự động tạo mã yêu cầu
        /// </summary>
        /// <returns></returns>
        [Route("GenerateCode")]
        [HttpPost]
        public HttpResponseMessage GenerateCode()
        {
            var result = _business.GenerateCode();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Thêm yêu cầu tuyển dụng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        public HttpResponseMessage CreateRecruitmentRequest(RecruitmentRequestCreateModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateRecruitmentRequest(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("get-by-id/{id}")]
        [HttpGet]
        public HttpResponseMessage GetCandidateById(string id)
        {
            var result = _business.GetRecruitmentRequestById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("get-worktype-salary-by-id/{id}")]
        [HttpGet]
        public HttpResponseMessage GetCompanySalary(string id)
        {
            var result = _business.GetCompanySalary(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Sửa yêu cầu tuyển dụng
        /// </summary>
        /// <returns></returns>
        [Route("update/{id}")]
        [HttpPost]
        public HttpResponseMessage UpdateRecruitmentRequest(string id, RecruitmentRequestInfoModel model)
        {
            _business.UpdateRecruitmentRequest(id, model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Xóa yêu cầu tuyển dụng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete/{id}")]
        [HttpPost]
        public HttpResponseMessage Delete(string id)
        {
            _business.DeleteRecruitmentRequest(id, GetUserIdByRequest());
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("cancel-status/{id}")]
        [HttpPost]
        public HttpResponseMessage cancelStatus(string id)
        {
            _business.CancelStatus(id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("next-status/{id}")]
        [HttpPost]
        public HttpResponseMessage NextStatus(string id)
        {
            _business.NextStatus(id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("back-status/{id}")]
        [HttpPost]
        public HttpResponseMessage BackStatus(string id)
        {
            _business.BackStatus(id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("get-work-type-by-request-id/{id}")]
        [HttpGet]
        public HttpResponseMessage GetWorkTypeByRequestId(string id)
        {
            var result = _business.GetWorkTypeByRequestId(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("get-salary-by-request-id/{id?}")]
        [HttpGet]
        public HttpResponseMessage GetSalaryByRequestId(string id = null)
        {
            var result = _business.GetSalaryByRequestId(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcel")]
        [HttpPost]
        public HttpResponseMessage ExportExcel(RecruitmentRequestSearchModel model)
        {
            string path = _business.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }


        ///// <summary>
        ///// Thêm lần phỏng vấn
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[Route("AddMoreInterviews")]
        //[HttpPost]
        //public HttpResponseMessage AddMoreInterviews(MoreInterviewsModel model)
        //{
        //    _business.AddMoreInterviews(model);
        //    return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        //}
    }
}