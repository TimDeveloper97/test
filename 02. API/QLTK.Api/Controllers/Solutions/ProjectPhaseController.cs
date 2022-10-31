using NTS.Model.ProjectPhase;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Solutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ProjectPhase
{
    [RoutePrefix("api/ProjectPhase")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F091205")]
    public class ProjectPhaseController : BaseController
    {
        private readonly ProjectPhaseBusiness _business = new ProjectPhaseBusiness();

        [Route("SearchProjectPhase")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F091200")]
        public HttpResponseMessage SearchProjectPhase(ProjectPhaseSearchModel modelSearch)
        {
            var result = _business.SearchProjectPhase(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddProjectPhase")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F091201")]
        public HttpResponseMessage AddProjectPhase(ProjectPhaseModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddProjectPhase(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetProjectPhase")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F091202;F091204")]
        public HttpResponseMessage GetProjectPhase(ProjectPhaseModel model)
        {
            var result = _business.GetProjectPhase(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateProjectPhase")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F091202")]
        public HttpResponseMessage UpdateProjectPhase(ProjectPhaseModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateProjectPhase(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteProjectPhase")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F091203")]
        public HttpResponseMessage DeleteProjectPhase(ProjectPhaseModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteProjectPhase(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
