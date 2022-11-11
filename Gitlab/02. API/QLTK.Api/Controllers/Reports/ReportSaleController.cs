using Microsoft.AspNet.SignalR;
using NTS.Model.Report;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ReportStatusModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using QLTK.Api.Attributes;
using QLTK.Business.Reports;

namespace QLTK.Api.Controllers.Reports
{
    public class Timeline
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class PostEmployee : Timeline
    {
        public string DepartmentId { get; set; }
        public string[] EmployeesCode { get; set; }
    }

    public class PostDepartment : Timeline
    {
        public string[] DepartmentsCode { get; set; }
    }

    public class PostDepartmentLine: PostDepartment
    {
        public Mode Mode { get; set; }
    }

    public class PostEmployeeLine : PostEmployee
    {
        public Mode Mode { get; set; }
    }


    [RoutePrefix("api/ReportBussiness")]
    [ApiHandleExceptionSystem]
    //[NTSIPAuthorize(AllowFeature = "F100013")]
    public class ReportSaleController : BaseController
    {
        private readonly ReportSaleBussiness _bussiness = new ReportSaleBussiness();

        #region Region
        [HttpPost]
        [Route("SalesTargetRegion")]
        public HttpResponseMessage SalesTargetRegion(Timeline sale)
        {
            try
            {
                var result = _bussiness.SalesTargetRegion(sale.From, sale.To);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("SalesRealityRegion")]
        public HttpResponseMessage SalesRealityByRegion(Timeline sale)
        {
            try
            {
                var result = _bussiness.SalesRealityByRegion(sale.From, sale.To);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region Job - Application - Industry
        [HttpPost]
        [Route("SalesJob")]
        public HttpResponseMessage SalesJob(Timeline sale)
        {
            try
            {
                //var result = _bussiness.SalesJob(new DateTime(2019, 1, 1), new DateTime(2023, 1, 1));
                var result = _bussiness.SalesJob(sale.From, sale.To);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("SalesIndustry")]
        public HttpResponseMessage SalesIndustry(Timeline sale)
        {
            try
            {
                var result = _bussiness.SalesIndustry(sale.From, sale.To);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("SalesApplication")]
        public HttpResponseMessage SalesApplication(Timeline sale)
        {
            try
            {
                var result = _bussiness.SalesApplication(sale.From, sale.To);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region [Column] NV, PB kinh doanh
        [HttpGet]
        [Route("Departments")]
        public HttpResponseMessage Departments()
        {
            try
            {
                var result = _bussiness.Departments();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("Employees")]
        public HttpResponseMessage Employees(string code)
        {
            try
            {
                var result = _bussiness.Employees(code);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("SalesEmployeeByDepartment")]
        public HttpResponseMessage SalesEmployeeByDepartment(PostEmployee post)
        {
            try
            {
                var result = _bussiness.SalesEmployeeByDepartment(post.From, post.To, post.DepartmentId, post.EmployeesCode);
                //var result = _bussiness.SalesEmployeeByDepartment(new DateTime(2019, 1, 1), new DateTime(2023, 1, 1), "b850e6db-db36-406d-a4b5-c166094ac8c2", new string[] { "NV00325", "NV00331", "NV00363" });
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("SalesDepartment")]
        public HttpResponseMessage SalesDepartment(PostDepartment post)
        {
            try
            {
                var result = _bussiness.SalesDepartment(post.From, post.To, post.DepartmentsCode);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

        #region [Line] NV, PB kinh doanh
        [HttpPost]
        [Route("SalesEmployeeByDepartmentLine")]
        public HttpResponseMessage SalesEmployeeByDepartment(PostEmployeeLine line)
        //public HttpResponseMessage SalesEmployeeByDepartment()
        {
            try
            {
                //var result = _bussiness.SalesEmployeeByDepartment(new DateTime(2019,1,1), new DateTime(2023,1,1), 
                //    "b850e6db-db36-406d-a4b5-c166094ac8c2", new string[] { "NV00875", "NV00646", "NV01013", "NV00734" }, Mode.Month);
                var result = _bussiness.SalesEmployeeByDepartment(line.From, line.To, line.DepartmentId, line.EmployeesCode, line.Mode);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("SalesDepartmentLine")]
        public HttpResponseMessage SalesDepartment(PostDepartmentLine line)
        {
            try
            {
                var result = _bussiness.SalesDepartment(line.From, line.To, line.DepartmentsCode, line.Mode);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion
    }
}
