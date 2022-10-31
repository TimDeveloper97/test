using NTS.Model.Cost;
using NTS.Model.CostWarning;
using QLTK.Api.Attributes;
using QLTK.Business.CostWarning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.CostWarning
{
    [RoutePrefix("api/CostWarning")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F100013")]
    public class CostWarningsController : ApiController
    {
        private readonly CostWarningBussiness _business = new CostWarningBussiness();

        [Route("SearchCost")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100007")]
        public HttpResponseMessage SearchCost(CostModel modelSearch)
        {
            var result = _business.SearchCost(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddCost")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100007")]
        public HttpResponseMessage AddCost(CostWarningModel modelSearch)
        {
            _business.AddCost(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
