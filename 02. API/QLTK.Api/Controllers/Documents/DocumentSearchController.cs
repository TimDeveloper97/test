using NTS.Common;
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
    [RoutePrefix("api/document-search")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F121355")]
    public class DocumentSearchController : BaseController
    {
        private readonly DocumentBussiness _business = new DocumentBussiness();

        /// <summary>
        /// Tìm kiếm tài liệu
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121350")]
        public HttpResponseMessage SearchDocument(DocumentSearchModel searchModel)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F121353) && !this.CheckPermission(Constants.Permission_Code_F121354))
            {
                searchModel.DepartmentUseId = this.GetDepartmentIdByRequest();
            }

            var result = _business.SearchDocument(searchModel);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
