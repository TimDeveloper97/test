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
    [RoutePrefix("api/salarygroup")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F121514")]
    public class SalaryGroupController : BaseController
    {
        private readonly SalaryGroupBussiness _business = new SalaryGroupBussiness();

        /// <summary>
        /// Tìm kiếm nhóm lương
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121510")]
        public HttpResponseMessage SearchSalaryGroup(SalaryGroupSearchModel searchModel)
        {
            var result = _business.SearchSalaryGroup(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xóa nhóm lương
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121513")]
        public HttpResponseMessage DeleteSalaryGroup(SalaryGroupModel model)
        {
            _business.DeleteSalaryGroup(model, GetUserIdByRequest());
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm nhóm lương
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121511")]
        public HttpResponseMessage AddSalatyGroup(SalaryGroupModel model)
        {
            _business.CreateSalaryGroup(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin nhóm lương
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getSalaryGroupInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121512")]
        public HttpResponseMessage GetSalaryLevel(SalaryGroupModel modelSearch)
        {

            var result = _business.GetSalaryGroupById(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật nhóm lương
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121512")]
        public HttpResponseMessage UpdateSalaryGroup(SalaryGroupModel model)
        {
            _business.UpdateGroup(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
