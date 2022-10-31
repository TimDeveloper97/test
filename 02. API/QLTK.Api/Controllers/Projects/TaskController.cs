using NTS.Model.Task;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Task
{
    [RoutePrefix(prefix: "api/Tasks")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F060506")]
    public class TaskController : BaseController
    {
        private readonly TasksBussiness _task = new TasksBussiness();

        [Route("SearchTasks")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060500")]
        public HttpResponseMessage SearchTasks(TasksSearchModel modelSearch)
        {
            var result = _task.SearchTasks(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("GetTaskInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060502;F060505")]
        public HttpResponseMessage GetTaskInfo(TasksModel model)
        {
            var result = _task.GetTaskInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("CreateTask")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060501")]
        public HttpResponseMessage CreateTask(TasksModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _task.CreateTask(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        [Route("UpdateTask")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060502")]
        public HttpResponseMessage UpdateTask(TasksModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _task.UpdateTask(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        [Route("DeleteTask")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060503")]
        public HttpResponseMessage DeleteTask(TasksModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _task.DeleteTask(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
