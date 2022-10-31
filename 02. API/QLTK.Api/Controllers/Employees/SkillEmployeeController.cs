using NTS.Model.SkillEmployee;
using QLTK.Api.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.SkillEmployee
{
    [RoutePrefix("api/SkillEmployee")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F080020")]
    public class SkillEmployeeController : ApiController
    {
        private readonly Business.SkillEmployee.SkillEmployeeBussiness bussiness = new Business.SkillEmployee.SkillEmployeeBussiness();
        
        [Route("GetSkillEmployeeInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080012;F080019;F080002;F080004")]
        public HttpResponseMessage GetSkillGroupInfo(SkillEmployeeModel model)
        {
            var result = bussiness.SearchWorkSkill(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetSkillEmployeeInfos")]
        [HttpPost]
        public HttpResponseMessage GetSkillGroupInfos(SkillEmployeeModel model)
        {
            var result = bussiness.SearchWorkSkill(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddSkillEmployee")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080012")]
        public HttpResponseMessage AddSkillEmployee(SkillEmployeeModel model)
        {
            bussiness.AddSkillEmployee(model);
            return Request.CreateResponse(HttpStatusCode.OK, value: string.Empty);
        }
    }
}
