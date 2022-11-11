using NTS.Model.ProjectTransferAttach;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ProjectTransferAttachs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ProjectTransferAttach
{
    [RoutePrefix("api/ProjectTransferAttach")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F060036")]
    public class ProjectTransferAttachsController : BaseController
    {
        private readonly ProjectTransferAttachBusiness _business = new ProjectTransferAttachBusiness();

        [Route("SearchProjectTransferAttach")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060004")]
        public HttpResponseMessage SearchProjectTransferAttachs(ProjectTransferAttachAddModel searchModel)
        {
            var result = _business.SearchProjectTransferAttachs(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("AddProjectTransferAttach")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060012;F060014")]
        public HttpResponseMessage AddProjectTransferAttach(ProjectTransferAttachAddModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddProjectTransferAttach(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy list sản phẩm không có nhóm cho 
        /// </summary>
        /// <param name="projectId">tên dự án</param>
        /// <param name="fileId">tên file select</param>
        /// <returns></returns>
        [Route("GetProjectProductToTranfer")]
        [HttpPost]
        public HttpResponseMessage AddProjectTransferAttach(string projectId, string fileId)
        {
            var result = _business.GetProjectProductToTranfer(projectId, fileId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("StatusTrangerProduct")]
        [HttpPost]
        public HttpResponseMessage StatusTrangerProduct(string projectId)
        {
            var result = _business.StatusTranferProduct(projectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("getListPlanTransferByProjectId")]
        [HttpPost]
        public HttpResponseMessage getListPlanTransferByProjectId(string projectId)
        {
            var result = _business.getListPlanTransferByProjectId(projectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdatePlanStatusByProjectId")]
        [HttpPost]
        public HttpResponseMessage updatePlanStatusByProjectId(List<string> plansId)
        {
             _business.updatePlanStatusByProjectId(plansId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
