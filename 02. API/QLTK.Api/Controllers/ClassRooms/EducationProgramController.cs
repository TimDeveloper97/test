using NTS.Model.EducationProgram;
using NTS.Model.Job;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.EducationProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace QLTK.Api.Controllers.EducationProgram
{
    [RoutePrefix("api/EducationProgram")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F050106")]
    public class EducationProgramController : BaseController
    {
        private readonly EducationProgramBussiness  _eduProgram = new EducationProgramBussiness();
        private readonly EducationProgramBussiness _job = new EducationProgramBussiness();

        [Route("SearchEducationProgram")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050100")]
        public HttpResponseMessage SearchEducationProgram(EducationProgramSearchModel modelSearch)
        {
            var result = _eduProgram.SearchEducationProgram(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchJob")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050101;F050102")]
        public HttpResponseMessage SearchMaterial(JobSearchModel modelSearch)
        {
            var result = _job.SearchJob(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("AddEducationProgram")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050101")]
        public HttpResponseMessage AddEducationProgram(EducationProgramModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _eduProgram.AddEducationProgram(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetEducationProgram")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050102;F050104")]
        public HttpResponseMessage GetEducationProgram(EducationProgramModel model)
        {
            var result = _eduProgram.GetIdEducationProgram(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateEducationProgram")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050102")]
        public HttpResponseMessage UpdateEducationProgram(EducationProgramModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _eduProgram.UpdateEducationProgram(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteEducationProgram")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050103")]
        public HttpResponseMessage DeleteEducationProgram(EducationProgramModel model)
        {
            _eduProgram.DeleteEducationProgram(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050105")]
        public HttpResponseMessage ExportExcel(EducationProgramSearchModel model)
        {
            try
            {
                string path = _eduProgram.ExportExcel(model);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
