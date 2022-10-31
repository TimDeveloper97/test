using NTS.Model.ReasonEndWorking;
using NTS.Model.RecruitmentChannels;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ReasonEndWorking;
using QLTK.Business.Recruitments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ReasonEndWorking
{
    [RoutePrefix("api/recruitment-channel")]
    [ApiHandleExceptionSystem]
    //[NTSIPAuthorize(AllowFeature = "F030105")]
    public class RecruitmentChannelController : BaseController
    {
        private readonly RecruitmentChannelBussiness _business = new RecruitmentChannelBussiness();

        /// <summary>
        /// Tìm kiếm kênh tuyển dụng
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F030100")]
        public HttpResponseMessage SearchChannel(RecruitmentChannelSearchModel searchModel)
        {
            var result = _business.SearchChannels(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Xóa kênh tuyển dụng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F030103")]
        public HttpResponseMessage DeletetChannel(RecruitmentChannelModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeletetChannel(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm kênh tuyển dụng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F030101")]
        public HttpResponseMessage CreatetChannel(RecruitmentChannelModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreatetChannel(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông kênh tuyển dụng
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("get-channel-by-id")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F030102;F030104")]
        public HttpResponseMessage GetChannelById(RecruitmentChannelModel modelSearch)
        {

            var result = _business.GetChannelById(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật kênh tuyển dụng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F030102")]
        public HttpResponseMessage UpdatetChannel(RecruitmentChannelModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdatetChannel(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

    }
}
