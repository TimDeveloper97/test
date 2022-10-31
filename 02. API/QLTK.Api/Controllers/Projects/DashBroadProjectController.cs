using NTS.Common;
using NTS.Model.DashBroadProject;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.DashBroadProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.DashBroadProject
{
    [RoutePrefix("api/DashboardProject")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F061103")]
    public class DashBroadProjectController : BaseController
    {
        private readonly DashBroadProjectBussiness _business = new DashBroadProjectBussiness();

        [Route("GetListProject")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F061100")]
        public HttpResponseMessage SearchListEmployee(DashBroadProjectSearchModel model)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F061102))
            {
                model.SBUId = this.GetSbuIdByRequest();
                model.DepartmentId = this.GetDepartmentIdByRequest();
            }
            var result = _business.GetDashBroadProject(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ViewDetailDesign")]
        [HttpPost]
        public HttpResponseMessage ViewDetailDesign(string projectId, int value)
        {
            var result = _business.ViewDetailDesign(projectId, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ViewDetailDocument")]
        [HttpPost]
        public HttpResponseMessage ViewDetailDocument(string projectId, int value)
        {
            var result = _business.ViewDetailDocument(projectId, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ViewDetailTransfer")]
        [HttpPost]
        public HttpResponseMessage ViewDetailTransfer(string projectId, int value)
        {
            var result = _business.ViewDetailTransfer(projectId, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
