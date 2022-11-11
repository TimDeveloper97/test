using NTS.Model.Expert;
using NTS.Model.PracticeExperts;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.PracticeExperts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.PracticeExpertChoose
{
    [RoutePrefix("api/PracticeExpertChoose")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F040730")]
    public class PracticeExpertChooseController : BaseController
    {
        private readonly PracticeExpertBussiness _bussiness = new PracticeExpertBussiness();


        [Route("LoadPracticeExperts")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040719")]

        public HttpResponseMessage LoadPracticeExperts(PracticeExpertsSearchModel modelSearch)
        {
            var result = _bussiness.LoadPracticeExpert(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchPracticeExperts")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040719")]
        public HttpResponseMessage SearchPracticeExperts(PracticeExpertsSearchModel modelSearch)
        {
            var result = _bussiness.SearchPracticeExpert(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchExpert")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040717")]
        public HttpResponseMessage SearchExperts(ExpertSearchModel modelSearch)
        {
            var result = _bussiness.SearchExpert(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddPracticeExperts")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040717")]
        public HttpResponseMessage AddPracticeExperts(PracticeExpertsModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _bussiness.AddPracticeExperts(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
