using NTS.Model.ReasonChangeInsurance;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ReasonChangeInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ReasonChangeInsurance
{
    [RoutePrefix("api/reason-change-insurance")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F121104")]
    public class ReasonChangeInsuranceController : BaseController
    {
        private readonly ReasonChangeInsuranceBussiness _business = new ReasonChangeInsuranceBussiness();

        /// <summary>
        /// Tìm kiếm lý do điều chỉnh bảo hiểm xã hội
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121100")]
        public HttpResponseMessage SearchReasonChangeInsurance(ReasonChangeInsuranceSearchModel searchModel)
        {
            var result = _business.SearchReason(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xóa lý do điều chỉnh bảo hiểm xã hội
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121103")]
        public HttpResponseMessage DeleteJobPositions(ReasonChangeInsuranceModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteReason(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm lý do điều chỉnh bảo hiểm xã hội
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121101")]
        public HttpResponseMessage AddJobPositions(ReasonChangeInsuranceModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateReason(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin lý do điều chỉnh bảo hiểm xã hội
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getReasonChangeInsuranceInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121102")]
        public HttpResponseMessage GetReasonChangeInsurance(ReasonChangeInsuranceModel modelSearch)
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
        [NTSAuthorize(AllowFeature = "F121102")]
        public HttpResponseMessage UpdateJobPositions(ReasonChangeInsuranceModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateReason(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
