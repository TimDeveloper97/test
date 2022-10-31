using NTS.Common;
using NTS.Model.ProductStandardTPAFile;
using NTS.Model.ProductStandardTPAs;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ProductStandardTPAs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.ProductStandardTPA
{
    [RoutePrefix("api/ProductStandardTPA")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F110812")]
    public class ProductStandardTPAController : BaseController
    {
        private readonly ProductStandardTPABussiness productStandardTPABussiness = new ProductStandardTPABussiness();

        [Route("SearchProductStandardTPA")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110800")]
        public HttpResponseMessage SearchProductStandardTPA(ProductStandardTPASearchModel modelSearch)
        {
            var result = productStandardTPABussiness.SearchProductStandardTPA(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CreateProductStandardTPA")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110801")]
        public HttpResponseMessage CreateProductStandardTPA(ProductStandardTPAModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            productStandardTPABussiness.CreateProductStandardTPA(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateProductStandardTPA")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110802;F110811")]
        public HttpResponseMessage UpdateProductStandardTPA(ProductStandardTPAModel model)
        {
            model.UpdateBy = GetUserIdByRequest();

            var isEditPriceTPA = CheckPermission(Constants.Permission_Code_F110811);
            productStandardTPABussiness.UpdateProductStandardTPA(model, isEditPriceTPA);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteProductStandardTPA")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110803")]
        public HttpResponseMessage DeleteProductStandardTPA(ProductStandardTPAModel modelSearch)
        {
            productStandardTPABussiness.DeleteProductStandardTPA(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetProductStandardTPAInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110802;F110804")]
        public HttpResponseMessage GetProductStandardTPAInfo(ProductStandardTPAModel modelSearch)
        {
            var result = productStandardTPABussiness.GetProductStandardTPAInfo(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("ImportProductStandardTPA")]
        [NTSAuthorize(AllowFeature = "F110801")]
        public HttpResponseMessage ImportProductStandardTPA()
        {
            var userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                productStandardTPABussiness.ImportProductStandardTPA(userId, hfc[0]);
            }

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UploadFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110801;F110802")]
        public HttpResponseMessage UploadFile(ProductStandardTPAFileCreateModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            productStandardTPABussiness.UploadFile(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetListProductStandardTPAFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110801;F110802;F110804")]
        public HttpResponseMessage GetListProductStandardTPAFile(ProductStandardTPAFileCreateModel model)
        {
            var result = productStandardTPABussiness.GetListProductStandardTPAFile(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110808")]
        public HttpResponseMessage ExportExcel(ProductStandardTPASearchModel model)
        {
            var result = productStandardTPABussiness.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcelBusiness")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110809")]
        public HttpResponseMessage ExportExcelBusiness(ProductStandardTPASearchModel model)
        {
            var result = productStandardTPABussiness.ExportExcelBusiness(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcelAccountant")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110810")]
        public HttpResponseMessage ExportExcelAccountant(ProductStandardTPASearchModel model)
        {
            var result = productStandardTPABussiness.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Danh sách nhà cung cấp
        /// </summary>
        /// <returns></returns>
        [Route("GetSuppliers")]
        [HttpPost]
        public HttpResponseMessage GetSuppliers()
        {
            var result = productStandardTPABussiness.GetSuppliers();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ImportFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110813")]
        public HttpResponseMessage ImportFile()
        {
            var data = HttpContext.Current.Request.Form["isConfirm"];
            var isConfirm = Convert.ToBoolean(data);
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                var result = productStandardTPABussiness.ImportFile(hfc[0], isConfirm, GetUserIdByRequest());
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("SyncSaleProductStandardTPA")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110813")]
        public HttpResponseMessage SyncSaleProduct(bool check, bool isConfirm, List<string> listProductId)
        {
            var result = productStandardTPABussiness.SyncSaleProductStandardTPA(check, isConfirm, listProductId, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
