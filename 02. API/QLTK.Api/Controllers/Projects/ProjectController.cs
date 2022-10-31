using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Customers;
using NTS.Model.Project;
using NTS.Model.ProjectProducts;
using NTS.Model.Projects.Project;
using NTS.Model.TestCriteria;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Projects;

namespace QLTK.Api.Controllers.Project
{
    [RoutePrefix("api/Project")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F060036")]
    public class ProjectController : BaseController
    {
        private readonly ProjectBusiness project = new ProjectBusiness();

        /// <summary>
        /// Tìm kiếm
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("SearchProjectModel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020125")]
        public HttpResponseMessage SearchProjectModel(ProjectSearchModel model)
        {
            //model.DepartermentUserId = GetDepartmentIdByRequest();
            var result = project.SearchProjectModel(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Tìm kiếm
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("SearchTestCriteria")]
        [HttpPost]
        public HttpResponseMessage SearchTestCriteria(TestCriterSearchModel searchModelTest)
        {

            var result = project.SearchTestCriteria(searchModelTest);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Xuất Excel
        /// </summary>
        /// <param name="ExportExcel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportExcel")]
        public HttpResponseMessage ExportExcel(TestCriterSearchModel model)
        {
            try
            {
                string path = project.ExportExcel(model);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("SearchProject")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060000")]
        public HttpResponseMessage SearchProject(ProjectSearchModel searchModel)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F060005))
            {
                searchModel.SBUId = this.GetSbuIdByRequest();
                searchModel.DepartmentId = this.GetDepartmentIdByRequest();
            }
            var result = project.SearchProject(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xuất Excel
        /// </summary>
        /// <param name="ExportExcel"></param>
        /// <returns></returns>
        [Route("Excel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060006")]
        public HttpResponseMessage Excel(ProjectSearchModel model)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F060005))
            {
                model.SBUId = this.GetSbuIdByRequest();
                model.DepartmentId = this.GetDepartmentIdByRequest();
            }
            string path = project.Excel(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        /// <summary>
        /// Delete 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060003")]
        public HttpResponseMessage Delete(ProjectResultModel model)
        {
            var departmentId = GetDepartmentIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            project.Delete(model, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm dự án
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [Route("AddProject")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060001")]
        public HttpResponseMessage AddProject(ProjectResultModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            project.AddProject(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin dự án
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [Route("GetProjectInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060002;F060004;F060005")]
        public HttpResponseMessage GetProjectInfo(ProjectResultModel model)
        {
            var result = project.GetProjectInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCustomerTypeInfo")]
        [HttpPost]
        public HttpResponseMessage GetCustomerTypeInfo(CustomersModel model)
        {
            var result = project.GetCustomerTypeInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật dự án
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [Route("UpdateProject")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060002")]
        public HttpResponseMessage UpdateProject(ProjectResultModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            var departmentId = GetDepartmentIdByRequest();

            project.UpdateProject(model, departmentId);

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("Report")]
        [HttpPost]
        public HttpResponseMessage Report (MoneyCollectionProjectReportSearchModel searchModel)
        {
            var result = project.Report(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListReportProject")]
        [HttpPost]
        public HttpResponseMessage GetListReportProject(ReportProjectSearchModel searchModel)
        {
            var result = project.GetListReportProject(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateBedDebt")]
        [HttpPost]
        public HttpResponseMessage UpdateBedDebt(ProjectResultModel model)
        {
            model.UpdateBy = GetUserIdByRequest();

            project.UpdateBedDebt(model);

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateBadDebtDate")]
        [HttpPost]
        public HttpResponseMessage UpdateBadDebtDate(ProjectResultModel model)
        {
            project.UpdateBadDebtDate(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetMinYear")]
        [HttpPost]
        public HttpResponseMessage GetMinYear()
        {
            var result = project.GetMinYear();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcelReport")]
        [HttpPost]
        public HttpResponseMessage ExportExcelReport(MoneyCollectionProjectReportSearchModel model)
        {
            try
            {
                string path = project.ExportExcelReport(model);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetTotalBadDebt")]
        [HttpPost]
        public HttpResponseMessage GetTotalBadDebt(int year)
        {
            var result = project.GetTotalBadDebt(year);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
