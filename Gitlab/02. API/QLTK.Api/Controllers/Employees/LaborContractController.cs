using NTS.Model.LaborContract;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.LaborContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.LaborContract
{
    [RoutePrefix("api/labor-contract")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F120954")]
    public class LaborContractController : BaseController
    {
        private readonly LaborContractBussiness _business = new LaborContractBussiness();

        /// <summary>
        /// Tìm kiếm hợp đồng lao động
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120950")]
        public HttpResponseMessage SearchLaborContract(LaborContractSearchModel searchModel)
        {
            var result = _business.SearchLaborContract(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xóa hợp đồng lao động
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120953")]
        public HttpResponseMessage DeleteJobPositions(LaborContractModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteLaborContract(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm hợp đồng lao động
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120951")]
        public HttpResponseMessage AddJobPositions(LaborContractModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateLaborContract(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin hợp đồng lao động
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getLaborContractInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120952")]
        public HttpResponseMessage GetLaborContract(LaborContractModel modelSearch)
        {

            var result = _business.GetLaborContract(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật chức vụ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120952")]
        public HttpResponseMessage UpdateJobPositions(LaborContractModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateLaborContract(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
