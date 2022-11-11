using NTS.Model.ImportProfileProblemExist;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ImportProfileProblemExists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ImportProfileProblemExist
{
    [RoutePrefix("api/ReportProblemExist")]
    [ApiHandleExceptionSystem]
    public class ImportProfileProblemExistController : BaseController
    {
        private readonly ImportProfileProblemExistBusiness importProfileProblem = new ImportProfileProblemExistBusiness();

        [Route("SearchImportProfileProblemExist")]
        [HttpPost]
        public HttpResponseMessage SearchImportProfileProblemExist(ImportProfileProblemSearchModel modelSearch)
         {
            var result = importProfileProblem.SearchImportProfileProblemExist(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CreateImportProblemExist")]
        [HttpPost]
        public HttpResponseMessage CreateImportProblemExist(List<ImportProfileProblemExistCreateModel> ListProblem)
        {
            string userId = GetUserIdByRequest();
            importProfileProblem.CreateImportProblemExist(userId, ListProblem);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
