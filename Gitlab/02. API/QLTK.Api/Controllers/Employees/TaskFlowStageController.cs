using NTS.Model.TaskFlowStage;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.TaskFlowStage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.TaskFlowStage
{
    [RoutePrefix("api/task-flow-stage")]
    [ApiHandleExceptionSystem]
    public class TaskFlowStageController : BaseController
    {
        private readonly TaskFlowStageBussiness _business = new TaskFlowStageBussiness();

        /// <summary>
        /// Tìm kiếm công việc
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121550")]
        public HttpResponseMessage SearchTask(TaskFlowStageSearchModel searchModel)
        {
            var result = _business.SearchTask(searchModel,false);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xóa công việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121553")]
        public HttpResponseMessage Delete(TaskFlowStageModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteTask(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm công việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121551")]
        public HttpResponseMessage Create(TaskFlowStageModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateTask(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin công việc
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getTaskInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121552")]
        public HttpResponseMessage GetInfo(TaskFlowStageModel modelSearch)
        {

            var result = _business.GetFlowStageInfo(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy danh sách khóa học
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("get-courses")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121552;F121551")]
        public HttpResponseMessage GetCourses(List<string> skillIds)
        {

            var result = _business.GetCourses(skillIds);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Chỉnh sửa công việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121552")]
        public HttpResponseMessage Update(TaskFlowStageModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateTask(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ExportExcel")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F020106")]
        public HttpResponseMessage ExportExcel(TaskFlowStageSearchModel model)
        {
            var path = _business.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }
    }
}
