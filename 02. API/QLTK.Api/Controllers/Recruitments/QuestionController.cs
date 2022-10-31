using NTS.Model.Question;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Question
{
    [RoutePrefix("api/question")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F120926")]
    public class QuestionController : BaseController
    {
        private readonly QuestionBussiness _business = new QuestionBussiness();
        /// <summary>
        /// Tìm kiếm câu hỏi
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120920")]
        public HttpResponseMessage SearchQuestion(QuestionSearchModel searchModel)
        {
            var result = _business.SearchQuestion(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Xóa câu hỏi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120923")]
        public HttpResponseMessage DeleteJobPositions(QuestionModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteQuestion(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm câu hỏi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120921")]
        public HttpResponseMessage AddJobPositions(QuestionModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateQuestion(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin câu hỏi
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getQuestionInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120922")]
        public HttpResponseMessage GetQuestion(QuestionModel modelSearch)
        {

            var result = _business.GetQuestion(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật chức vụ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120922")]
        public HttpResponseMessage UpdateJobPositions(QuestionModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateQuestion(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
