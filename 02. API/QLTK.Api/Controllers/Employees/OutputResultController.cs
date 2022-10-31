using NTS.Model.Document;
using NTS.Model.FlowStage;
using NTS.Model.OutputResult;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.OutputResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.OutputResult
{
    [RoutePrefix("api/output-result")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F121474")]
    public class OutputResultController : BaseController
    {
        private readonly OutputResultBussiness _business = new OutputResultBussiness();

        /// <summary>
        /// Tìm kiếm kết quả đầu ra
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121470")]
        public HttpResponseMessage SearchOutputResult(OutputResultSearchModel searchModel)
        {
            var result = _business.SearchOutputResult(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xóa kết quả đầu ra
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121473")]
        public HttpResponseMessage Delete(OutputResultModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteOutputResult(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm kết quả đầu ra
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121471")]
        public HttpResponseMessage Create(OutputResultModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateOutputResult(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin kết quả đầu ra
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getOutputResultInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121472")]
        public HttpResponseMessage GetInfo(OutputResultModel modelSearch)
        {

            var result = _business.GetOutputResult(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật kết quả đầu ra
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121472")]
        public HttpResponseMessage Update(OutputResultModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateOutputResult(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Tìm kiếm lựa chọn dòng chảy
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search-flowstage")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121471;F121472")]
        public HttpResponseMessage SearchFlowStage(FlowStageSearchModel searchModel)
        {
            var result = _business.SearchFlowStage(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Tìm kiếm lựa chọn tài liệu
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search-document")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121471;F121472")]
        public HttpResponseMessage SearcChooseDocument(DocumentSearchModel searchModel)
        {
            var result = _business.SearchChooseDocument(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }
    }
}
