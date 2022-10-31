using NTS.Model.JobPosition;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.JobPositionss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.JobPosition
{
    [RoutePrefix("api/JobPosition")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F030105")]
    public class JobPositionController : BaseController
    {
        private readonly JobPositionBusiness _business = new JobPositionBusiness();

        /// <summary>
        /// Tìm kiếm chức vụ
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchJobPostitons")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030100")]
        public HttpResponseMessage SearchJobPostitons(JobPositionSearchModel modelSearch)
        {
            var result = _business.SearchJobPostiton(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Get dữ liệu
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("GetJobPositions")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030102;F030104")]
        public HttpResponseMessage GetJobPositions(JobPositionModel modelSearch)
        {

            var result = _business.GetJobPosition(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Xóa chức vụ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteJobPositions")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030103")]
        public HttpResponseMessage DeleteJobPositions(JobPositionModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteJobPosition(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm chức vụ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddJobPositions")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030101")]
        public HttpResponseMessage AddJobPositions(JobPositionModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddJobPosition(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Cập nhật chức vụ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateJobPositions")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030102")]
        public HttpResponseMessage UpdateJobPositions(JobPositionModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateJobPosition(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
