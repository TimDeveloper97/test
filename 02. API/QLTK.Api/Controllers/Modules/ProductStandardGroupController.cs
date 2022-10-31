using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NTS.Model.ProductStandardGroup;
using QLTK.Api.Attributes;
using QLTK.Business.ProductStandardGroups;

namespace QLTK.Api.Controllers.ProductStandardGroup
{
    [RoutePrefix("api/ProductStandardGroup")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F020404")]
    public class ProductStandardGroupController : ApiController
    {
        private readonly ProductStandardGroupBussiness _productStandardGroupBusiness = new ProductStandardGroupBussiness();

        [Route("SearchProductStandardGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020500")]
        public HttpResponseMessage SearchProductStandardGroup(ProductStandardGroupSearchModel modelSearch)
        {
            var result = _productStandardGroupBusiness.SearchProductStandardGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddProductStandardGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020401")]
        public HttpResponseMessage AddProductStandardGroup(ProductStandardGroupModel model)
        {

            _productStandardGroupBusiness.AddProductStandardGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateProductStandardGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020402")]
        public HttpResponseMessage UpdateProductStandardGroup(ProductStandardGroupModel model)
        {
            _productStandardGroupBusiness.UpdateProductStandardGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteProductStandardGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020403")]
        public HttpResponseMessage DeleteProductStandardGroup(ProductStandardGroupModel model)
        {
            _productStandardGroupBusiness.DeleteProductStandardGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetProductStandardGroupInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020402")]
        public HttpResponseMessage GetProductStandardGroupInfo(ProductStandardGroupModel model)
        {
            var result = _productStandardGroupBusiness.GetProductStandardGroupInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
