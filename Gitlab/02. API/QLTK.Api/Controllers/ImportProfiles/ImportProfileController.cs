using NTS.Common;
using NTS.Model.Holiday;
using NTS.Model.ImportProfile;
using NTS.Model.ImportProfileDocumentConfigs;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Holidays;
using QLTK.Business.ImportProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ImportProfiles
{
    [RoutePrefix("api/ImportProfile")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F120806")]
    public class ImportProfileController : BaseController
    {
        ImportProfileBussiness _importProfileBussiness = new ImportProfileBussiness();

        [Route("Search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120800")]
        public HttpResponseMessage Search(ImportProfileSearchModel searchModel)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F120804))
            {
                searchModel.EmployeeId = GetEmployeeIdByRequest();
            }
            searchModel.WorkStatus = Constants.ImportProfile_WorkStatus_UnFinish;
            var result = _importProfileBussiness.SearchImportProfile(searchModel, false);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchFinish")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120805")]
        public HttpResponseMessage SearchFinish(ImportProfileSearchModel searchModel)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F120804))
            {
                searchModel.EmployeeId = GetEmployeeIdByRequest();
            }
            searchModel.WorkStatus = Constants.ImportProfile_WorkStatus_Finish;
            searchModel.Step = Constants.ImportProfile_Step_Finish;
            var result = _importProfileBussiness.SearchImportProfile(searchModel, false);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchKanban")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120800")]
        public HttpResponseMessage SearchKanban(ImportProfileSearchModel searchModel)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F120804))
            {
                searchModel.EmployeeId = GetEmployeeIdByRequest();
            }
            var result = _importProfileBussiness.SearchImportProfileKanban(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("Delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120803")]
        public HttpResponseMessage DeleteImportProfile(ImportProfileReusltModel importProfile)
        {
            _importProfileBussiness.DeleteImportProfile(importProfile, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("Create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120801")]
        public HttpResponseMessage CreateImportProfile(ImportProfileCreateModel model)
        {
            _importProfileBussiness.CreateImportProfile(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetById")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120802")]
        public HttpResponseMessage GetById(ChangeStepModel model)
        {
            var result = _importProfileBussiness.GetImportProfileById(model.Id, false);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetViewById")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120807")]
        public HttpResponseMessage GetViewById(ChangeStepModel model)
        {
            var result = _importProfileBussiness.GetImportProfileById(model.Id, true);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("Update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120802")]
        public HttpResponseMessage UpdateImportProfile(ImportProfileUpdateModel model)
        {
            _importProfileBussiness.UpdateImportProfile(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("NextStep")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120802")]
        public HttpResponseMessage NextStep(ImportProfileUpdateModel model)
        {
            _importProfileBussiness.NextStep(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("BackStep")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120802")]
        public HttpResponseMessage BackStep(ChangeStepModel model)
        {
            _importProfileBussiness.BackStep(model.Id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetListFile")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F120802")]
        public HttpResponseMessage GetListFile(ExportListFileModel model)
        {
            var result = _importProfileBussiness.GetListFile(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetImportProfileCode")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120801")]
        public HttpResponseMessage GetImportProfileCode()
        {
            var result = _importProfileBussiness.GetImportProfileCode();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
