using NTS.Model.InsuranceLevel;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.InsuranceLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.InsuranceLevel
{
    [RoutePrefix("api/insurancelevel")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F030105")]
    public class InsuranceLevelController : BaseController
    {
        private readonly InsuranceLevelBussiness _business = new InsuranceLevelBussiness();

        /// <summary>
        /// Tìm kiếm mức đóng bảo hiểm
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030100")]
        public HttpResponseMessage SearchInsuranceLevel(InsuranceLevelSearchModel searchModel)
        {
            var result = _business.SearchInsuranceLevel(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xóa mức đóng bảo hiểm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030103")]
        public HttpResponseMessage DeleteJobPositions(InsuranceLevelModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteInsuranceLevel(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm mức đóng bảo hiểm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030101")]
        public HttpResponseMessage AddJobPositions(InsuranceLevelModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateInsuranceLevel(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin mức đóng bảo hiểm
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030102;F030104")]
        public HttpResponseMessage GetInsuranceLevel(InsuranceLevelModel modelSearch)
        {

            var result = _business.GetInsuranceLevel(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật chức vụ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030102")]
        public HttpResponseMessage UpdateJobPositions(InsuranceLevelModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateInsuranceLevel(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
