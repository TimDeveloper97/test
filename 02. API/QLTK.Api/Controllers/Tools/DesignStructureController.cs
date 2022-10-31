using NTS.Common;
using NTS.Model.DesignStructure;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.DesignStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.DesignStructure
{
    [RoutePrefix("api/DesignStructure")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F090007")]
    public class DesignStructureController : BaseController
    {
        private readonly DesignStructureBusiness _business = new DesignStructureBusiness();
        [Route("SearchDesignStructure")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090000")]
        public HttpResponseMessage SearchDesignStructure(DesignStructureSearchModel modelSearch)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F090006))
            {
                modelSearch.SBUId = this.GetSBUIdByRequest();
                modelSearch.DepartmentId = this.GetDepartmentIdByRequest();
            }
            var result = _business.SearchDesignStructure(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CreateDesignStructure")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090002")]
        public HttpResponseMessage CreateDesignStructure(DesignStructureModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.DepartmentId = GetDepartmentIdByRequest();
            _business.CreateDesignStructure(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetInfoDesignStructure")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090003;F090005")]
        public HttpResponseMessage GetInfoDesignStructure(DesignStructureModel model)
        {
            var result = _business.GetInfoDesignStructure(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateDesignStructure")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090003")]
        public HttpResponseMessage UpdateDesignStructure(DesignStructureModel model)
        {
            model.DepartmentId = GetDepartmentIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateDesignStructure(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteDesignStructure")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090004")]
        public HttpResponseMessage DeleteDesignStructure(DesignStructureModel model)
        {
            model.DepartmentId = GetDepartmentIdByRequest();
            _business.DeleteStructure(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("CreateDesignStructureFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090001;F090002")]
        public HttpResponseMessage CreateDesignStructureFile(DesignStructureFileModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateDesignStructureFile(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteDesignStructureFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090001;F090002")]
        public HttpResponseMessage DeleteDesignStructureFile(DesignStructureFileModel model)
        {
            var departmentId = GetDepartmentIdByRequest();
            _business.DeleteStructureFile(model, departmentId, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateDesignStructureFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090001;F090002")]
        public HttpResponseMessage UpdateDesignStructureFile(DesignStructureFileModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            var departmentId = GetDepartmentIdByRequest();
            _business.UpdateDesignStructureFile(model, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetInfoDesignStructureFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090003;F090005;F090006")]
        public HttpResponseMessage GetInfoDesignStructureFile(DesignStructureFileModel model)
        {
            var result = _business.GetInfoDesignStructureFile(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetInfoDesignStructureCreate")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090001")]
        public HttpResponseMessage GetInfoDesignStructureCreate(DesignStructureCreateModel model)
        {
            string userId = GetUserIdByRequest();
            var departmentId = GetDepartmentIdByRequest();
            var result = _business.GetInfoDesignStructureCreate(model, userId, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetEmployeeInfoByUserId")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090001;F090002")]
        public HttpResponseMessage GetEmployeeInfoByUserId()
        {
            string userId = GetUserIdByRequest();
            var result = _business.GetEmployeeInfoByUserId(userId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy danh sách thư mục cha
        /// </summary>
        /// <returns></returns>
        [Route("GetFolderParent")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090001;F090002")]
        public HttpResponseMessage GetFolderParent(DesignStructureSearchParentModel searchModel)
        {
            if (string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                searchModel.DepartmentId = GetDepartmentIdByRequest();
            }
            var result = _business.GetFolderParent(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
