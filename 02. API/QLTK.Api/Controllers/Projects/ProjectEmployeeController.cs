using QLTK.Business.Projects;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NTS.Model.Projects.ProjectEmloyee;
using NTS.Model.ExternalEmployee;

namespace QLTK.Api.Controllers.Projects
{
    [RoutePrefix("api/ProjectEmployee")]
    [ApiHandleExceptionSystem]
 //   [NTSIPAuthorize(AllowFeature = "F060036")]
    public class ProjectEmployeeController : BaseController
    {
        private readonly ProjectEmployeeBusiness _business = new ProjectEmployeeBusiness();

        [Route("GetProjectEmployeeByProjectId")]
        [HttpPost]
        public HttpResponseMessage GetProjectEmployeeByProjectId(string projectId)
        {
            var result = _business.GetProjectEmployeeByProjectId(projectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetProjectExternalEmployeeByProjectId")]
        [HttpPost]
        public HttpResponseMessage GetProjectExternalEmployeeByProjectId(string projectId)
        {
            var result = _business.GetProjectExternalEmployeeByProjectId(projectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetSubsidyHistory")]
        [HttpPost]
        public HttpResponseMessage GetSubsidyHistory(string projectEmployeeId)
        {
            var result = _business.GetSubsidyHistory(projectEmployeeId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetDescriptionRoleById")]
        [HttpPost]
        public HttpResponseMessage GetDescriptionRoleById(string RoleId)
        {
            var result = _business.GetDescriptionRoleById(RoleId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateSubsidyPE")]
        [HttpPost]
        public HttpResponseMessage UpdateSubsidyPE(ProjectProductToEmployeeModel model)
        {
            _business.UpdateSubsidyPE(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("AddSubsidyHistory")]
        [HttpPost]

        public HttpResponseMessage AddSubsidyHistory(SubsidyHistoryModel model)
        {
            _business.AddSubsidyHistory(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("SearchProjectByEmployeeId")]
        [HttpPost]
        public HttpResponseMessage SearchProjectByEmployeeId(string EmployeeId)
        {
            var result = _business.SearchProjectByEmployeeId(EmployeeId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetEmployeeName")]
        [HttpPost]
        public HttpResponseMessage GetEmployeeName(string EmployeeId)
        {
            var result = _business.GetEmployeeName(EmployeeId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetExternalEmployeeName")]
        [HttpPost]
        public HttpResponseMessage GetExternalEmployeeName(string EmployeeId)
        {
            var result = _business.GetExternalEmployeeName(EmployeeId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchProjectByExEmployeeId")]
        [HttpPost]
        public HttpResponseMessage SearchProjectByExEmployeeId(string EmployeeId)
        {
            var result = _business.SearchProjectByExEmployeeId(EmployeeId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetProjectEmployeeNotByProjectId")]
        [HttpPost]
        public HttpResponseMessage GetProjectEmployeeNotByProjectId(string projectId)
        {
            var result = _business.GetProjectEmployeeNotByProjectId(projectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("SearchProjectEmployeeGroup")]
        [HttpPost]
        public HttpResponseMessage SearchProjectEmployeeGroup(ProjectEmployeeSearchModel modelSearch)
        {
            var result = _business.SearchProjectEmployeeGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //
        /// <summary>
        /// Lấy ra danh sách nhân viên khác nhân viên được chọn.
        /// </summary>
        /// <param name="searchModel">Danh sách id nhân viên được chọn lúc đầu.</param>
        /// <returns></returns>
        [Route("GetListEmployee")]
        [HttpPost]
        public HttpResponseMessage GetListEmployee(EmployeeSearchModel modelSearch)
        {
            var result = _business.GetListEmployee(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListExEmployee")]
        [HttpPost]
        public HttpResponseMessage GetListExEmployee(EmployeeSearchModel modelSearch)
        {
            var result = _business.GetListExEmployee(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        
        [Route("GetRole")]
        [HttpPost]
        public HttpResponseMessage GetRole()
        {
            var result = _business.GetRole();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Thêm nhân viên cho dự án
        /// </summary>
        /// <returns></returns>
        [Route("AddMoreProjectEmployee")]
        [HttpPost]
        public HttpResponseMessage AddMoreProjectEmployee(List<ProjectEmployeeModel> model)
        {
            _business.AddMoreProjectEmployee(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("CreateProjectEmployeeAndExternalEmployee")]
        [HttpPost]
        public HttpResponseMessage CreateProjectEmployeeAndExternalEmployee(ProjectExternalEmployeeModel Model)
        {
            Model.CreateBy = GetUserIdByRequest();
            Model.UpdateBy = GetUserIdByRequest();
            _business.CreateProjectEmployeeAndExternalEmployee(Model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteEmployee")]
        [HttpPost]
        public HttpResponseMessage DeleteEmployee(ProjectEmployeeModel model)
        {
            _business.DeleteEmployee(model);
            return Request.CreateResponse(HttpStatusCode.OK, new { });
        }

        [Route("ExportExcel")]
        [HttpPost]
        public HttpResponseMessage ExportExcel(ProjectEmployeeSearchModel model)
        {
            try
            {
                string path = _business.ExportExcel(model);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("updateHasContractPlanPermit")]
        [HttpPost]
        public HttpResponseMessage updateHasContractPlanPermit(ProjectProductToEmployeeModel model)
        {
            _business.updateHasContractPlanPermit(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
