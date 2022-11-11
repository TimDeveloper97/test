using NTS.Model.ProjectSolution;
using NTS.Model.Solution;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ProjectSolutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ProjectSolution
{
    [RoutePrefix("api/ProjectSolution")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F060036")]
    public class ProjectSolutionsController : BaseController
    {
        private readonly ProjectSolutionBusiness _business = new ProjectSolutionBusiness();

        [Route("SearchProjectSolution")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060004")]
        public HttpResponseMessage SearchProjectSolution(SolutionSearchModel model)
        {
            var result = _business.SearchProjectSolution(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("SearchSolution")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F060026;F060027")]
        public HttpResponseMessage SearchSolution(SolutionSearchModel model)
        {
            var result = _business.SearchSolution(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Thêm sản phẩm vào giải pháp
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddProjectSolution")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060026;F060027")]
        public HttpResponseMessage AddProjectSolution(ProjectSolutionModel model)
        {
            string userId = GetUserIdByRequest();
           _business.AddProjectSolution(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetProjectProductByProjectId")]
        [HttpPost]
        public HttpResponseMessage GetProjectProductByProjectId(string projectId, string projectSolutionId)
        {
            var result = _business.GetProjectProductByProjectId(projectId, projectSolutionId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("StatusSolutionProduct")]
        [HttpPost]
        public HttpResponseMessage StatusSolutionProduct(string projectId)
        {
            var result = _business.StatusSolutionProduct(projectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
