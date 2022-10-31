using NTS.Model.ProductGroup;
using QLTK.Api.Attributes;
using QLTK.Business.ProductGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ProductGroup
{
    [RoutePrefix("api/ProductGroups")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F030304")]
    public class ProductGroupsController : ApiController
    {
        private readonly ProductGroupBussiness _business = new ProductGroupBussiness();

        [Route("SearchProductGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030400")]
        public HttpResponseMessage SearchProductGroup(ProductGroupSearchModel modelSearch)
        {

                var result = _business.GetListProductGroup(modelSearch);
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchProductGroupById")]
        [HttpPost]
        public HttpResponseMessage SearchProductGroupById(ProductGroupSearchModel modelSearch)
        {
                var result = _business.GetProductGroupById(modelSearch.Id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteProductGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030303")]
        public HttpResponseMessage DeleteProductGroup(ProductGroupModel model)
        { 
                _business.DeleteProductGroup(model);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("AddProductGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030301")]
        public HttpResponseMessage AddProductGroup(ProductGroupModel model)
        {

                _business.AddProductGroup(model);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        [Route("GetProductGroupInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030302")]
        public HttpResponseMessage GetProductGroupInfo(ProductGroupModel model)
        {
                var result = _business.GetProductGroupInfo(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateProductGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030302")]
        public HttpResponseMessage UpdateProductGroup(ProductGroupModel model)
        {
                _business.UpdateProductGroup(model);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        //[Route("SearchProductStandard")]
        //[HttpPost]
        //public HttpResponseMessage SearchProductStandard(ProductStandardsSearchModel modelSearch)
        //{
        //    try
        //    {
        //        var result = _business.ProductStandards(modelSearch);
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        [Route("SearchProductGroupExpect")]
        [HttpPost]
        public HttpResponseMessage SearchProductGroupExcepted(ProductGroupSearchModel modelSearch)
        {
                var result = _business.GetProductGroupExcepted(modelSearch);
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
