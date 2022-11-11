using NTS.Model.EmployeeGroups;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.EmployeeGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.EmployeeGroups
{
    [RoutePrefix("api/EmployeeGroup")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F080105")]
    public class EmployeeGroupController : BaseController
    {
        private readonly EmployeeGroupBusiness employeeGroupBusiness = new EmployeeGroupBusiness();

        /// <summary>
        /// tìm kiếm
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("GetListEmployeeGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080100")]
        public HttpResponseMessage GetListEmployeeGroup(EmployeeGroupSearchModel modelSearch)
        {
            var result = employeeGroupBusiness.GetListEmployeeGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Xóa nhóm 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteEmployeeGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080103")]
        public HttpResponseMessage DeleteEmployeeGroup(EmployeeGroupModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            employeeGroupBusiness.DeleteEmployeeGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Get dữ liệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("GetEmployeeGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080102;F080104")]
        public HttpResponseMessage GetEmployeeGroup(EmployeeGroupModel model)
        {
            var result = employeeGroupBusiness.GetEmployeeGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddEmployeeGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080101")]
        public HttpResponseMessage AddEmployeeGroup(EmployeeGroupModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            employeeGroupBusiness.AddEmployeeGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateEmployeeGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080102;F080104")]
        public HttpResponseMessage UpdateEmployeeGroup(EmployeeGroupModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            employeeGroupBusiness.UpdateEmployeeGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);

        }
    }
}
