using NTS.Common;
using NTS.Model.ExportAndKeep;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ExportAndKeep;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace QLTK.Api.Controllers.ExportAndKeep
{
    [RoutePrefix("api/ExportAndKeep")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F120105")]
    public class ExportAndKeepController : BaseController
    {
        private readonly ExportAndKeepBussiness service = new ExportAndKeepBussiness();

        /// <summary>
        /// Tìm kiếm xuất giữ
        /// </summary>
        /// <param name="modelSearch">Dữ liệu tìm kiếm</param>
        /// <returns>Danh sách trả về theo điều kiện</returns>
        [Route("Search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120100;F120120")]
        public HttpResponseMessage SearchExportAndKeep(ExportAndKeepSearchModel modelSearch)
        {
            if (!CheckPermission(Constants.Permission_Code_F120120))
            {
                modelSearch.EmployeeId = GetEmployeeIdByRequest();
            }

            var result = service.SearchExportAndKeep(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lịch sử xuất giữ
        /// </summary>
        /// <param name="modelSearch">Điều kiện tìm kiếm </param>
        /// <returns></returns>
        [Route("SearchExportAndKeepHistory")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120200")]
        public HttpResponseMessage SearchExportAndKeepHistory(ExportAndKeepSearchModel modelSearch)
        {
            if (!CheckPermission(Constants.Permission_Code_F120202))
            {
                modelSearch.EmployeeId = GetEmployeeIdByRequest();
            }

            var result = service.SearchExportAndKeepHistory(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Thêm mới xuất giữ
        /// </summary>
        /// <param name="model">Thông tin thêm mới</param>
        [Route("Create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120101")]
        public HttpResponseMessage CreateExportAndKeep(ExportAndKeepCreateModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            service.CreateExportAndKeep(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Chỉnh sửa xuất giữ
        /// </summary>
        /// <param name="model">Thông tin chỉnh sửa</param>
        [Route("UpdateExportAndKeep")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120102;F120104")]
        public HttpResponseMessage UpdateExportAndKeep(ExportAndKeepCreateModel model)
        {
            var isUpdateOther = CheckPermission(Constants.Permission_Code_F120106);
            service.UpdateExportAndKeep(model, GetUserIdByRequest(), isUpdateOther);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin xuất giữ theo id
        /// </summary>
        /// <param name="id">id xuất giữ</param>
        /// <returns></returns>
        [Route("GetInfoByIdExportAndKeep/{id}")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F120102;F120106")]
        public HttpResponseMessage GetInfoByIdExportAndKeep(string id)
        {
            var result = service.GetInfoByIdExportAndKeep(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy thông tin xuất giữ theo id
        /// </summary>
        /// <param name="id">id xuất giữ</param>
        /// <returns></returns>
        [Route("GetExportAndKeepViewById/{id}")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F120104;F120108")]
        public HttpResponseMessage GetExportAndKeepViewById(string id)
        {
            var result = service.GetExportAndKeepViewById(id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Xóa xuất giữ theo id
        /// </summary>
        /// <param name="id">Id xuất giữ</param>
        /// <returns></returns>
        [Route("DeleteUpdateExportAndKeep/{id}")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120103;F120107")]
        public HttpResponseMessage DeleteExportAndKeepById(string id)
        {
            var isUpdateOther = CheckPermission(Constants.Permission_Code_F120107);
            service.DeleteExportAndKeepById(id, GetUserIdByRequest(), isUpdateOther);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy danh sách sản phẩm kinh doanh
        /// </summary>
        /// <param name="model">dữ liệu tìm kiếm</param>
        /// <returns></returns>
        [Route("GetSaleProducts")]
        [HttpPost]
        public HttpResponseMessage GetSaleProducts(SaleProductSearchModel modelSearch)
        {
            var isPermissionViewAll = false;
            if (this.CheckPermission(Constants.Permission_Code_View_ALl_F120007))
            {
                isPermissionViewAll = true;
            }

            modelSearch.SaleGroupIdRequests = GetSaleGroupByRequest();
            var result = service.GetSaleProducts(modelSearch, isPermissionViewAll);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy danh sách sản phẩm kinh doanh
        /// </summary>
        /// <param name="model">dữ liệu tìm kiếm</param>
        /// <returns></returns>
        [Route("GetSaleProductById/{id}")]
        [HttpPost]
        public HttpResponseMessage GetSaleProductById(string id)
        {
            var result = service.GetSaleProductById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Tự động tạo mã xuất giữ
        /// </summary>
        /// <returns></returns>
        [Route("GenerateCode")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120101")]
        public HttpResponseMessage GenerateCode()
        {
            var result = service.GenerateCode();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy danh sách nhóm khách hàng có type == 1;
        /// </summary>
        /// <returns></returns>
        [Route("GetCustomerTypes")]
        [HttpPost]
        public HttpResponseMessage GetCustomerTypes()
        {

            var result = service.GetCustomerTypes();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns></returns>
        [Route("GetListCustomer")]
        [HttpPost]
        public HttpResponseMessage GetListCustomer()
        {

            var result = service.GetListCustomer();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Thêm mới thông tin khách hàng
        /// </summary>
        /// <param name="model">Thêm mới thông tin khách hàng</param>
        /// <returns></returns>
        [Route("CreateCustomer")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120101;F120102;F120106")]
        public HttpResponseMessage CreateCustomer(CustomerCreateModel model)
        {
            var result = service.CreateCustomer(model, GetUserIdByRequest(), GetSbuIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Tự động tạo mã khách hàng
        /// </summary>
        /// <returns></returns>
        [Route("GenerateCodeCustomer")]
        [HttpPost]
        public HttpResponseMessage GenerateCodeCustomer()
        {
            var result = service.GenerateCodeCustomer();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Giải phóng xuất giữ
        /// </summary>
        /// <param name="id">id xuất giữ</param>
        /// <returns></returns>
        [Route("ManumitExportAndKeep/{id}")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120102;F120106")]
        public HttpResponseMessage ManumitExportAndKeep(string id)
        {
            service.ManumitExportAndKeep(id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Đã bán xuất giữ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("SoldExportAndKeep/{id}")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120102;F120106")]
        public HttpResponseMessage SoldExportAndKeep(string id)
        {
            service.SoldExportAndKeep(id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetListExportDetailBySaleProductId/{id}")]
        [HttpGet]
        public HttpResponseMessage GetListExportDetailBySaleProductId(string id)
        {
            var data = service.GetListExportDetailBySaleProductId(id);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("PrintCustomer")]
        [HttpGet]
        public async Task<HttpResponseMessage> PrintCustomer(string id)
        {
            try
            {
                var path = service.PrintCustomer(id);

                if (!File.Exists(path))
                {
                    throw new Exception("File không tồn tại!");
                }

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }

                memory.Position = 0;

                HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
                httpResponseMessage.Content = new StreamContent(memory);
                httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                httpResponseMessage.Content.Headers.ContentDisposition.FileName = "Thông tin khách hàng";
                httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                return httpResponseMessage;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }
    }
}
