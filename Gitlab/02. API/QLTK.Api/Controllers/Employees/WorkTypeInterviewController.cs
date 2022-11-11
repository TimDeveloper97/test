using NTS.Model.Question;
using NTS.Model.WorkTypeInterview;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.WorkTypeInterview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.WorkTypeInterview
{
    [RoutePrefix("api/worktypeinterview")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F080506")]
    public class WorkTypeInterviewController : BaseController
    {
        private readonly WorkTypeInterviewBussiness _business = new WorkTypeInterviewBussiness();

        /// <summary>
        /// Tìm kiếm phỏng vấn vị trí công việc
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080501;F080502")]
        public HttpResponseMessage SearchWorkTypeInterview(WorkTypeInterviewSearchModel searchModel)
        {
            var result = _business.SearchWorkTypeInterview(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xóa phỏng vấn vị trí công việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080503")]
        public HttpResponseMessage DeleteJobPositions(WorkTypeInterviewModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteWorkTypeInterview(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm phỏng vấn vị trí công việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080501")]
        public HttpResponseMessage AddJobPositions(WorkTypeInterviewModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateWorkTypeInterview(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin phỏng vấn vị trí công việc
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getWorkTypeInterviewInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080501;F080502")]
        public HttpResponseMessage GetWorkTypeInterview(WorkTypeInterviewModel modelSearch)
        {

            var result = _business.GetWorkTypeInterview(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật chức vụ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080501")]
        public HttpResponseMessage UpdateJobPositions(WorkTypeInterviewModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateWorkTypeInterview(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
            
        [Route("search-question")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F080501;F080502")]
        public HttpResponseMessage SearchQuestion(QuestionSearchModel modelSearch)
        {
            var result = _business.SearchQuestion(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
