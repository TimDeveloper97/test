using NTS.Model.CoefficientEmployee;
using NTS.Model.ReportEmployeesPresent;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ReportEmployeesPresent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ReportEmployeePresent
{
    [RoutePrefix("api/Employees")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F100013;F110603")]
    public class ReportEmployeePresentController : BaseController
    {
        private readonly ReportEmployeesPresentBussiness reportEmployees = new ReportEmployeesPresentBussiness();

        [Route("GetReportEmployees")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100003")]
        public HttpResponseMessage SearchReport(ReportEmployeesPresentSearchModel modelSearch)
        {
            var result = reportEmployees.searchEmployees(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbProjectBySBUId_DepartmentId")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100003")]
        public HttpResponseMessage GetCbbProjectBySBUId_DepartmentId(string SBUId, string DepartmentId)
        {
            var result = reportEmployees.GetCbbProjectBySBUId_DepartmentId(SBUId, DepartmentId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetGroupProducts")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100003")]
        public HttpResponseMessage GetGroupProducts(string ProjectId)
        {
            var result = reportEmployees.GetGroupProducts(ProjectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CoefficientEmployee")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110601")]
        public HttpResponseMessage CoefficientEmployee(ReportEmployeesPresentSearchModel model)
        {
            var result = reportEmployees.CoefficientEmployees(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateCoefficientEmployee")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110603")]
        public HttpResponseMessage UpdateCoefficientEmployee(CoefficientEmployeeCreateModel model)
        {
            reportEmployees.UpdateCoefficientEmployee(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
