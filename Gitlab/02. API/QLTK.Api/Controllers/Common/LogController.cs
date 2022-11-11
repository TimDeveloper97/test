using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using NTS.Api;
using NTS.Api.Models;
using System.Web.Http;
using System.Net.Http;
using NTS.Business;
using NTS.Model.SearchCondition;
using System.Net;
using QLTK.Business;

namespace NTS.Api
{
    [RoutePrefix("api/UserEventLog")]
    public class LogController : ApiController
    {
        private readonly EventLogBusiness _Business = new EventLogBusiness();
        [Route("SearchUserEventLog")]
        [HttpPost]
        public HttpResponseMessage SearchUserEventLog(UserEventLogSearchCondition modelSearch)
        {
            try
            {
                var result = _Business.SearchUserEventLog(modelSearch);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}