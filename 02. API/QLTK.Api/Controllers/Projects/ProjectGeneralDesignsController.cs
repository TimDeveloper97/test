using NTS.Model.DMVTImportSAP;
using NTS.Model.Materials;
using NTS.Model.ProjectGeneralDesign;
using NTS.Model.ProjectProducts;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ProjectGeneralDesigns;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ProjectGeneralDesign
{
    [RoutePrefix("api/ProjectGeneralDesign")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F060036")]
    public class ProjectGeneralDesignsController : BaseController
    {
        private readonly ProjectGeneralDesignBusiness _business = new ProjectGeneralDesignBusiness();

        [Route("SearchProjectProductExport")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060004")]
        public HttpResponseMessage SearchProjectProductExport(ProjectProductsSearchModel modelSearch)
        {
            var result = _business.SearchProjectProductExport(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchProjectGeneralDesign")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060022;F060023;F060024;F060025")]
        public HttpResponseMessage SearchProjectGeneralDesign(ProjectGeneralDesignSearchModel modelSearch)
        {
            var result = _business.SearchProjectGeneralDesign(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060022;F060023")]
        public HttpResponseMessage SearchMaterial(MaterialSearchModel modelSearch)
        {
            var result = _business.SearchMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddProjectGeneralDesign")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060022")]
        public HttpResponseMessage AddProjectGeneralDesign(ProjectGeneralDesignModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            return Request.CreateResponse(HttpStatusCode.OK, _business.AddProjectGeneralDesign(model));
        }

        [Route("UpdateProjectGeneralDesign")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060023")]
        public HttpResponseMessage UpdateProjectGeneralDesign(ProjectGeneralDesignModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateProjectGeneralDesign(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteProjectGeneralDesign")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060029")]
        public HttpResponseMessage DeleteProjectGeneralDesign(ProjectGeneralDesignModel model)
        {
            _business.DeleteProjectGeneralDesign(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetProjectGeneralDesignInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060023;F060024;F060025")]
        public HttpResponseMessage GetProjectGeneralDesignInfo(ProjectGeneralDesignModel model)
        {
            var result = _business.GetProjectGeneralDesignInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GeneralDesign")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060022;F060023;F060024;F060025")]
        public HttpResponseMessage GeneralDesign(ProjectGeneralDesignModel model)
        {
            model.DepartmentId = GetDepartmentIdByRequest();
            var result = _business.GeneralDesign(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListDepartment")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060022;F060023;F060024;F060025")]
        public HttpResponseMessage GetListDepartment(string sbuId)
        {
            var result = _business.GetListDepartment(sbuId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [HttpPost]
        [Route("GetData")]
        [NTSAuthorize(AllowFeature = "F060022;F060023;F060024;F060025")]
        public HttpResponseMessage GetData()
        {
            var employeeId = GetEmployeeIdByRequest();
            var data = _business.GetData(employeeId);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("ExpoetGeneralDesign")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060025")]
        public HttpResponseMessage ExpoetGeneralDesign(ProjectGeneralDesignModel model)
        {
            string path = _business.ExpoetGeneralDesign(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("ExportExcelManage")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060025")]
        public HttpResponseMessage ExportExcelManage(ProjectGeneralDesignModel model)
        {
            model.DepartmentId = GetDepartmentIdByRequest();
            var result = _business.ExportExcelManage(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportBOM")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060025")]
        public HttpResponseMessage ExportBOM(DesignModuleInfoModel model)
        {
            var result = _business.ExportBOM(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcelBOM")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060025")]
        public HttpResponseMessage ExportExcelBOM(ProjectGeneralDesignModel model)
        {
            var result = _business.ExportExcelBOM(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateApproveStatus")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060035")]
        public HttpResponseMessage UpdateApproveStatus(ProjectGaneralDesignApproveStatusModel model)
        {
            _business.UpdateApproveStatus(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetListPlanByProjectProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060035")]
        public HttpResponseMessage GetListPlanByProjectProduct(ProjectGeneralDesignModel model)
        {
            var result = _business.GetListPlanByProjectProduct(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CheckApproveStatus")]
        [HttpPost]
        public HttpResponseMessage CheckApproveStatus(string projectProductId)
        {
            var result = _business.CheckApproveStatus(projectProductId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListMaterialOfModule")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060023")]
        public HttpResponseMessage GetListMaterialOfModule(ProjectGeneralDesignModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            var result = _business.GetListMaterialOfModule(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateMaterialImportBOM")]
        [HttpPost]
        public HttpResponseMessage UpdateMaterialImportBOM(ProjectGeneralDesignModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateMaterialImportBOM(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
