using NTS.Model.Bank;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Bank;
using QLTK.Business.SendEmailOutlook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers
{
    [RoutePrefix("api/Bank")]
    [ApiHandleExceptionSystem]

    public class BankController : BaseController
    {
        private readonly BankBussiness _bank = new BankBussiness();
        private readonly SendEmailOutlookBussiness _test = new SendEmailOutlookBussiness();

        [Route("SearchBank")]
        [HttpPost]
        public HttpResponseMessage SearchBank(BankSearchModel modelSearch)
        {
            var result = _bank.SearchBank(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetBank")]
        [HttpPost]
        public HttpResponseMessage GetBank(BankModel model)
        {
            var result = _bank.GetBankInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteBank")]
        [HttpPost]
        public HttpResponseMessage DeleteBank(BankModel model)
        {
            _bank.DeleteBank(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("Test")]
        [HttpPost]
        public HttpResponseMessage Test()
        {
            _test.PushQueue();
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
