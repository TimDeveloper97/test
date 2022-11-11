using NTS.Model.ProjectAttch;
using NTS.Model.Projects.ProjectAttch;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ProjectAttch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.ProjectAttch
{
    [RoutePrefix("api/ProjectAttach")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F060036")]
    public class ProjectAttchController : BaseController
    {
        private readonly ProjectAttchBusiness db = new ProjectAttchBusiness();
        /// <summary>
        /// Tìm kiếm
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("GetProjectAttach")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060004")]
        public HttpResponseMessage GetProjectAttach(ProjectAttchSearchModel searchModel)
        {   var userId= GetUserIdByRequest();
            var result = db.GetProjectAttach(searchModel,userId);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Thêm file dự án
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [Route("AddProjectAttach")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060008;F060010")]
        public HttpResponseMessage AddProjectAttach(ProjectAttchInfoModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            db.AddProjectAttach(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Delete 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Delete")]
        [HttpPost]
        public HttpResponseMessage Delete(ProjectAttchModel model)
        {

            db.DeleteProjectAttach(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetGroupInTemplate")]
        [HttpPost]
        public HttpResponseMessage GetGroupInTemplate()
        {
            string path = db.GetGroupInTemplate();
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("ProjectAttachImportFile")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F060038")]
        public HttpResponseMessage ProjectAttachImportFile(string projectId)
        {
            string userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                db.ImportFileProjectAttach(userId, hfc[0], projectId);
            }
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        [Route("ExportExcel")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F060037")]
        public HttpResponseMessage ExportExcelProjectAttach( string projectId)
        {
            string userId = GetUserIdByRequest();
            string path = db.ExportExcelProjectAttach(projectId, userId);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("AddType")]
        [HttpPost]
        public HttpResponseMessage AddType(ProjectAttchTabTypeModel model)
        {
            db.AddType(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("search-document-file")]
        [HttpPost]
        public HttpResponseMessage SearchDocumentFile(ProjectAttchModel searchModel)
        {
            var result = db.SearchDocumentFile(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetAttachProject/{id}")]
        [HttpPost]
        public HttpResponseMessage GetAttachProject(string id)
        {
            var result = db.GetAttachProject(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("GetTypeInfo")]
        [HttpPost]
        public HttpResponseMessage GetUnitInfo(ProjectAttchTabTypeModel model)
        {
            var result = db.GetTypeInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateType")]
        [HttpPost]
        public HttpResponseMessage UpdateType(ProjectAttchTabTypeModel model)
        {
            db.UpdateType(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteType")]
        [HttpPost]
        public HttpResponseMessage DeleteManufacture(string Id)
        {
            db.DeleteType(Id);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("CheckNameProjectAttach")]
        [HttpPost]
        public HttpResponseMessage CheckNameProjectAttach(ProjectAttchModel model)
        {
            db.CheckNameProjectAttach(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);

        }

        [Route("GetProjectAttachInfo")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F060004")]
        public HttpResponseMessage GetProjectAttachInfo(ProjectAttchSearchModel searchModel)
        {
            var userId = GetUserIdByRequest();
            var result = db.GetProjectAttachInfo(searchModel, userId);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }
    }
}
