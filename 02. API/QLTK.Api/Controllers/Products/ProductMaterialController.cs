using NTS.Model.Materials;
using NTS.Model.ModuleMaterials;
using NTS.Model.ProductMaterials;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ProductMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ProductMaterials
{
    [RoutePrefix("api/ProductMaterials")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F030418")]
    public class ProductMaterialController : BaseController
    {
        private readonly ProductMaterialsBussiness _business = new ProductMaterialsBussiness();

        [Route("SearchProductMaterials")]
        [HttpPost]
        public HttpResponseMessage SearchProductMaterials(ProductMaterialSearchModel modelSearch)
        {
            var result = _business.SearchProductMaterials(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchProductMaterialsSetup")]
        [HttpPost]
        public HttpResponseMessage SearchProductMaterialsSetup(ProductMaterialSearchModel modelSearch)
        {
            var result = _business.SearchProductMaterialsSetup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchMaterials")]
        [HttpPost]
        public HttpResponseMessage SearchMaterials(MaterialSearchModel modelSearch)
        {
            var result = _business.SearchMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddProductMaterials")]
        [HttpPost]
        public HttpResponseMessage AddProductMaterials(MaterialModel model)
        {
            _business.AddProductMaterials(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteProductMaterials")]
        [HttpPost]
        public HttpResponseMessage DeleteProductMaterials(ProductMaterialsModel model)
        {
            _business.DeleteProductMaterials(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateProductMaterials")]
        [HttpPost]
        public HttpResponseMessage UpdateProductMaterials(ProductMaterialsModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.UpdateProductMaterials(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetProductInfo")]
        [HttpPost]
        public HttpResponseMessage GetProductInfo(ProductMaterialsModel model)
        {
            var result = _business.GetProductInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcel")]
        public HttpResponseMessage ExportExcel(ProductMaterialsModel model)
        {
            string path = _business.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }
    }
}
