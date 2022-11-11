using NTS.Model.SalaryLevel;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.SalaryLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.SalaryLevel
{
    [RoutePrefix("api/salarytype")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F121524")]
    public class SalaryTypeController : BaseController
    {
        private readonly SalaryTypeBussiness _business = new SalaryTypeBussiness();

        /// <summary>
        /// Tìm kiếm ngạch lương
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121520")]
        public HttpResponseMessage SearchSalaryType(SalaryTypeSearchModel searchModel)
        {
            var result = _business.SearchSalaryLevel(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xóa ngạch lương
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121523")]
        public HttpResponseMessage DeleteSalaryType(SalaryTypeModel model)
        {
            _business.DeleteSalaryType(model, GetUserIdByRequest());
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm ngạch lương
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121521")]
        public HttpResponseMessage AddSalaryType(SalaryTypeModel model)
        {
            _business.CreateSalaryType(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin ngạch lương
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getSalaryTypeInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121522")]
        public HttpResponseMessage GetSalaryType(SalaryTypeModel modelSearch)
        {

            var result = _business.GetSalaryTypeById(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật ngạch lương
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121522")]
        public HttpResponseMessage UpdateSalaryType(SalaryTypeModel model)
        {
            _business.UpdateSalaryType(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
