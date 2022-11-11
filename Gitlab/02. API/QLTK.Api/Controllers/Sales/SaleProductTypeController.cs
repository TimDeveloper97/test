using NTS.Model.Sale.SaleProduct;
using NTS.Model.Unit;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.SaleProducts;
using QLTK.Business.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Sale
{
    [RoutePrefix("api/SaleProductType")]
    [ApiHandleExceptionSystemAttribute]
    [NTSIPAuthorize(AllowFeature = "F120705")]
    public class SaleProductTypeController : BaseController
    {
        private readonly SaleProductTypeBussiness _saleProductTypeBussiness = new SaleProductTypeBussiness();

        [Route("DeleteType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120703")]
        public HttpResponseMessage DeleteManufacture(SaleProductTypeModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _saleProductTypeBussiness.DeleteType(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("AddType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120701")]
        public HttpResponseMessage AddType(SaleProductTypeModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _saleProductTypeBussiness.AddType(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetTypeInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120701;F120702")]
        public HttpResponseMessage GetUnitInfo(SaleProductTypeModel model)
        {
            var result = _saleProductTypeBussiness.GetTypeInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120702")]
        public HttpResponseMessage UpdateType(SaleProductTypeModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _saleProductTypeBussiness.UpdateType(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetListType")]
        [HttpPost]
        public HttpResponseMessage GetListUnit()
        {
            var rs = _saleProductTypeBussiness.GetListType();
            return Request.CreateResponse(HttpStatusCode.OK, rs);
        }
    }
}
