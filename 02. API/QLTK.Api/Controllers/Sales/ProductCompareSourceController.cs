using NTS.Model.ProductCompareSource;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ProductCompareSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ProductCompareSource
{
    [RoutePrefix("api/ProductCompareSource")]
    [ApiHandleExceptionSystem]
    public class ProductCompareSourceController : BaseController
    {
        private readonly ProductCompareSourceBussiness _business = new ProductCompareSourceBussiness();

        /// <summary>
        /// Tìm kiếm sản phẩm sai khác so với nguồn
        /// </summary>
        /// <param name="modelSearch">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        [Route("SearchProductCompareSource")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120300")]

        public HttpResponseMessage SearchProductCompareSource(ProductCompareSourceSearchModel modelSearch)
        {

            var result = _business.SearchProductCompareSource(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy thông tin sản phẩm sai khác trong kinh doanh
        /// </summary>
        /// <param name="id">Id sản phẩm trong thư viện sản phẩm</param>
        /// <returns></returns>
        [Route("GetProductCompareSourceById/{id}")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F120301")]
        public HttpResponseMessage GetProductCompareSourceById(string id)
        {
            var result = _business.GetProductCompareSourceById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Update danh sách sai khác
        /// </summary>
        /// <param name="model">Danh sách id sale product</param>
        /// <returns></returns>
        [Route("UpdateListSaleProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120302")]
        public HttpResponseMessage UpdateListSaleProduct(UpdateSaleProductModel model)
        {
            var userId = GetUserIdByRequest();
            _business.UpdateListSaleProduct(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Update sai khác với nguồn
        /// </summary>
        /// <param name="id">id sai khác</param>
        /// <returns></returns>
        [Route("UpdateSaleProduct/{id}")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120302")]
        public HttpResponseMessage UpdateSaleProduct(string id)
        {
            var userId = GetUserIdByRequest();
            _business.UpdateSaleProduct(id, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
