using NTS.Model.WorkLocation;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.WorkLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.WorkLocation
{
    [RoutePrefix("api/work-location")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F121004")]
    public class WorkLocationController : BaseController
    {
        private readonly WorkLocationBussiness _business = new WorkLocationBussiness();
        /// <summary>
        /// Tìm kiếm nơi làm việc
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121000")]
        public HttpResponseMessage SearchWorkLocation(WorkLocationSearchModel searchModel)
        {
            var result = _business.SearchWorkLocation(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xóa nơi làm việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121003")]
        public HttpResponseMessage DeleteJobPositions(WorkLocationModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteWorkLocation(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm nơi làm việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121001")]
        public HttpResponseMessage AddJobPositions(WorkLocationModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateWorkLocation(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin nơi làm việc
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getWorkLocationInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121002")]
        public HttpResponseMessage GetWorkLocation(WorkLocationModel modelSearch)
        {

            var result = _business.GetWorkLocation(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật chức vụ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121002")]
        public HttpResponseMessage UpdateJobPositions(WorkLocationModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateWorkLocation(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

    }
}
