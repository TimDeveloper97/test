using NTS.Model.ModuleGroupTimeStandards;
using NTS.Model.TaskTimeStandardModel;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ModuleGroupTimeStandard;
using QLTK.Business.TaskTimeStandards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.TaskTimeStandard
{
    [RoutePrefix(prefix: "api/TasksTimeStandard")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F060606")]
    public class TaskTimeStandardController : BaseController
    {
        private readonly TaskTimeStandardBussiness _taskTimeStandard = new TaskTimeStandardBussiness();
        private readonly ModuleGroupTimeStandardBussiness _moduleGrTime = new ModuleGroupTimeStandardBussiness();


        [Route("SearchTasksTimeStandard")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060600")]
        public HttpResponseMessage SearchTasks(TaskTimeStandardSearchModel modelSearch)
        {
            var result = _taskTimeStandard.SearchTaskTimeStandard(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("GetTaskTimeStandardInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060602")]
        public HttpResponseMessage GetTaskInfo(TaskTimeStandardModel model)
        {
            var result = _taskTimeStandard.GetTaskTimeStandardInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("CreateTaskTimeStandard")]
        [HttpPost]
        
        public HttpResponseMessage CreateTask(TaskTimeStandardModel model)
        {
            _taskTimeStandard.CreateTaskTimeStandard(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        [Route("UpdateTaskTimeStandard")]
        [HttpPost]
        public HttpResponseMessage UpdateTask(TaskTimeStandardModel model)
        {
            _taskTimeStandard.UpdateTaskTimeStandard(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        [Route("DeleteTaskTimeStandard")]
        [HttpPost]
        public HttpResponseMessage DeleteTask(TaskTimeStandardModel model)
        {
            _taskTimeStandard.DeleteTaskTimeStandard(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        [Route("CreateListTaskTim")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060601")]
        public HttpResponseMessage CreateListTaskTim(TaskTimeStandardResultModel model)
        {
            string userId = GetUserIdByRequest();
            _taskTimeStandard.CreateListTaskTim(model , userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("CreateModuleGroupTimeStandard")]
        [HttpPost]
        public HttpResponseMessage CreateModuleGroupTimeStandard(ModuleGroupTimeStandardsModel model)
        {
            _moduleGrTime.CreateModuleGroupTimeStandard(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateModuleGroupTimeStandard")]
        [HttpPost]
        public HttpResponseMessage UpdateModuleGroupTimeStandard(ModuleGroupTimeStandardsModel model)
        {
            _moduleGrTime.UpdateModuleGroupTimeStandard(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("CalculateAverageTaskTimeStandard")]
        [HttpPost]
        public HttpResponseMessage CalculateAverageTaskTimeStandard(CalculateAverageTaskTimeStandardModel model)
        {
            var rs = _taskTimeStandard.CalculateAverageTaskTimeStandard(model);
            return Request.CreateResponse(HttpStatusCode.OK, rs);
        }

        [HttpPost]
        [Route("ImportTaskTimeStandard")]
        public HttpResponseMessage ImportTaskTimeStandard()
        {
            var userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                _taskTimeStandard.ImportTaskTimeStandard(userId, hfc[0]);
            }

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [HttpPost]
        [Route("ImportExcelTaskTimeStandard")]
        public HttpResponseMessage ImportExcelTaskTimeStandard()
        {
            var userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                _taskTimeStandard.ImportExcelTaskTimeStandard(userId, hfc[0]);
            }

            return Request.CreateResponse(HttpStatusCode.OK, "OK");
        }
    }
}
