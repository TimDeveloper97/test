using NTS.Model.CurrentCostWarning;
using QLTK.Api.Attributes;
using QLTK.Business.CurrentCostWarning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.CurrentCostWarning
{
    [RoutePrefix("api/CurrentCostWarning")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F100013")]
    public class CurrentCostWarningsController : ApiController
    {
        private readonly CurrentCostWarningBusiness _business = new CurrentCostWarningBusiness();

        [Route("getDataCurrentCostWarning")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100012")]
        public HttpResponseMessage getDataCurrentCostWarning(CurrentCostWarningSearchModel modelSearch)
        {
            var result = _business.getDataCurrentCostWarning(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExcelCurrentCostWarning")]
        [HttpPost]
        public HttpResponseMessage ExcelCurrentCostWarning(List<GroupAddModel> listData)
        {
            var result = _business.ExcelCurrentCostWarning(listData);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
