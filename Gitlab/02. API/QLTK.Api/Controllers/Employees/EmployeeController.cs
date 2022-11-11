using NTS.Model.Combobox;
using NTS.Model.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using QLTK.Business.Employees;
using QLTK.Api.Controllers.Common;
using QLTK.Api.Attributes;
using NTS.Model.Common;
using QLTK.Api;

namespace NTS.Api.Controllers.Employee
{
    [RoutePrefix("api/Employee")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F080020")]
    public class EmployeeController : BaseController
    {
        private readonly EmployeeBusiness _business = new EmployeeBusiness();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchEmployee")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080000")]
        public HttpResponseMessage SearchEmployee(EmployeeSearchModel modelSearch)
        {
            try
            {
                var result = _business.SearchEmployee(modelSearch);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <param name="Xuất excel"></param>
        /// <returns></returns>
        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080006")]
        public HttpResponseMessage ExportExcel(EmployeeSearchModel model)
        {
            try
            {
                var result = _business.ExportExcel(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Delete employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteEmployee")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080003")]
        public HttpResponseMessage DeleteEmployee(EmployeeModel model)
        {

            _business.DeleteEmployee(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("CreateEmployee")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080001")]
        public HttpResponseMessage CreateEmployee(EmployeeModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _business.CreateEmployee(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("LockEmployee")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080018")]
        public HttpResponseMessage LockEmployee(EmployeeModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _business.LockEmployee(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateEmployee")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080002")]
        public HttpResponseMessage UpdateEmployee(EmployeeModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            //model.DepartmentId = GetDepartmentIdByRequest();
            _business.UpdateEmployee(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetEmployeeInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080002;F080004")]
        public HttpResponseMessage GetEmployeeInfo(EmployeeModel model)
        {
            try
            {
                var result = _business.GetEmployeeInfo(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetEmployeeInfos")]
        [HttpPost]
        public HttpResponseMessage GetEmployeeInfos(EmployeeModel model)
        {
            try
            {
                var result = _business.GetEmployeeInfo(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetPermissionByGroupId")]
        [HttpPost]
        public HttpResponseMessage GetPermissionByGroupId(EmployeeModel model)
        {
            try
            {
                var result = _business.GetPermissionByGroupId(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("ImportFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080005")]
        public HttpResponseMessage ImportFile()
        {
            string userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                _business.ImportFile(userId, hfc[0]);
            }
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetListCourse")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080015;F080002;F080004")]
        public HttpResponseMessage GetListCourse(EmployeeModel modelSearch)
        {
            try
            {
                var result = _business.GetListCourse(modelSearch);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListEmployeeTraining")]
        [HttpPost]
        public HttpResponseMessage GetListEmployeeTraining(EmployeeModel modelSearch)
        {
            try
            {
                var result = _business.GetListEmployeeTraining(modelSearch);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Xuất excel template
        /// </summary>
        /// <param name="Xuất excel"></param>
        /// <returns></returns>
        [Route("ExportExcelTemplate")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080006")]
        public HttpResponseMessage ExportExcelTemplate(EmployeeSearchModel model)
        {
            try
            {
                var result = _business.ExportExcelTemplate(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetRegulation")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F080021")]
        public HttpResponseMessage GetRegulation(EmployeeModel model)
        {
            try
            {
                var result = _business.GetRegulation(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("GetProcedure")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F080021")]
        public HttpResponseMessage GetProcedure(EmployeeModel model)
        {
            try
            {
                var result = _business.GetProcedure(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("GetWorkList")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F080022")]
        public HttpResponseMessage GetWorkList(EmployeeModel model)
        {
            try
            {
                var result = _business.GetWorkList(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}