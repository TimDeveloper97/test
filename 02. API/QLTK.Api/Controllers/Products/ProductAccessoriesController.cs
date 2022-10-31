using NTS.Model.Materials;
using NTS.Model.ProductAccessories;
using QLTK.Api.Attributes;
using QLTK.Business.ProductAccessories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ProductAccessories
{
    [RoutePrefix("api/ProductAccessories")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F030418")]
    public class ProductAccessoriesController : ApiController
    {
        private readonly ProductAccessoriesBussiness _business = new ProductAccessoriesBussiness();

        [Route("SearchMaterial")]
        [HttpPost]
        public HttpResponseMessage SearchMaterial(MaterialSearchModel modelSearch)
        {
            var result = _business.SearchMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchProductAccessories")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030406;F030416")]
        public HttpResponseMessage SearchProductAccessories(ProductAccessoriesSearchModel modelSearch)
        {
            var result = _business.SearchProductAccessories(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddProductAccessories")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030406")]
        public HttpResponseMessage AddProductAccessories(MaterialModel model)
        {
            _business.AddProductAccessories(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateProductAccessories")]
        [HttpPost]
        public HttpResponseMessage UpdateProductAccessories(ProductAccessoriesModel model)
        {
            _business.UpdateProductAccessories(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteProductAccessories")]
        [HttpPost]
        public HttpResponseMessage DeleteDesigner(ProductAccessoriesModel model)
        {
            _business.DeleteProductAccessories(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030415")]
        public HttpResponseMessage ExportExcel(ProductAccessoriesExcelModel model)
        {
            try
            {
                string path = _business.ExportExcel(model);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
