using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Common;
using NTS.Model.CustomerContact;
using NTS.Model.Customers;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Customers;
using QLTK.Business.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.Projects
{
    [RoutePrefix("api/CustomerContact")]
    [ApiHandleExceptionSystem]
    public class CustomerContactController : BaseController
    {
        private readonly CustomerContactBussiness _business = new CustomerContactBussiness();

        [Route("SearchCustomerContact")]
        [HttpPost]
        public HttpResponseMessage SearchCustomerContact(CustomerContactSearchModel modelSearch)
        {
            var result = _business.SearchCustomerContact(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}