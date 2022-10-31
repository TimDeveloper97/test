using NTS.Model.ClassRoomTool;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ClassRoomTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ClassRoomTool
{
    [RoutePrefix("api/ClassRoomTool")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F050313")]
    public class ClassRoomToolController : BaseController
    {
        private readonly ClassRoomToolBussiness classRoomToolBussiness = new ClassRoomToolBussiness();

        [Route("GetPracticeAndSkill")]
        [HttpPost]
        public HttpResponseMessage GetPracticeAndSkill(ClassRoomToolPracticeAndSkillModel model)
        {
            var result = classRoomToolBussiness.GetPracticeAndSkill(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetPracticeAndProduct")]
        [HttpPost]
        public HttpResponseMessage GetPracticeAndProduct(ClassRoomToolPracticeAndProductModel model)
        {
            var result = classRoomToolBussiness.GetPracticeAndProduct(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddClassRoomTool")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050312")]
        public HttpResponseMessage AddClassRoomTool(ClassRoomToolModel model)
        {
            var userId = GetUserIdByRequest();
            classRoomToolBussiness.AddClassRoomTool(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetClassRoomToolInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050312")]
        public HttpResponseMessage GetClassRoomToolInfo()
        {
            var userId = GetUserIdByRequest();
            var result = classRoomToolBussiness.GetClassRoomToolInfo(userId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetAutoPracticeWithSkill")]
        [HttpPost]
        public HttpResponseMessage GetAutoPracticeWithSkill(ClassRoomToolPracticeAndSkillModel model)
        {
            var result = classRoomToolBussiness.GetAutoPracticeWithSkill(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetAutoProductWithPractice")]
        [HttpPost]
        public HttpResponseMessage GetAutoProductWithPractice(ClassRoomToolPracticeAndProductModel model)
        {
            var result = classRoomToolBussiness.GetAutoProductWithPractice(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
