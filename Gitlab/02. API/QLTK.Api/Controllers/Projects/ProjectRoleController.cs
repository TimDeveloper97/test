using NTS.Model.ProjectRole;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Projects
{
    [RoutePrefix(prefix: "api/ProjectRole")]
    [ApiHandleExceptionSystem]
    public class ProjectRoleController : BaseController
    {
        private readonly ProjectRoleBusiness _business = new ProjectRoleBusiness();

        [Route("SearchRoles")]
        [HttpPost]
        public HttpResponseMessage SearchRoles(SearchRoleModel search)
        {
            var result = _business.Search(search);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbRole")]
        [HttpPost]
        public HttpResponseMessage GetCbbRole()
        {
            try
            {
                var result = _business.GetCbbRole();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("SearchRole")]
        [HttpGet]
        public HttpResponseMessage SearchRole()
        {
            var result = _business.GetAll();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("SearchPlanFunction")]
        [HttpGet]
        public HttpResponseMessage SearchPlanFunction()
        {
            var result = _business.GetPlanFunctions();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchRoleById/{id}")]
        [HttpGet]
        public HttpResponseMessage SearchRoleId(string id)
        {
            var result = _business.SearchById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetRoleById/{id}")]
        [HttpGet]
        public HttpResponseMessage GetRoleById(string id)
        {
            var result = _business.GetById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CreateRole")]
        [HttpPost]
        public HttpResponseMessage CreateRole(ProjectRoleModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateRole(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateRole")]
        [HttpPost]
        public HttpResponseMessage UpdateRole(ProjectRoleModel model)
        {
            _business.UpdateRole(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteRole")]
        [HttpPost]
        public HttpResponseMessage DeleteRole(ProjectRoleModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteRole(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ExportExcel")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F100004")]
        public HttpResponseMessage ExportExcel(SearchRoleModel model)
        {
            var result = _business.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
