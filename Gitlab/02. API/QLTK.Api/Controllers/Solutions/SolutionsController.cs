using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.CustomerRequirementMaterial;
using NTS.Model.Solution;
using NTS.Model.SolutionDesignDocuments;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Recruitments;
using QLTK.Business.Solutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Solution
{
    [RoutePrefix("api/Solution")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F070113")]
    public class SolutionsController : BaseController
    {
        private readonly SolutionBussiness _business = new SolutionBussiness();

        [Route("SearchSolution")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F070000")]
        public HttpResponseMessage SearchSolution(SolutionSearchModel modelSearch)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F070105))
            {
                modelSearch.DepartmentId = this.GetDepartmentIdByRequest();
                modelSearch.SBUId = this.GetSbuIdByRequest();
            }
            var result = _business.SearchSolution(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddSolution")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F070101")]
        public HttpResponseMessage AddSolution(SolutionModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.DepartmentId = GetDepartmentIdByRequest();
            _business.AddSolution(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateSolution")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F070102")]
        public HttpResponseMessage UpdateSolution(SolutionModel model)
        {
            var departmentId = GetDepartmentIdByRequest();
            if (!string.IsNullOrEmpty(model.DepartmentId) && !model.DepartmentId.Equals(departmentId))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0034, TextResourceKey.Solution);
            }
            model.UpdateBy = GetUserIdByRequest();
            model.DepartmentId = GetDepartmentIdByRequest();
            _business.UpdateSolution(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteSolution")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F070103")]
        public HttpResponseMessage DeleteSolution(SolutionModel model)
        {
            var departmentId = GetDepartmentIdByRequest();
            _business.DeleteSolution(model, departmentId, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetSolutionInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F070104;F070102;F070105")]
        public HttpResponseMessage GetSolutionInfo(SolutionModel model)
        {
            var result = _business.GetSolutionInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F070106")]
        public HttpResponseMessage ExportExcel(SolutionSearchModel model)
        {
            string path = _business.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("SearchProjectSolution")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F070110")]
        public HttpResponseMessage SearchProjectSolution(ChooseProjectSolutionModel modelSearch)
        {
            var result = _business.SearchProjectSolution(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetSolutionCode")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F070101")]
        public HttpResponseMessage GetSolutionCode(SolutionModel model)
        {
            model.SBUId = GetSBUIdByRequest();
            var result = _business.GetSolutionCode(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListFolderSolution")]
        [HttpGet]
        public HttpResponseMessage GetListFolderSolution(string solutionId, int curentVersion)
        {
            var result = _business.GetListFolderSolution(solutionId, curentVersion);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListFileSolution")]
        [HttpPost]
        public HttpResponseMessage GetListFileSolution(SolutionDesignDocumentModel model)
        {
            var result = _business.GetListFileSolution(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Import file DMVT
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UploadDesignDocument")]
        [HttpPost]
        public HttpResponseMessage UploadDesignDocument(UploadFolderSolutionDesignDocumentModel model)
        {
            var userId = GetUserIdByRequest();

            _business.UploadDesignDocument(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Import file DMVT
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UploadFileDesignDocument")]
        [HttpPost]
        public HttpResponseMessage UploadFileDesignDocument(UploadFileSolutionDesignDocumentModel model)
        {
            var userId = GetUserIdByRequest();

            _business.UploadFileDesignDocument(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("GetSolutionOldVersion")]
        [HttpGet]
        public HttpResponseMessage GetSolutionOldVersion(string solutionId)
        {
            var userId = GetUserIdByRequest();

            var result = _business.GetSolutionOldVersion(solutionId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetDomainById")]
        [HttpGet]
        public HttpResponseMessage GetDomainById(string id)
        {
            var userId = GetUserIdByRequest();

            var result = _business.GetDomainById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetSurveyContentId/{id}")]
        [HttpGet]
        public HttpResponseMessage GetSurveyContentId(string id)
        {
            var result = _business.GetSurveyContentId(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("Checkdelete/{id}")]
        [HttpPost]
        public HttpResponseMessage CheckDeleteSurvey(string id)
        {
            _business.CheckDeleteSurvey(id);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);

        }

        [Route("DeleteMaterial/{id}")]
        [HttpPost]
        public HttpResponseMessage DeleteMaterial(string id)
        {
            _business.DeleteMaterial(id);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);

        }

        [Route("DeleteConten/{id}")]
        [HttpPost]
        public HttpResponseMessage DeleteConten(string id)
        {
            _business.DeleteConten(id);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);

        }

        [Route("saveMaterial")]
        [HttpPost]
        public HttpResponseMessage SaveMaterial(string id,List<CustomerRequirementMaterialInfoModel> model)
        {
            _business.SaveMaterial(id, model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);

        }

        [Route("GetSurveyMaterialId/{id}")]
        [HttpGet]
        public HttpResponseMessage GetSurveyMaterialId(string id)
        {
            var result = _business.GetSurveyMaterialId(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

    }
}
