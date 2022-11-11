using NTS.Model.SaleGroups;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.SaleGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.SaleGroups
{
    [RoutePrefix("api/SaleGroup")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F120404")]
    public class SaleGroupController : BaseController
    {
        private readonly SaleGroupBussiness saleGroup = new SaleGroupBussiness();

        /// <summary>
        /// Lấy danh sách nhóm kinh doanh
        /// </summary>
        /// <param name="modelSearch">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        [Route("SearchSaleGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120400")]
        public HttpResponseMessage SearchSaleGroup(SearchSaleGroupModel modelSearch)
        {
            var result = saleGroup.SearchSaleGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy thông tin nhóm kinh doanh
        /// </summary>
        /// <param name="id">id nhóm kinh doanh</param>
        /// <returns></returns>
        [Route("GetInfoSaleGroup")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F120402")]
        public HttpResponseMessage GetInfoSaleGroup(string id)
        {
            var result = saleGroup.GetInfoSaleGroup(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Thêm nhóm kinh doanh
        /// </summary>
        /// <param name="model">Dữ liệu thêm vào</param>
        [Route("CreateSaleGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120401")]
        public HttpResponseMessage CreateSaleGroup(SaleGroupCreateModel model)
        {
            saleGroup.CreateSaleGroup(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, value: string.Empty);
        }

        /// <summary>
        /// Cập nhật nhóm kinh doanh
        /// </summary>
        /// <param name="id">id nhóm</param>
        /// <param name="model">dữ liệu cập nhật</param>
        [Route("UpdateSaleGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120402")]
        public HttpResponseMessage UpdateSaleGroup(string id, SaleGroupCreateModel model)
        {
            saleGroup.UpdateSaleGroup(id, model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Xóa nhóm kinh doanh
        /// </summary>
        /// <param name="id">Nhóm kinh doanh</param>
        [Route("DeleteSaleGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120403")]
        public HttpResponseMessage DeleteSaleGroup(string id)
        {
            saleGroup.DeleteSaleGroup(id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy ra danh sách nhân viên khác nhân viên được chọn.
        /// </summary>
        /// <param name="searchModel">Danh sách id nhân viên được chọn lúc đầu.</param>
        /// <returns></returns>
        [Route("GetListEmployee")]
        [HttpPost]
        public HttpResponseMessage GetListEmployee(EmployeeSearchModel modelSearch)
        {
            var result = saleGroup.GetListEmployee(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Danh sách sản phẩm khác sản phẩm đã được chọn.
        /// </summary>
        /// <param name="searchModel">Danh sách id sản phẩm đã chọn từ trước.</param>
        /// <returns></returns>
        [Route("GetListProduct")]
        [HttpPost]
        public HttpResponseMessage GetListProduct(ProductSearchModel modelSearch)
        {
            var result = saleGroup.GetListProduct(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

    }
}
