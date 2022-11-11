using NTS.Model.ImportProfileProblemExist;
using NTS.Model.ReportProblemExist;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ReportProblemExists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ReportProblemExist
{
    [RoutePrefix("api/ReportProblemExist")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F120853")]
    public class ReportProblemExistController : BaseController
    {
        private readonly ReportProblemExistBusiness _reportProblemExist = new ReportProblemExistBusiness();

        //[Route("SearchReportProblemExist")]
        //[HttpPost]
        //[NTSAuthorize(AllowFeature = "F030000")]
        //public HttpResponseMessage SearchReportProblemExistBusiness(ReportProblemExistSearchModel modelSearch)
        //{
        //    var result = _ReportProblemExist.SearchReportProblemExist(modelSearch);
        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}

        //[Route("CreateReportProblemExist")]
        //[HttpPost]
        //[NTSAuthorize(AllowFeature = "F030001")]
        //public HttpResponseMessage CreateReportProblemExist(ReportProblemExistCreateModel model)
        //{
        //    string userId = GetUserIdByRequest();
        //    _ReportProblemExist.CreateReportProblemExist(userId, model);
        //    return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        //}

        //[Route("UpdateReportProblemExist")]
        //[HttpPost]
        //[NTSAuthorize(AllowFeature = "F030002")]
        //public HttpResponseMessage UpdateReportProblemExist(ReportProblemExistCreateModel model)
        //{
        //    string userId = GetUserIdByRequest();
        //    _ReportProblemExist.UpdateReportProblemExist(userId, model);
        //    return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        //}

        //[Route("DeleteReportProblemExist")]
        //[HttpPost]
        //[NTSAuthorize(AllowFeature = "F030003")]
        //public HttpResponseMessage DeleteReportProblemExist(ReportProblemExistCreateModel model)
        //{
        //    string userId = GetUserIdByRequest();
        //    _ReportProblemExist.DeleteReportProblemExist(userId, model);
        //    return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        //}

        //[Route("GetReportProblemExistInfo")]
        //[HttpPost]
        //[NTSAuthorize(AllowFeature = "F030002;F030004")]
        //public HttpResponseMessage GetReportProblemExistInfo(ReportProblemExistCreateModel model)
        //{
        //    var result = _ReportProblemExist.GetReportProblemExistInfo(model);
        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}

        [Route("GetListImportProfileProblemExist")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120851;F120852")]
        public HttpResponseMessage GetListImportProfileProblemExist(ImportProfileProblemExistSearchModel model)
        {
            var result = _reportProblemExist.GetListImportProfileProblemExist(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExcelReportProblemExist")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120852")]
        public HttpResponseMessage ExcelReportProblemExist(ReportProblemExistExcelModel model)
        {
            var result = _reportProblemExist.ExcelReportProblemExist(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
