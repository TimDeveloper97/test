using NTS.Model.EmployeeSkillDetails;
using QLTK.Api.Attributes;
using QLTK.Business.EmployeeSkillDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.EmployeeSkillDetails
{
    [RoutePrefix("api/EmployeeSkillDetails")]
    [ApiHandleExceptionSystemAttribute]

    public class EmployeeSkillDetailController : ApiController
    {
        private readonly EmployeeSkillDetailsBusiness _business = new EmployeeSkillDetailsBusiness();

        [Route("SearchEmployeeSkillDetails")]
        [HttpPost]
        public HttpResponseMessage SearchEmployeeSkillDetails(EmployeeSkillDetailsSeaechModel modelSearch)
        {
            var result = _business.SearchEmployeeSkillDetails(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateEmployeeSkillDetails")]
        [HttpPost]
        public HttpResponseMessage UpdateEmployeeSkillDetails(EmployeeSkillDetailsModel model)
        {
            _business.UpdateEmployeeSkillDetails(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
