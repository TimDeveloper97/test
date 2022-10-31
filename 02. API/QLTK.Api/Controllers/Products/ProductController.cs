using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Practice;
using NTS.Model.Product;
using NTS.Model.ProductDesignDocuments;
using NTS.Model.ProductDocumentAttach;
using NTS.Model.QLTKMODULE;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.Product
{

    [RoutePrefix("api/Product")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F030417")]
    public class ProductController : BaseController
    {
        private readonly ProductBusiness _business = new ProductBusiness();

        /// <summary>
        /// Tìm kiếm 
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030400")]
        public HttpResponseMessage SearchProduct(ProductSearchModel modelSearch)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F030405))
            {
                modelSearch.DepartmentId = this.GetDepartmentIdByRequest();
                modelSearch.SBUId = this.GetSbuIdByRequest();
            }
            var result = _business.SearchProduct(modelSearch, false);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Thêm thiết bị
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [Route("AddProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030401")]
        public HttpResponseMessage AddProduct(ProductModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.DepartmentId = GetDepartmentIdByRequest();
            _business.AddProduct(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin thiết bị cập nhật
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [Route("GetProductInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030402;F030404")]
        public HttpResponseMessage GetProductInfo(ProductModel model)
        {
            var result = _business.GetProductInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy danh sách file tài liệu
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [Route("GetProductDocumentAttachs")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030402;F030404")]
        public HttpResponseMessage GetProductDocumentAttachs(ProductModel model)
        {
            var result = _business.GetProductDocumentAttachs(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật thiết bị
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [Route("UpdateProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030402")]
        public HttpResponseMessage UpdateProduct(ProductModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            var departmentId = GetDepartmentIdByRequest();
            if (!string.IsNullOrEmpty(model.DepartmentId) && !model.DepartmentId.Equals(departmentId))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0034, TextResourceKey.Product);
            }
            _business.UpdateProduct(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateProductDocument")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030402")]
        public HttpResponseMessage UpdateProductDocument(UpdateProductDocumentModel model)
        {
            model.UserId = GetUserIdByRequest();
            var rs = _business.UpdateProductDocument(model);
            return Request.CreateResponse(HttpStatusCode.OK, rs);
        }

        [Route("GetModule")]
        [HttpPost]
        public HttpResponseMessage GetFunction(ModuleSearchModel modelSearch)
        {

            var result = _business.GetModule(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        /// <summary>
        /// Thêm phác thảo thiết kế Thiết bị
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [Route("AddProductSketches")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030406")]
        public HttpResponseMessage AddProductSketches(ProductModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddProductSkeches(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetProductSketchesInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030416;F030406")]
        public HttpResponseMessage GetProductSketchesInfo(ProductModel model)
        {
            var result = _business.GetProductSketchesInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListMatarial")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F030416;F030406")]
        public HttpResponseMessage GetListMatarial(List<PracticeModel> practiceModels)
        {
            var result = _business.GetListMatarial(practiceModels);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030403")]
        public HttpResponseMessage DeleteMaterialGroupTPA(ProductModel model)
        {
            var departmentId = GetDepartmentIdByRequest();
            _business.DeleteProduct(model, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteProductModuleUpdate")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030403")]
        public HttpResponseMessage DeleteProductModuleUpdate(string id)
        {
            _business.DeleteProductModuleUpdate(id);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetListFolderProduct")]
        [HttpGet]
        public HttpResponseMessage GetListFolderModule(string productId)
        {
            var result = _business.GetListFolderProduct(productId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListFileProduct")]
        [HttpGet]
        public HttpResponseMessage GetListFileProduct(string folderId)
        {
            var result = _business.GetListFileProduct(folderId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Import file DMVT
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UploadDesignDocument")]
        [HttpPost]
        public HttpResponseMessage UploadDesignDocument(UploadFolderProductDesignDocumentModel model)
        {
            var userId = GetUserIdByRequest();

            _business.UploadDesignDocument(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("GetModulePrice")]
        [HttpPost]
        public HttpResponseMessage GetModulePrice(List<string> listModuleId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _business.GetModulePrice(listModuleId));
        }

        [Route("SynchronizedPractice")]
        [HttpPost]
        public HttpResponseMessage SynchronizedPractice(ProductModel model)
        {
            var result = _business.SynchronizedPractice(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("ImportProductModule")]
        public HttpResponseMessage ImportProductModule()
        {
            var productId = HttpContext.Current.Request.Form["ProductId"];
            //var projectId = JsonConvert.DeserializeObject<string>(modelJson);
            var userId = GetUserIdByRequest();
            List<ModuleModel> result = new List<ModuleModel>();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                result = _business.ImportModuleProductSketches(userId, hfc[0], productId);
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        /// <summary>
        /// Lấy danh sách thiết bị lỗi
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchProductErrors")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030402")]
        public HttpResponseMessage SearchProductErrors(SearchErrorProductModel modelSearch)
        {
            var result = _business.SearchProductErrors(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListFileTestAttachByProductId")]
        [HttpPost]
        public HttpResponseMessage GetListFileTestAttachByProductId(SearchFileTestAttachModel model)
        {
            var result = _business.GetListFileTestAttachByProductId(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CreateFileTestAttach")]
        [HttpPost]
        public HttpResponseMessage CreateFileTestAttach(SearchFileTestAttachModel model)
        {
            var userId = GetUserIdByRequest();

            _business.CreateFileTestAttach(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("CheckStatusProduct")]
        [HttpPost]
        public HttpResponseMessage CheckStatusProduct(CheckStatusModel model)
        {
            _business.CheckStatusProduct(model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Xuất file excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020106")]
        public HttpResponseMessage ExportExcel(ProductSearchModel model)
        {
            var path = _business.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GetContentProduct")]
        [HttpPost]
        public HttpResponseMessage GetContentProduct(string productId)
        {
            var data = _business.GetContentProduct(productId);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("UpdateContent")]
        [HttpPost]
        public HttpResponseMessage UpdateContent(ProductContentModel model)
        {
            _business.UpdateContent(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ImportFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030419")]
        public HttpResponseMessage ImportFile()
        {
            var data = HttpContext.Current.Request.Form["isConfirm"];
            var isConfirm = Convert.ToBoolean(data);
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                var result = _business.ImportFile(hfc[0], isConfirm, GetUserIdByRequest());
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("SyncSaleProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030419")]
        public HttpResponseMessage SyncSaleProduct(bool check, bool isConfirm, List<string> listProductId)
        {
            var result = _business.SyncSaleProduct(check, isConfirm, listProductId, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetModuleByProduct")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F030416;F030406")]
        public HttpResponseMessage GetModuleByProduct(ProductModel model)
        {
            var result = _business.GetModuleByProduct(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateNewPrice")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F030416;F030406")]
        public HttpResponseMessage GetModuleByProduct()
        {
            _business.UpdateNewPrice();
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("getProductNeedPublications")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F030416;F030406")]
        public HttpResponseMessage getProductNeedPublications(ProductSearchModel model)
        {
            var result = _business.GetProductNeedPublications(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
