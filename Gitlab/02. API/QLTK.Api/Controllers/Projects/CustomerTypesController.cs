using NTS.Model.CustomerType;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.CustomerTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.CustomerType
{
    [RoutePrefix("api/CustomerType")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F060105")]
    public class CustomerTypesController : BaseController
    {
        private readonly CustomerTypeBussiness _business = new CustomerTypeBussiness();

        [Route("SearchCustomerType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060100")]
        public HttpResponseMessage SearchCustomerType(CustomerTypeSearchModel modelSearch)
        {
            var result = _business.SearchCustomerType(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddCustomerType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060101")]
        public HttpResponseMessage AddCustomerType(CustomerTypeModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            var result = _business.AddCustomerType(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateCustomerType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060102")]
        public HttpResponseMessage UpdateCustomerType(CustomerTypeModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateCustomerType(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteCustomerType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060103")]
        public HttpResponseMessage DeleteCustomerType(CustomerTypeModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteCustomerType(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetCustomerTypeInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060104;F060102")]
        public HttpResponseMessage GetCustomerTypeInfo(CustomerTypeModel model)
        {
            var result = _business.GetCustomerTypeInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
