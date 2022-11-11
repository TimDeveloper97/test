
using NTS.Model.HistoryVersion;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.HistoryVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.HistoryVersion
{
    [RoutePrefix("api/HistoryVersion")]
    [ApiHandleExceptionSystemAttribute]
    public class HistoryVersionController : BaseController
    {
        HistoryVersionBussiness historyVersionBussiness = new HistoryVersionBussiness();

        [Route("GetDataHistoryVersion")]
        [HttpPost]
        public HttpResponseMessage GetDataHistoryVersion(HistoryVersionModel modelSearch)
        {
            var result = historyVersionBussiness.GetDataHistoryVersion(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateVersion")]
        [HttpPost]
        public HttpResponseMessage UpdateVersion(HistoryVersionModel modelSearch)
        {
            var userId = GetUserIdByRequest();
            historyVersionBussiness.UpdateVersion(modelSearch, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
