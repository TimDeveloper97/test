using NTS.Model.ReasonEndWorking;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ReasonEndWorking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ReasonEndWorking
{
    [RoutePrefix("api/reason-endworking")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F121154")]
    public class ReasonEndWorkingController : BaseController
    {
        private readonly ReasonEndWorkingBussiness _business = new ReasonEndWorkingBussiness();

        /// <summary>
        /// Tìm kiếm lý do nghỉ việc
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121150")]
        public HttpResponseMessage SearchReason(ReasonSearchModel searchModel)
        {
            var result = _business.SearchReason(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xóa lý do nghỉ việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121153")]
        public HttpResponseMessage DeleteJobPositions(ReasonModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteReason(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm lý do nghỉ việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121151")]
        public HttpResponseMessage AddJobPositions(ReasonModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateReason(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin lý do nghỉ việc
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getReasonInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121152")]
        public HttpResponseMessage GetReason(ReasonModel modelSearch)
        {

            var result = _business.GetReason(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật chức vụ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121152")]
        public HttpResponseMessage UpdateJobPositions(ReasonModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateReason(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

    }
}
