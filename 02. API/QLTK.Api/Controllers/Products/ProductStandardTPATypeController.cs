using NTS.Model.ProductStandardTPAs;
using NTS.Model.Unit;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ProductStandardTPAs;
using QLTK.Business.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ProductStandardTPA
{
    [RoutePrefix("api/ProductStandardTPAType")]
    [ApiHandleExceptionSystemAttribute]
    [NTSIPAuthorize(AllowFeature = "F110855")]
    public class ProductStandardTPATypeController : BaseController
    {
        private readonly ProductStandardTPATypeBussiness _productStandardTPATypeBussiness = new ProductStandardTPATypeBussiness();

        [Route("SearchType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110850")] 

        public HttpResponseMessage SearchMaterial(ProductStandardTPATypeSearchModel modelSearch)
        {
            var result = _productStandardTPATypeBussiness.SearchType(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110853")]
        public HttpResponseMessage DeleteManufacture(ProductStandardTPATypeModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _productStandardTPATypeBussiness.DeleteType(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("AddType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110851")]
        public HttpResponseMessage AddManufacture(ProductStandardTPATypeModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _productStandardTPATypeBussiness.AddType(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetTypeInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110851;F110852")]
        public HttpResponseMessage GetUnitInfo(ProductStandardTPATypeModel model)
        {
            var result = _productStandardTPATypeBussiness.GetTypeInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110852")]
        public HttpResponseMessage UpdateManufacture(ProductStandardTPATypeModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _productStandardTPATypeBussiness.UpdateType(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetListType")]
        [HttpPost]
        public HttpResponseMessage GetListUnit()
        {
            var rs = _productStandardTPATypeBussiness.GetListType();
            return Request.CreateResponse(HttpStatusCode.OK, rs);
        }
    }
}
