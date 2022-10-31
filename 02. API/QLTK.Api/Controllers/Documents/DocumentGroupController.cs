using NTS.Model.DocumentGroups;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Documents
{
    [RoutePrefix("api/document-group")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F121454;F121355;F121323")]
    public class DocumentGroupController : BaseController
    {
        private readonly DocumentGroupBussiness _business = new DocumentGroupBussiness();

        /// <summary>
        /// Tìm kiếm nhóm tài liệu
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121450;F121350;F121300")]
        public HttpResponseMessage SearchDocumentGroup(DocumentGroupSearchModel searchModel)
        {
            var result = _business.SearchDocumentGroup(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xóa nhóm tài liệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121453")]
        public HttpResponseMessage Delete(DocumentGroupModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteDocumentGroup(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm nhóm tài liệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121451")]
        public HttpResponseMessage Create(DocumentGroupModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateDocumentGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin nhóm tài liệu
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getDocumentGroupInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121452")]
        public HttpResponseMessage GetInfo(DocumentGroupModel modelSearch)
        {

            var result = _business.GetDocumentGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật nhóm tài liệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121452")]
        public HttpResponseMessage Update(DocumentGroupModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateDocumentGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
