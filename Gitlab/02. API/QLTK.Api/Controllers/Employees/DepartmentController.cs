using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.NTSDepartment;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Departments;

namespace QLTK.Api.Controllers.Department
{
    [RoutePrefix("api/Department")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F030005")]
    public class DepartmentController : BaseController
    {
        private readonly DepartmentBusiness _department = new DepartmentBusiness();

        [Route("SearchDepartment")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030000")]
        public HttpResponseMessage SearchDepartmentBusiness(DepartmentSearchModel modelSearch)
        {
            var result = _department.SearchDepartment(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteDepartment")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030003")]
        public HttpResponseMessage DeleteDepartment(DepartmentModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _department.DeleteDepartment(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetDepartmentInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030002;F030004")]
        public HttpResponseMessage GetDepartmentInfo(DepartmentModel model)
        {
            var result = _department.GetDepartmentInfo(model);
            if (!result.SBUId.Equals(this.GetSbuIdByRequest()) && !this.CheckPermission(Constants.Permission_Code_F030000))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0030, TextResourceKey.Department);
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddDepartment")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030001")]
        public HttpResponseMessage AddDepartment(DepartmentModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _department.AddDepartment(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateDepartment")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030002")]
        public HttpResponseMessage UpdateDepartment(DepartmentModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _department.UpdateDepartment(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}