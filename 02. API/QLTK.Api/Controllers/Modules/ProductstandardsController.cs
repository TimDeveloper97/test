using Newtonsoft.Json;
using NTS.Model.ProductStandards;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Productstandards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;


namespace QLTK.Api.Controllers.Productstandards
{
    [RoutePrefix("api/ProductStandard")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F020506")]
    public class ProductstandardsController : BaseController
    {
        private readonly ProductStandardsBusiness _productStandardBusiness = new ProductStandardsBusiness();

        [Route("SearchProductStandard")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020500")]
        public HttpResponseMessage SearchProductStandard(ProductStandardsSearchModel modelSearch)
        {
            var result = _productStandardBusiness.SearchModel(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddProductStandard")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020501")]
        public HttpResponseMessage AddProductStandard(ProductStandardsModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            var result = _productStandardBusiness.AddProductStandards(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateProductStandard")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020502")]
        public HttpResponseMessage UpdateProductStandard(ProductStandardsModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            var result = _productStandardBusiness.UpdateProductStandard(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteProductStandard")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020503")]
        public HttpResponseMessage DeleteProductStandard(ProductStandardsModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _productStandardBusiness.DeleteProductStandard(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetSBUIdandDeepartmentId")]
        [HttpPost]
        public HttpResponseMessage GetSBUIdandDeepartmentId()
        {
            string userId = GetUserIdByRequest();
            var result = _productStandardBusiness.GetSBUIdandDeepartmentId(userId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetProductStandardInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020502;F020504")]
        public HttpResponseMessage GetProductStandardGroupInfo(ProductStandardsModel model)
        {
            var result = _productStandardBusiness.GetProductStandardInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020505")]
        public HttpResponseMessage ExportExcel(ProductStandardsSearchModel model)
        {
            string path = _productStandardBusiness.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GetShowQCProductStandardInfo")]
        [HttpPost]
        public HttpResponseMessage GetShowQCProductStandardInfo(ProductStandardsModel model)
        {
            var result = _productStandardBusiness.GetShowQCProductStandardInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
