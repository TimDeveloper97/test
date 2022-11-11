using NTS.Model.Expert;
using NTS.Model.PracticeExperts;
using QLTK.Api.Attributes;
using QLTK.Business.PracticeExperts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.PracticeExpert
{
    [RoutePrefix("api/PracticeExpert")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F040730")]
    public class PracticeExpertsController : ApiController
    {
        private readonly PracticeExpertBussiness _bussiness = new PracticeExpertBussiness();

        [Route("SearchPracticeExperts")]
        [HttpPost]
        public HttpResponseMessage SearchPracticeExperts(PracticeExpertsSearchModel modelSearch)
        {
            var result = _bussiness.SearchPracticeExpert(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchExpert")]
        [HttpPost]
        public HttpResponseMessage SearchExperts(ExpertSearchModel modelSearch)
        {
            var result = _bussiness.SearchExpert(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddPracticeExperts")]
        [HttpPost]
        public HttpResponseMessage AddPracticeExperts(PracticeExpertsModel model)
        {
            _bussiness.AddPracticeExperts(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
