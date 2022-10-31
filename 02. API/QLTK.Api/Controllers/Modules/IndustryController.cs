using NTS.Model.Industry;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Industry
{
    [RoutePrefix("api/Industry")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F091205")]
    public class IndustryController : BaseController
    {
        private readonly QLTK.Business.Industrys.IndustryBusiness industryBusiness = new Business.Industrys.IndustryBusiness();

        [Route("SearchIndustry")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F091200")]
        public HttpResponseMessage SearchIndustry(IndustrySearchModel modelSearch)
        {
            var result = industryBusiness.SearchIndustry(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddIndustry")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F091201")]
        public HttpResponseMessage AddIndustry(IndustryModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            industryBusiness.AddIndustry(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetIndustry")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F091202;F091204")]
        public HttpResponseMessage GetIndustry(IndustryModel model)
        {
            var result = industryBusiness.GetIndustry(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateIndustry")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F091202")]
        public HttpResponseMessage UpdateIndustry(IndustryModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            industryBusiness.UpdateIndustry(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteIndustry")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F091203")]
        public HttpResponseMessage DeleteIndustry(IndustryModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            industryBusiness.DeleteIndustry(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
