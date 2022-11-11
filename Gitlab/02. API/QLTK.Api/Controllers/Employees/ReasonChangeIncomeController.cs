using NTS.Model.ReasonChangeIncome;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ReasonChangeIncome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ReasonChangeIncomeChangeIncome
{
    [RoutePrefix("api/reason-change-income")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F121054")]
    public class ReasonChangeIncomeChangeIncomeController : BaseController
    {
        private readonly ReasonChangeIncomeBussiness _business = new ReasonChangeIncomeBussiness();

        /// <summary>
        /// Tìm kiếm lý do điều chỉnh thu nhập
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121050")]
        public HttpResponseMessage SearchReasonChangeIncome(ReasonChangeIncomeSearchModel searchModel)
        {
            var result = _business.SearchReason(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xóa lý do điều chỉnh thu nhập
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121053")]
        public HttpResponseMessage DeleteJobPositions(ReasonChangeIncomeModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteReason(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm lý do điều chỉnh thu nhập
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121051")]
        public HttpResponseMessage AddJobPositions(ReasonChangeIncomeModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateReason(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin lý do điều chỉnh thu nhập
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getReasonChangeIncomeInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121052")]
        public HttpResponseMessage GetReasonChangeIncome(ReasonChangeIncomeModel modelSearch)
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
        [NTSAuthorize(AllowFeature = "F121052")]
        public HttpResponseMessage UpdateJobPositions(ReasonChangeIncomeModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateReason(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
