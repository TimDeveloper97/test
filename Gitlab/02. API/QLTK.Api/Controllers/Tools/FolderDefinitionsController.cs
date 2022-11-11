using NTS.Common;
using NTS.Model.FolderDefinition;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.FolderDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.FolderDefinition
{
    [RoutePrefix("api/FolderDefinition")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F090608")]
    public class FolderDefinitionsController : BaseController
    {
        private readonly FolderDefinitionBusiness _business = new FolderDefinitionBusiness();

        [Route("AddFolderDefinition")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090601")]
        public HttpResponseMessage AddFolderDefinition(FolderDefinitionModel model)
        {
            var  departmentId = GetDepartmentIdByRequest();
            _business.AddFolderDefinition(model, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateFolderDefinition")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090602")]
        public HttpResponseMessage UpdateFolderDefinition(FolderDefinitionModel model)
        {
        var    departmentId = GetDepartmentIdByRequest();
            _business.UpdateFolderDefinition(model, departmentId, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteFolderDefinition")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090603")]
        public HttpResponseMessage DeleteFolderDefinition(FolderDefinitionModel model)
        {
            var departmentId = GetDepartmentIdByRequest();
            _business.DeleteFolderDefinition(model, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetListFolderDefinition")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090600")]
        public HttpResponseMessage GetListFolderDefinition(FolderDefinitionModel model)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F090608))
            {
                model.DepartmentId = this.GetDepartmentIdByRequest();
            }
            var result = _business.GetListFolderDefinition(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetFolderDefinitionInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090602;F090607")]
        public HttpResponseMessage GetFolderDefinitionInfo(FolderDefinitionModel model)
        {
            var result = _business.GetFolderDefinitionInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetFolderDefinitions")]
        [HttpGet]
        public HttpResponseMessage GetFolderDefinitions(int designType, int objectType)
        {
            var departmentId = this.GetDepartmentIdByRequest();
            var result = _business.GetFolderDefinitions(designType, objectType, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
