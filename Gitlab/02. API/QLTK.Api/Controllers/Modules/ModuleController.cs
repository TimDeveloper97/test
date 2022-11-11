
using Newtonsoft.Json;
using NTS.Common;
using NTS.Model.DataCheckModuleUpload;
using NTS.Model.GroupModule;
using NTS.Model.Module;
using NTS.Model.ModuleDesignDocument;
using NTS.Model.ModuleGroupProductStandard;
using NTS.Model.ModuleProductStandard;
using NTS.Model.ProductStandards;
using NTS.Model.QLTKMODULE;
using NTS.Model.Stage;
using NTS.Model.TestCriteria;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business;
using QLTK.Business.CodeRules;
using QLTK.Business.FileDefinitions;
using QLTK.Business.FolderDefinitions;
using QLTK.Business.Manufacturer;
using QLTK.Business.MaterialGroups;
using QLTK.Business.Materials;
using QLTK.Business.QLTKMODULE;
using QLTK.Business.RawMaterials;
using QLTK.Business.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.Module
{
    [RoutePrefix("api/Module")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F020129")]
    public class ModuleController : BaseController
    {
        private readonly ModuleBussiness _business = new ModuleBussiness();

        /// <summary>
        /// Tìm kiếm moduls
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchModuls")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020100")]
        public HttpResponseMessage SearchModule(ModuleSearchModel modelSearch)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F020105))
            {
                modelSearch.DepartmentId = this.GetDepartmentIdByRequest();
                modelSearch.SBUId = this.GetSbuIdByRequest();
            }
            var result = _business.SearchModule(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListTestCriteria")]
        [HttpPost]
        public HttpResponseMessage GetListTestCriteria(TestCriterSearchModel modelSearch)
        {
            var result = _business.GetListTestCriteria(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("Productstandard")]
        [HttpPost]
        public HttpResponseMessage GetListProductstandard(ProductStandardsSearchModel modelSearch)
        {
            var result = _business.SearchProductStandard(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Xuất file excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020106")]
        public HttpResponseMessage ExportExcel(ModuleSearchModel model)
        {
            var path = _business.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        /// <summary>
        /// Xóa module
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteModul")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020103")]
        public HttpResponseMessage DeleteGroupModul(ModuleModel model)
        {
            var departmentId = GetDepartmentIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteModul(model, departmentId);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm module
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddModule")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020101")]
        public HttpResponseMessage AddModule(ModuleModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.DepartmentCreated = GetDepartmentIdByRequest();
            var moduleId = _business.AddModule(model);
            return Request.CreateResponse(HttpStatusCode.OK, moduleId);
        }

        /// <summary>
        /// Thêm tiêu chuẩn
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddModuleGroupProductStandard")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020123")]
        public HttpResponseMessage AddModuleGroupProductStandard(ModuleProductStandardModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddModuleProductStandard(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetModuleProductStandardInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020124")]
        public HttpResponseMessage GetModuleProductStandardInfo(ModuleProductStandardModel model)
        {
            var result = _business.GetModuleProductStandardInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetModuleTestCriteriInfo")]
        [HttpPost]
        public HttpResponseMessage GetModuleTestCriteriInfo(TestCriteriModel model)
        {
            var result = _business.GetModuleTestCriteriInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetTestCriteria")]
        [HttpPost]
        public HttpResponseMessage GetTestCriteria(TestCriterSearchModel modelSearch)
        {

            var result = _business.GetTestCriteria(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchTestCriteria")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020122")]
        public HttpResponseMessage SearchTestCriteria(ModuleModel modelSearch)
        {

            var result = _business.SearchTestCriteria(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetTestCriteriaInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020122")]
        public HttpResponseMessage GetTestCriteriaInfo(ModuleModel model)
        {
            var result = _business.GetTestCriteriaInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddTestCriteria")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020119")]
        public HttpResponseMessage AddTestCriteria(ModuleModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddTestCriteria(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy tiêu chí còn lại
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("GetModuleGroupExcepted")]
        [HttpPost]
        public HttpResponseMessage GetModuleGroupExcepted(TestCriterSearchModel model)
        {
            var result = _business.GetModuleGroupExcepted(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Tìm kiếm theo id
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("GetModuleId")]
        [HttpPost]
        public HttpResponseMessage GetModuleId(TestCriteriaModel modelSearch)
        {
            var result = _business.GetModuleId(modelSearch.Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Xuất excel danh sách tiêu chí
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ExportExcelCriteria")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020121")]
        public HttpResponseMessage ExportExcelCriteria(TestCriterSearchModel model)
        {
            var result = _business.ExportExcelCriteria(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật module
        /// </summary>
        /// <returns></returns>
        [Route("UpdateModule")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020102")]
        public HttpResponseMessage UpdateModule(ModuleModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            var departmentId = GetDepartmentIdByRequest();
            _business.UpdateModule(model, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateModuleIsDMTV")]
        [HttpPost]
        public HttpResponseMessage UpdateModuleIsDMTV(ModuleModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateModuleIsDMTV(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetModuleInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020102;F020104;F020126")]
        public HttpResponseMessage GetModuleInfo(ModuleModel model)
        {
            model.DepartermentIdByRequest = GetDepartmentIdByRequest();
            var result = _business.GetModuleInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListFolderModule")]
        [HttpPost]
        public HttpResponseMessage GetListFolderModule(ModuleDesignDocumentModel model)
        {
            var result = _business.GetListFolderModule(model.Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListFileModule")]
        [HttpPost]
        public HttpResponseMessage GetListFileModule(ModuleDesignDocumentModel model)
        {
            try
            {
                var result = _business.GetListFileModule(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Import file DMVT
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UploadDesignDocument")]
        [HttpPost]
        public HttpResponseMessage UploadDesignDocument(ImportFileModuleModel model)
        {
            var userId = GetUserIdByRequest();

            _business.UploadDesignDocument(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("GetTreeList")]
        [HttpPost]
        public HttpResponseMessage GetTreeList(ModuleDesignDocumentModel model)
        {
            model.Id = "";
            var data = _business.GetTreeList(model);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("GetChildrenFolderId")]
        [HttpPost]
        public HttpResponseMessage GetChildrenFolderId(ModuleDesignDocumentModel model)
        {
            var data = _business.GetChildrenFolderId(model);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("GetDesigners")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F020127")]
        public HttpResponseMessage GetDesigners(string moduleId)
        {
            var data = _business.GetDesigners(moduleId);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Lấy danh sách công việc theo thiết kế chưa hoàn thành
        /// </summary>
        /// <returns></returns>
        [Route("SearchListPlanDesgin")]
        [HttpGet]
        public HttpResponseMessage SearchListPlanDesgin(string moduleId)
        {
            var data = _business.SearchListPlanDesgin(moduleId);

            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("UpdateListCheckStatus")]
        [HttpPost]
        public HttpResponseMessage UpdateListCheckStatus(List<string> moduleId)
        {
            var createBy = GetUserIdByRequest();
            _business.UpdateListCheckStatus(moduleId, createBy);

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy danh sách công đoạn theo nhóm module
        /// </summary>
        /// <param name="moduleGroupId"></param>
        /// <returns></returns>
        [Route("GetListStageByModuleGroupId")]
        [HttpPost]
        public HttpResponseMessage GetListStageByModuleGroupId(NTS.Model.Module.StageModel model)
        {
            var data = _business.GetListStageByModuleGroupId(model, GetDepartmentIdByRequest());

            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("GetContentModule")]
        [HttpPost]
        public HttpResponseMessage GetContentModule(string moduleId)
        {
            var data = _business.GetContentModule(moduleId);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("UpdateContent")]
        [HttpPost]
        public HttpResponseMessage UpdateContent(ModuleContenModel model)
        {
            _business.UpdateContent(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ImportFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020130")]
        public HttpResponseMessage ImportFile()
        {
            var userId = GetUserIdByRequest();
            var data = HttpContext.Current.Request.Form["isConfirm"];
            var isConfirm = Convert.ToBoolean(data);
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                var result = _business.ImportFile(hfc[0], isConfirm, userId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("SyncSaleModule")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020130")]
        public HttpResponseMessage SyncSaleModule(bool check, bool isConfirm, List<string> listProductId)
        {
            var userId = GetUserIdByRequest();
            var result = _business.SyncSaleModule(check, isConfirm, listProductId, userId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchDocument")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020101;F020102")]
        public HttpResponseMessage SearchDocument(ModuleDocumentSearchModel modelSearch)
        {
            var result = _business.SearchDocument(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListDocumentFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020101;F020102")]
        public HttpResponseMessage GetListDocumentFile(string documentId)
        {
            var result = _business.GetListDocumentFile(documentId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
