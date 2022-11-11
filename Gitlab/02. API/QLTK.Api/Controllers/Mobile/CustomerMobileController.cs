using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Solutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using NTS.Model.Meeting;
using System.Net;
using NTS.Model.SaleGroups;
using QLTK.Business.SaleGroups;
using QLTK.Business.Customers;
using NTS.Model.Customers;
using QLTK.Business.Projects;
using NTS.Model.CustomerContact;

namespace QLTK.Api.Controllers.Solutions
{
    [RoutePrefix("api/mobile/customer")]
    [ApiHandleExceptionSystem]
    public class CustomerMobileController : BaseController
    {
        private readonly CustomersBussiness _business = new CustomersBussiness();
        private readonly CustomerContactBussiness _customerContactBussiness = new CustomerContactBussiness();

        [Route("search")]
        [HttpPost]
        public HttpResponseMessage SearchCustomer(CustomersSearchModel modelSearch)
        {

            modelSearch.PageNumber = -1;
            modelSearch.OrderBy = "Code";
            modelSearch.OrderType = true;

            var result = _business.SearchCustomer(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("contact/create")]
        [HttpPost]
        public HttpResponseMessage CreateCustomerContact(CustomerContactModel model)
        {
            _customerContactBussiness.Create(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}