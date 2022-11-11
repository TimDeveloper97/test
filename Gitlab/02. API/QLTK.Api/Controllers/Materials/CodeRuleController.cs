using NTS.Model.CodeRule;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.CodeRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.CodeRule
{
    /// <summary>
    /// Định nghĩa mã vật tư PM tạo
    /// </summary>
    [RoutePrefix(prefix: "api/CodeRule")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F001104")]
    public class CodeRuleController : BaseController
    {
        private readonly CodeRuleBusiness business = new CodeRuleBusiness();

        [Route("GetCodeRules")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F001103")]
        public HttpResponseMessage GetCodeRules()
        {
            var result = business.GetCodeRules();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchCodeRule")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F001100")]
        public HttpResponseMessage SearchCodeRule(SearchCodeRuleModel model)
        {
            var result = business.SearchCodeRule(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SaveCodeRule")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F001101")]

        public HttpResponseMessage SaveCodeRule(CodeRuleData model)
        {
            string userId = GetUserIdByRequest();
            var result = business.SaveCodeRule(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
