using NTS.Model.DocumentType;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.DocumentType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.DocumentType
{
    [RoutePrefix("api/document-type")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F121404")]
    public class DocumentTypeController : BaseController
    {
        private readonly DocumentTypeBussiness _business = new DocumentTypeBussiness();

        /// <summary>
        /// Tìm kiếm loại tài liệu
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121400")]
        public HttpResponseMessage SearchDocumentType(DocumentTypeSearchModel searchModel)
        {
            var result = _business.SearchDocumentType(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xóa loại tài liệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121403")]
        public HttpResponseMessage DeleteJobPositions(DocumentTypeModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteDocumentType(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm loại tài liệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121401")]
        public HttpResponseMessage AddJobPositions(DocumentTypeModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateDocumentType(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin loại tài liệu
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getDocumentTypeInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121402")]
        public HttpResponseMessage GetDocumentType(DocumentTypeModel modelSearch)
        {

            var result = _business.GetDocumentType(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật chức vụ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121402")]
        public HttpResponseMessage UpdateJobPositions(DocumentTypeModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateDocumentType(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
