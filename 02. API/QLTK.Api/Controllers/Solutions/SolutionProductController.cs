using NTS.Model.SolutionProducts;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.SolutionProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.SolutionProduct
{
    [RoutePrefix("api/SolutionProduct")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F070113")]
    public class SolutionProductController : BaseController
    {
        private readonly SolutionProductBussiness _business = new SolutionProductBussiness();

        [Route("SearchSolutionProducts")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F070000")]
        public HttpResponseMessage SearchSolutionProduct(SolutionProductSearchModel modelSearch)
        {
            var result = _business.SearchSolutionProduct(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CreateSolutionProduct")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F070101")]
        public HttpResponseMessage CreateSolutionProduct(SolutionProductModel model)
        {
            string userId = GetUserIdByRequest();
            _business.CreateSolutionProduct(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateSolutionProduct")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F070102")]
        public HttpResponseMessage UpdateSolutionProduct(SolutionProductModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateSolutionProduct(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteSolutionProduct")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F070103")]
        public HttpResponseMessage DeleteSolutionProduct(string solutionProductId)
        {
            _business.DeleteSolutionProduct(solutionProductId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetByIdSolutionProduct")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F070104;F070102;F070105")]
        public HttpResponseMessage GetByIdSolutionProduct(string solutionProductId)
        {
            var result = _business.GetByIdSolutionProduct(solutionProductId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetObjectInfo")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F070104;F070102;F070105")]
        public HttpResponseMessage GetObjectInfo(SolutionProductModel objectInfo)
        {
            var result = _business.GetObjectInfo(objectInfo);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}