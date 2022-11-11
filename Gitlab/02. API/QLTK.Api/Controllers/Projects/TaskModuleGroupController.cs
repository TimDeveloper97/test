using NTS.Model.TaskModuleGroup;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.TaskModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.TaskModuleGroup
{
    [RoutePrefix(prefix: "api/TaskModuleGroups")]
    [ApiHandleExceptionSystem]
    [Authorize]
    public class TaskModuleGroupController : BaseController
    {
        private readonly TaskModuleGroupBussiness _taskModuleGroup = new TaskModuleGroupBussiness();

        [Route("SearchTaskModuleGroups")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060500")]
        public HttpResponseMessage SearchTaskModuleGroups(TaskModuleGroupSearchModel modelSearch)
        {
            var result = _taskModuleGroup.SearchTaskModuleGroups(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("GetTaskModuleGroupInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060502;F060505")]
        public HttpResponseMessage GetTaskModuleGroupInfo(TaskModuleGroupModel model)
        {
            var result = _taskModuleGroup.GetTaskModuleGroupInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("CreateTaskModuleGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060501;F060504")]
        public HttpResponseMessage CreateTaskModuleGroup(TaskModuleGroupModel model)
        {
            //model.CreateBy = GetUserIdByRequest();
            _taskModuleGroup.CreateTaskModuleGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        [Route("UpdateTaskModuleGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060502")]
        public HttpResponseMessage UpdateTaskModuleGroup(TaskModuleGroupModel model)
        {
            // model.UpdateBy = GetUserIdByRequest();
            _taskModuleGroup.UpdateTaskModuleGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        [Route("DeleteTaskModuleGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060503")]
        public HttpResponseMessage DeleteTaskModuleGroup(TaskModuleGroupModel model)
        {
            _taskModuleGroup.DeleteTaskModuleGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
