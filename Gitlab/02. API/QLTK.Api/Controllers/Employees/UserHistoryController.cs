using NTS.Common;
using NTS.Model.UserHistoryManage;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.UserHistoryManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.UserHistoryManage
{
    [RoutePrefix("api/userhistory")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F110503")]
    public class UserHistoryController : BaseController
    {
        private readonly UserHistoryBussiness _business = new UserHistoryBussiness();

        [Route("SearchUserHistory")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110501")]
        public HttpResponseMessage SearchUserHistory(UserHistorySearchModel modelSearch)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F110502))
            {
                modelSearch.DepartmentId = this.GetDepartmentIdByRequest();
                modelSearch.SBUId = this.GetSbuIdByRequest();
            }

            var result = _business.SearchUserHistory(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
