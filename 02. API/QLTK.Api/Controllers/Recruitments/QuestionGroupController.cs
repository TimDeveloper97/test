using NTS.Model.QuestionGroup;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.QuestionGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.QuestionGroup
{
    [RoutePrefix("api/question-group")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F120904")]
    public class QuestionGroupController : BaseController
    {
        private readonly QuestionGroupBussiness _business = new QuestionGroupBussiness();

        /// <summary>
        /// Tìm kiếm nhóm câu hỏi
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120900")]
        public HttpResponseMessage SearchQuestionGroup(QuestionGroupSearchModel searchModel)
        {
            var result = _business.SearchQuestionGroup(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xóa nhóm câu hỏi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120903")]
        public HttpResponseMessage Delete(QuestionGroupModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteQuestionGroup(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm nhóm câu hỏi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120901")]
        public HttpResponseMessage Add(QuestionGroupModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateQuestionGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin nhóm câu hỏi
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getQuestionGroupInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120902")]
        public HttpResponseMessage Get(QuestionGroupModel modelSearch)
        {

            var result = _business.GetQuestionGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật chức vụ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120902")]
        public HttpResponseMessage Update(QuestionGroupModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateQuestionGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
