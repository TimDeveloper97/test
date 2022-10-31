using NTS.Common;
using NTS.Model.Bussiness.Application;
using NTS.Model.Job;
using NTS.Model.Sale.SaleProduct;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Sale.ProductForBusiness;
using QLTK.Business.SaleProducts;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.Sale
{
    [RoutePrefix("api/SaleProduct")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F120005")]
    public class SaleProductController : BaseController
    {
        private readonly ProductForBusinessService _business = new ProductForBusinessService();
        private readonly SaleProductBussiness saleProduct = new SaleProductBussiness();

        /// <summary>
        /// Tìm kiếm sản phẩm kinh doanh
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchSaleProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120000")]
        public HttpResponseMessage SearchSaleProduct(SaleProductSearchModel modelSearch)
        {
            var isPermission = false;
            if (this.CheckPermission(Constants.Permission_Code_F120000))
            {
                isPermission = true;
            }
            var isPermissionViewAll = false;
            if (this.CheckPermission(Constants.Permission_Code_View_ALl_F120007))
            {
                isPermissionViewAll = true;
            }
            List<string> listIdSaleGroup = GetSaleGroupByRequest();
            var result = _business.SearchSaleProduct(modelSearch, isPermission, isPermissionViewAll, listIdSaleGroup);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Tìm kiếm sản phẩm danh sách phụ kiện
        /// </summary>
        /// <param name="modelSearch"></param>
        ///  <param name="idSaleProduct"></param>
        /// <returns></returns>
        [Route("SearchSaleProduct/{idSaleProduct}")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120000")]
        public HttpResponseMessage GetListAccessoryProduct(string idSaleProduct, SaleProductSearchModel modelSearch)
        {
            var result = _business.GetListAccessory(idSaleProduct, modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Thêm sản phẩm
        /// </summary>
        /// <returns></returns>
        [Route("CreateSaleProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120001")]
        public HttpResponseMessage CreateSaleProduct(SaleProductCreateModel model)
        {
            _business.CreateSaleProduct(model, GetUserIdByRequest(), GetSbuIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        /// <summary>
        /// Danh sách ngamhf nghề
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchJob")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage SearchJOb(JobSearchModel modelSearch)
        {
            var result = _business.SearchJob(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        /// <summary>
        /// Danh sách ứng dụng
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchApp")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage SearchApp(ApplicationSearchModel modelSearch)
        {
            var result = _business.SearchApplication(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        /// <summary>
        /// Danh sách sản phẩm
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getAllSaleProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage getAllSaleProduct(SaleProductSearchModel modelSearch)
        {
            var isPermission = false;
            if (this.CheckPermission(Constants.Permission_Code_F120000))
            {
                isPermission = true;
            }
            var isPermissionViewAll = false;
            if (this.CheckPermission(Constants.Permission_Code_View_ALl_F120007))
            {
                isPermissionViewAll = true;
            }
            List<string> listIdSaleGroup = GetSaleGroupByRequest();
            var result = _business.GetAllSaleProduct(modelSearch, isPermission, isPermissionViewAll, listIdSaleGroup);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        /// <summary>
        /// Sửa sản phẩm
        /// </summary>
        /// <returns></returns>
        [Route("UpdateSaleProduct/{id}")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120002")]
        public HttpResponseMessage UpdateSaleProduct(string id, SaleProductCreateModel model)
        {
            _business.UpdateSaleProduct(id, model, GetUserIdByRequest(), GetSbuIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("getProductInfoByProductId/{id}")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage GetProductInfoByProductId(string id)
        {
            var result = _business.GetInfoById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("getAppByProductId/{id}")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage GetAppByProductId(string id)
        {
            var result = _business.GetAppById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("getJobByProductId/{id}")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage GetJobByProductId(string id)
        {
            var result = _business.GetCareeById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("getMediaByProductId/{id}")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage GetMediaByProductId(string id)
        {
            var result = _business.GetMediaById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("getDocumentByProductId/{id}")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage GetDocumentByProductId(string id)
        {
            var result = _business.GetDocumentById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("getAccessoryByProductId/{id}")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage GetAccessoryByProductId(string id)
        {
            var result = _business.GetAccessoryById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ImportFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120006")]
        public HttpResponseMessage ImportFile()
        {
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            var lastModified = HttpContext.Current.Request.Form["LastModified"];
            if (hfc.Count > 0)
            {
                saleProduct.ImportFile(hfc[0], lastModified);
            }
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("deleteSaleProduct/{id}")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120003")]
        public HttpResponseMessage DeleteSaleProduct(string id)
        {
            _business.DeleteSaleProduct(id, GetUserIdByRequest(), GetSbuIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("updateStatus/{id}")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120002")]
        public HttpResponseMessage UpdateStatus(string id)
        {
            _business.UpdateStatus(id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Danh sách nhóm sản phẩm kinh doanh
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getAllGroupSaleProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage getAllGroupSaleProduct(SearchSaleGroupModel modelSearch)
        {
            var result = _business.SearchGroupProduct(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("getGroupByProductId/{id}")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage GetGroupByProductId(string id)
        {
            var result = _business.GetGroupById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("getEmployeeByGroupId/{id}")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F120001;F120002")]
        public HttpResponseMessage GetEmployeeByGroupId(string id)
        {
            var result = _business.getEmployee(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcel")]
        [HttpPost]
        public HttpResponseMessage ExportExcel(SaleProductSearchModel model)
        {
            var isPermission = false;
            if (this.CheckPermission(Constants.Permission_Code_F120000))
            {
                isPermission = true;
            }
            var isPermissionViewAll = false;
            if (this.CheckPermission(Constants.Permission_Code_View_ALl_F120007))
            {
                isPermissionViewAll = true;
            }
            var listGroupId = GetSaleGroupByRequest();
            var result = _business.ExportExcel(model, isPermission, isPermissionViewAll, listGroupId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DefaultType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120008")]
        public HttpResponseMessage DefaultType(List<string> productIds)
        {
            _business.DefaultType(productIds, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
