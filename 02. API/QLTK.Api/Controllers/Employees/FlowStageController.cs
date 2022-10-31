using NTS.Model.FlowStage;
using NTS.Model.OutputResult;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.FlowStage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.FlowStage
{
    [RoutePrefix("api/flow-stage")]
    [ApiHandleExceptionSystem]
   // [NTSIPAuthorize(AllowFeature = "F121574")]
    public class FlowStageController : BaseController
    {
        private readonly FlowStageBussiness _business = new FlowStageBussiness();

        /// <summary>
        /// Tìm kiếm dòng chảy
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
     //   [NTSAuthorize(AllowFeature = "F121570")]
        public HttpResponseMessage SearchFlowStage(FlowStageSearchModel searchModel)
        {
            var result = _business.SearchFlowStage(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Xóa dòng chảy
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
   //     [NTSAuthorize(AllowFeature = "F121573")]
        public HttpResponseMessage Delete(FlowStageModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteFlowStage(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm dòng chảy
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
   //     [NTSAuthorize(AllowFeature = "F121571")]
        public HttpResponseMessage Create(FlowStageModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateFlowStage(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin dòng chảy
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getFlowStageInfo")]
        [HttpPost]
    //    [NTSAuthorize(AllowFeature = "F121572")]
        public HttpResponseMessage GetInfo(FlowStageModel modelSearch)
        {
            var result = _business.GetFlowStage(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật dòng chảy
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
     //   [NTSAuthorize(AllowFeature = "F121572")]
        public HttpResponseMessage Update(FlowStageModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateFlowStage(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Tìm kiếm kết quả đầu ra
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search-outputresult")]
        [HttpPost]
    //    [NTSAuthorize(AllowFeature = "F121571;F121572")]
        public HttpResponseMessage SearchOutputResult(OutputResultSearchModel searchModel)
        {
            var result = _business.SearchOutputResult(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }
    }
}
