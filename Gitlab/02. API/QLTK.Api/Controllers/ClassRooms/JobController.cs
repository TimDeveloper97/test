using QLTK.Api.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QLTK.Business.Jobs;
using NTS.Model.Job;
using QLTK.Api.Controllers.Common;
using NTS.Model.Subjects;

namespace QLTK.Api.Controllers.Job
{
    [RoutePrefix("api/Job")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F050506")]
    public class JobController : BaseController
    {
        private readonly JobBusiness _business = new JobBusiness();

        /// <summary>
        /// Tìm kiếm nghề
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchJob")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050500")]
        public HttpResponseMessage SearchJob(JobSearchModel modelSearch)
        {
            var result = _business.SearchJob(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }


        /// <summary>
        /// Xuất file excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050505")]
        public HttpResponseMessage ExportExcel(JobSearchModel model)
        {

            string path = _business.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);

        }
        /// <summary>
        /// thêm mới nghề
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050501")]
        public HttpResponseMessage Create(JobModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            model.CreateBy = GetUserIdByRequest();
            _business.Create(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        // lấy dữ liệu môn học
        [Route("GetSubject")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050501;F050502")]
        public HttpResponseMessage GetSubject(SubjectsModel modelSearch)
        {

            var result = _business.GetSubject(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        //
        [Route("SearchSubject")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050501;F050502")]
        public HttpResponseMessage SearchSubject(JobModel modelSearch)
        {

            var result = _business.SearchSubject(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetJobInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050502;F050504")]
        public HttpResponseMessage GetJobInfo(JobModel model)
        {
            var result = _business.GetJobInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        // thêm dữ liệu môn học
        [Route("AddJob")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050501")]
        public HttpResponseMessage AddJob(JobModel model)
        {
            _business.AddJob(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Xóa nghề
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteJob")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050503")]
        public HttpResponseMessage DeleteJob(JobModel model)
        {
            _business.DeleteJob(model,GetUserIdByRequest());
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateJob")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050502")]
        public HttpResponseMessage UpdateJob(JobModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateJob(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }


        [Route("GetJobInfor")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050502;F050504")]
        public HttpResponseMessage GetJobInfor(JobModel model)
        {
            var result = _business.GetJobInfor(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetProductClassInfo")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F050502;F050504")]
        public HttpResponseMessage GetProductClassInfor(JobModel model)
        {
            var result = _business.GetProductClassInfor(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetClassRoomByIdSubject")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F050502;F050504")]
        public HttpResponseMessage GetClassRoomByIdSubject(string Id)
        {
            var result = _business.GetClassRoomByIdSubject(Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("getSubjectInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050501;F050502")]
        public HttpResponseMessage GetSubjectInfo(JobModel modelSearch)
        {

            var result = _business.GetSubjectInfo(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
