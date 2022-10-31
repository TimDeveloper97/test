using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using NTS.Model.IncurredMaterial;
using NTS.Model.ModuleMaterials;
using NTS.Model.ProductStandards;
using NTS.Model.ProjectProducts;
using NTS.Model.Projects.ProjectProducts;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Productstandards;
using QLTK.Business.ProjectProducts;
using QLTK.Business.Recruitments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.ProjectProduct
{
    [RoutePrefix("api/ProjectProduct")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F060036")]
    public class ProjectProductsController : BaseController
    {
        private readonly ProjectProductBusiness _business = new ProjectProductBusiness();
        private readonly ProjectProductQcBusiness _qcbusiness = new ProjectProductQcBusiness();

        [Route("SearchProjectProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060004")]
        public HttpResponseMessage SearchProjectProduct(ProjectProductsSearchModel modelSearch)
        {
            var result = _business.SearchProjectProduct(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchProjectProducts")]
        [HttpPost]
        public HttpResponseMessage SearchProjectProducts(ProjectProductsSearchModel modelSearch)
        {
            var result = _business.SearchProjectProduct(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchProjectProductById")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060004;F060019")]
        public HttpResponseMessage SearchProjectProductById(ProjectProductsSearchModel modelSearch)
        {
            var result = _business.GetProjectProductById(modelSearch.Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CreateProjectProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060016")]
        public HttpResponseMessage CreateProjectProduct(ProjectProductsModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.EmployeeId = GetEmployeeIdByRequest();
            model.SBUId = GetSBUIdByRequest();
            _business.CreateProjectProduct(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateProjectProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060017")]
        public HttpResponseMessage UpdateProjectProduct(ProjectProductsModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            model.EmployeeId = GetEmployeeIdByRequest();
            _business.UpdateProjectProduct(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteProjectProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060018")]
        public HttpResponseMessage DeleteProjectProduct(ProjectProductsModel model)
        {
            _business.DeleteProjectProduct(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetProjectProductInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060017;F060016;F060019")]
        public HttpResponseMessage GetProjectProductInfo(ProjectProductsModel model)
        {
            var result = _business.GetProjectProductInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CompareContract")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060021")]
        public HttpResponseMessage CompareContract(ProjectProductsModel model)
        {
            string path = _business.CompareContract(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        /// <summary>
        /// Import danh sách sản phẩm dự án
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ImportProduct")]
        [NTSAuthorize(AllowFeature = "F060020")]
        public HttpResponseMessage ImportProduct()
        {
            var projectId = HttpContext.Current.Request.Form["ProjectId"];
            //var projectId = JsonConvert.DeserializeObject<string>(modelJson);
            var userId = GetUserIdByRequest();

            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                _business.ImportProduct(userId, hfc[0], projectId);
            }

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [HttpPost]
        [Route("UpdateIsGeneralDesign")]
        [NTSAuthorize(AllowFeature = "F060017")]
        public HttpResponseMessage UpdateIsGeneralDesign(ProjectProductsModel projectProduct)
        {
            _business.UpdateIsGeneralDesign(projectProduct);

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("SearchModuleMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020113")]
        public HttpResponseMessage SearchModuleMaterial(ModuleMaterialSearchModel model)
        {
            var result = _business.SearchModuleMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetIncurredMaterial")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F060004;F060019")]
        public HttpResponseMessage GetIncurredMaterial(IncurredMaterialSearchModel model)
        {
            var result = _business.GetIncurredMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetProjectProductByProjectId/{id}")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F060004;F060019")]
        public HttpResponseMessage GetProjectProductByProjectId(string id)
        {
            var result = _business.GetProjectProductByProjectId(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetNumberErrorOfProject/{id}")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F060004;F060019")]
        public HttpResponseMessage GetNumberErrorOfProject(string id)
        {
            var result = _business.GetNumberErrorOfProject(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetWorkOfProject/{id}")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F060004;F060019")];
        public HttpResponseMessage GetWorkOfProject(string id)
        {
            var result = _business.GetWorkOfProject(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetRatioDoneOfProject/{id}")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F060004;F060019")];
        public HttpResponseMessage GetRatioDoneOfProject(string id)
        {
            var result = _business.GetRatioDoneOfProject(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetImplementationPlanVersusReality/{id}")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F060004;F060019")];
        public HttpResponseMessage GetImplementationPlanVersusReality(string id)
        {
            var result = _business.GetImplementationPlanVersusReality(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetNumberWorkInProject/{id}")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F060004;F060019")];
        public HttpResponseMessage GetNumberWorkInProject(string id)
        {
            var result = _business.GetNumberWorkInProject(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchProjectProductQc")]
        [HttpPost]
        public HttpResponseMessage SearchProjectProductQc(ProjectProductsSearchModel modelSearch)
        {
            var result = _qcbusiness.SearchProjectProduct(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcelQC")]
        [HttpPost]
        public HttpResponseMessage ExportExcelQC(ProjectProductsModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string path = _qcbusiness.ExportExcelQC(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("searchShowProjectProductQc")]
        [HttpPost]
        public HttpResponseMessage searchShowProjectProductQc(ProjectProductsSearchModel modelSearch)
        {
            var result = _qcbusiness.searchShowProjectProductQc(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchCheckList/{id}")]
        [HttpPost]
        public HttpResponseMessage SearchCheckList(string id)
        {
            var result = _business.SearchCheckList(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddChecklisst")]
        [HttpPost]
        public HttpResponseMessage AddChecklisst(ProductStandardsModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            var result = _business.AddChecklisst(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Xóa Tiêu chuẩn QC
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("deleteQCChecklist/{id}")]
        [HttpPost]
        public HttpResponseMessage Delete(string id)
        {
            _business.Delete(id);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("copyQCCheckList")]
        [HttpPost]
        public HttpResponseMessage CopyQCCheckList(CopyQCCheckListModel model)
        {
            var id = GetUserIdByRequest();
            _business.CopyQCCheckList(id, model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("SelectStandard")]
        [HttpPost]
        public HttpResponseMessage SelectStandard(CopyQCCheckListModel model)
        {
            var id = GetUserIdByRequest();
            _business.SelectStandard(id, model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
