using NTS.Common;
using NTS.Model.ImportPR;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ImportPR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.ImportPR
{
    [RoutePrefix("api/ImportPR")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F120754")]
    public class ImportPRController : BaseController
    {
        ImportPRBussiness _importProfileBussiness = new ImportPRBussiness();

        [Route("Search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120750")]
        public HttpResponseMessage Search(ImportPRSearchModel searchModel)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F120753))
            {
                searchModel.EmployeeId = this.GetEmployeeIdByRequest();
            }

            var result = _importProfileBussiness.SearchPR(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchChoose")]
        [HttpPost]
        public HttpResponseMessage SearchChoose(ImportPRSearchModel searchModel)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F120753))
            {
                searchModel.EmployeeId = this.GetEmployeeIdByRequest();
            }

            var result = _importProfileBussiness.SearchChoosePR(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120755")]
        public HttpResponseMessage DeleteProduct(ImportPRModel searchModel)
        {
            _importProfileBussiness.DeleteProduct(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [Route("ImportFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120752")]
        public HttpResponseMessage ImportFile()
        {
            string userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                _importProfileBussiness.ImportFile(userId, hfc[0]);
            }
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}