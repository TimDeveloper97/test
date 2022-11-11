using NTS.Common;
using NTS.Model.DashboardEmployee;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.DashboardEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.DashboardEmployee
{
    [RoutePrefix("api/DashboardEmployee")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F061003")]
    public class DashboardEmployeesController : BaseController
    {
        private readonly DashboardEmployeeBussiness _business = new DashboardEmployeeBussiness();

        [Route("SearchListEmployee")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F061000")]
        public HttpResponseMessage SearchListEmployee(DashboardEmployeeSearchModel modelSearch)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F061002))
            {
                modelSearch.SBUId = this.GetSbuIdByRequest();
                modelSearch.DepartmentId = this.GetDepartmentIdByRequest();
            }
            var result = _business.SearchListEmployee(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetGeneralDashboardEmployee")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F061000")]
        public HttpResponseMessage GetGeneralDashboardEmployee(DashboardEmployeeSearchModel modelSearch)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F061002))
            {
                modelSearch.SBUId = this.GetSbuIdByRequest();
                modelSearch.DepartmentId = this.GetDepartmentIdByRequest();
            }
            var result = _business.GetGeneralDashboardEmployee(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetEmployee")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F061000")]
        public HttpResponseMessage GetEmployee(DashboardEmployeeSearchModel modelSearch)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F061002))
            {
                modelSearch.SBUId = this.GetSbuIdByRequest();
                modelSearch.DepartmentId = this.GetDepartmentIdByRequest();
            }
            var result = _business.GetEmployee(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
