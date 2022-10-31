using NTS.Model.Document;
using NTS.Model.DocumentPromulgate;
using NTS.Model.NTSDepartment;
using NTS.Model.TaskFlowStage;
using NTS.Model.WorkType;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Document
{
    [RoutePrefix("api/document")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F121323")]
    public class DocumentController : BaseController
    {
        private readonly DocumentBussiness _business = new DocumentBussiness();

        /// <summary>
        /// Tìm kiếm tài liệu
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121300")]
        public HttpResponseMessage SearcManagehDocument(DocumentSearchModel searchModel)
        {
            var result = _business.SearchManageDocument(searchModel);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }      

        /// <summary>
        /// Thêm tài liệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121301")]
        public HttpResponseMessage CreateDocument(DocumentModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            string id = _business.CreateDocument(model);
            return Request.CreateResponse(HttpStatusCode.OK, id);
        }

        [Route("search-department")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121301;F121302")]
        public HttpResponseMessage SearchDepartment(DepartmentSearchModel modelSearch)
        {
            var result = _business.SearchDepartment(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("search-worktype")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121301;F121302")]
        public HttpResponseMessage SearchWorkType(WorkTypeSearchModel modelSearch)
        {
            var result = _business.SearchWorkType(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("search-task")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121301;F121302")]
        public HttpResponseMessage SearchTask(TaskFlowStageSearchModel modelSearch)
        {
            var result = _business.SearchTask(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Xóa tài liệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121303")]
        public HttpResponseMessage Delete(DocumentModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteDocument(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy danh sách tags
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("get-documenttags")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121301;F121302")]
        public HttpResponseMessage GetDocumentTags()
        {

            var result = _business.GetDocumentTags();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy thông tin tài liệu
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getDocumentInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121302;F121306")]
        public HttpResponseMessage GetDocument(DocumentModel modelSearch)
        {

            var result = _business.GetDocumentInfo(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật tài liệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121302")]
        public HttpResponseMessage Update(DocumentModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateDocument(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Cập nhật file tài liệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update-document-file")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121307;F121308")]
        public HttpResponseMessage UpdateDocumentFile(DocumentUploadModel model)
        {
            var userId = GetUserIdByRequest();
            _business.UpdateDocumentFile(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("search-choose-document")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121307")]
        public HttpResponseMessage SearchChooseDocument(DocumentSearchModel modelSearch)
        {
            var result = _business.SearchChooseDocument(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy thông tin file tài liệu
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getDocumentFile")]
        [HttpPost]

        [NTSAuthorize(AllowFeature = "F121307;F121308;F121309")]
        public HttpResponseMessage GetDocumentFile(DocumentUploadModel modelSearch)
        {

            var result = _business.GetDocumentFile(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("search-document-promulgate")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121312;F121313;F121314;F121315")]
        public HttpResponseMessage SearchDocumentPromulgate(DocumentPromulgateSearchModel modelSearch)
        {
            var result = _business.SearchPromulgate(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Xóa ban hành
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete-promulgate")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121314")]
        public HttpResponseMessage DeletePromulgate(DocumentPromulgateModel model)
        {
            _business.DeleteDocumentPromulgate(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin ban hành
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getDocumentPromulgateInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121313")]
        public HttpResponseMessage GetDocumentPromulgate(DocumentPromulgateModel modelSearch)
        {

            var result = _business.GetDocumentPromulgateInfo(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật ban hành
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update-promulgate")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121313")]
        public HttpResponseMessage UpdatePromulgate(DocumentPromulgateModel model)
        {
            _business.UpdateDocumentPromulgate(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm mới ban hành
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create-promulgate")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121312")]
        public HttpResponseMessage CreatePromulgate(DocumentPromulgateModel model)
        {
            _business.CreateDocumentPromulgate(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("search-document-file")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121324;F121351;F121353")]
        public HttpResponseMessage SearchDocumentFile(DocumentFileModel searchModel)
        {
            var result = _business.SearchDocumentFile(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("cancel-promulgate")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121304")]
        public HttpResponseMessage CancelPromulgate(DocumentModel model)
        {
            _business.CancelPromulgate(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("review-document")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121305")]
        public HttpResponseMessage ReviewDocument(DocumentModel model)
        {
            _business.ReviewDocument(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
